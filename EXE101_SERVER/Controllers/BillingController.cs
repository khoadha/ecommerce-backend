using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXE101_API.Controllers {

    [ApiController]
    [Route("api/v1/")]
    public class BillingController : ControllerBase {

        private readonly EXEContext _context;

        public BillingController(EXEContext context) {
            _context = context;
        }

        [HttpGet("billing")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetBillings() {
            var data = await _context
                .Billings
                .OrderByDescending(a=>a.Id)
                .Include(a=>a.Orders)
                .ToListAsync();
            return Ok(data);
        }


        [HttpPut("billing")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> AdminUpdateBilling([FromQuery] int billingId, [FromQuery] int status) {
            var bill = await _context.Billings
                .Include(a=>a.Orders)
                .FirstOrDefaultAsync(a=>a.Id== billingId);

            if(bill.Status==DataAccessLayer.Enums.BillingStatus.Completed || bill.Status == DataAccessLayer.Enums.BillingStatus.Denied) {
                return BadRequest();
            }

            if(status == 0) {
                return BadRequest();
            }

            if (status==1) {
                bill.Status = DataAccessLayer.Enums.BillingStatus.Denied;
            } else if (status==2) {
                bill.Status = DataAccessLayer.Enums.BillingStatus.Completed;
                foreach (var order in bill.Orders) {
                    order.IsAdminPaid = true;
                }
            }
            await _context.SaveChangesAsync();

            var billResult = await _context.Billings
                .Include(a=>a.Orders)
                .ToListAsync();
            return Ok(billResult);
        }

        [HttpPost("billing")]
        public async Task<IActionResult> CreateBilling([FromBody] CreateBillingRequestDto dto) {
            var availableOrders = await _context.Orders
                .Where(a => a.IsAdminPaid == false && a.StoreId == dto.StoreId && a.Status == OrderStatus.Completed)
                .ToListAsync();
            Billing billing = new Billing() {
                StoreId = dto.StoreId,
                CreatedDate = DateTime.Now,
                Orders = availableOrders,
                TotalBill = dto.TotalBill,
                Status = DataAccessLayer.Enums.BillingStatus.Processing
            };
            await _context.Billings.AddAsync(billing);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("billing/{id}")]
        public async Task<IActionResult> GetBillingsByStoreId([FromRoute] int id) {
            var data = await _context.Billings.Where(a => a.StoreId == id).ToListAsync();
            return Ok(data);
        }

        [HttpGet("billing-available-orders")]
        public async Task<IActionResult> GetAvailableOrdersToCreateBilling([FromQuery] int storeId) {
            var store = await _context.Stores.FirstOrDefaultAsync(store => store.Id == storeId);
            var availableOrders = await _context.Orders
                .Where(a => a.IsAdminPaid == false && a.StoreId == storeId && a.Status == OrderStatus.Completed)
                .ToListAsync();
            return Ok(availableOrders);
        }

        [HttpGet("billing-calculate")]
        public async Task<IActionResult> CalculateBilling([FromQuery] int storeId) {
            var store = await _context.Stores.FirstOrDefaultAsync(store => store.Id == storeId);
            var availableOrders = await _context.Orders
                .Where(a => !a.IsAdminPaid && a.StoreId == storeId && a.Status == OrderStatus.Completed)
                .ToListAsync();
            var package = await _context.BillingPackages.FirstOrDefaultAsync(a => a.Id == store.BillingPackageId);

            var totalCost = availableOrders.Sum(order => order.TotalCost);

            var finalCost = totalCost - (totalCost * (decimal)package.Percentage/100);

            var response = new {
                totalCost,
                finalCost
            };
            return Ok(new { response });
        }

        [HttpPut("billing-package-buy")]
        public async Task<IActionResult> StoreBuyPackage([FromQuery] int storeId, [FromQuery] int packageId) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == storeId);
            if (store is not null)
                store.BillingPackageId = packageId;
            store.IsPayPackageDeposit = false;
            store.BillingPackageExpiredDate = DateTime.Now.AddDays(30);

            _context.SaveChanges();
            return Ok(store);
        }

        [HttpGet("billing-package-store/{id}")]
        public async Task<IActionResult> GetStorePackage([FromRoute] int id) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            var response = 0;
            if (store.BillingPackageId is not null) {
                response = store.BillingPackageId.Value;
            }
            return Ok(new { response });
        }

        [HttpGet("billing-package-store-data/{id}")]
        public async Task<IActionResult> GetStorePackageData([FromRoute] int id) {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.Id == id);
            var package = await _context.BillingPackages.FirstOrDefaultAsync(a => a.Id == store.BillingPackageId);
            var response = new {
                Package = package,
                Expired = store.BillingPackageExpiredDate,
                IsPayDeposit = store.IsPayPackageDeposit
            };
            return Ok(new { response });
        }

        [HttpGet("billing-package")]
        public async Task<IActionResult> GetBillingPackages() {
            var data = await _context.BillingPackages.ToListAsync();
            return Ok(data);
        }
    }
}
