using DataAccessLayer.BusinessModels;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace DataAccessLayer.Repositories {
    public class OrderRepository : IOrderRepository {

        private readonly EXEContext _context;

        public OrderRepository(EXEContext context) {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByStoreId(int id) {
            var orders = await _context.Orders
                .Include(a => a.Customer)
                .Include(a => a.Store).Where(o => o.StoreId == id).ToListAsync();
            return orders;
        }

        public async Task<List<Order>> GetOrdersByUserId(string id) {
            var orders = await _context.Orders
                .OrderByDescending(a => a.Id)
                .Include(a => a.Store)
                .Where(o => o.CustomerId == id)
                .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderById(int id) {
            var order = await _context.Orders
                .Include(a => a.Customer)
                .Include(a => a.Store)
                .Include(a => a.OrderItems)
                .ThenInclude(a => a.Product)
                .FirstAsync(o => o.Id == id);
            return order;
        }

        public async Task<Order> UpdateOrderState(int id, OrderStatus state) {
            Order order = new();
            using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    order = await _context.Orders.Include(a => a.OrderItems).FirstAsync(o => o.Id == id);
                    order.Status = state;
                    if (state == OrderStatus.Completed) {
                        order.ShipDate = DateTime.Now;
                        foreach (var item in order.OrderItems) {
                            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == item.ProductId);
                            if (product != null) {
                                product.NumberOfSales += item.Quantity;
                            }
                            _context.Products.Update(product);
                            await SaveAsync();
                        }
                    }
                    _context.Orders.Update(order);
                    await SaveAsync();
                    await transaction.CommitAsync();
                } catch(Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
            return order;
        }

        public async Task<bool> IsUserFirstOrdered(string userId) {
            bool result = false;
            try {
                result = await _context.Orders.AnyAsync(a => a.CustomerId == userId);
                return result;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<Order> AddOrder(Order order, string shipOption, int? pointToPayment, string? appliedVoucherCode) {
            using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    if (shipOption.Equals(ShipOption.WemadePoint)) {
                        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == order.CustomerId);
                        if (user is not null && pointToPayment is not null && user.WemadePoint >= pointToPayment) {
                            user.WemadePoint -= (int)pointToPayment;
                            order.IsPaid = true;
                            if (appliedVoucherCode != null) {
                                var voucher = await _context.Vouchers.FirstOrDefaultAsync(a => a.Name.Equals(appliedVoucherCode));
                                if (voucher != null) {
                                    voucher.AvailableCount -= 1;
                                    order.AppliedVoucherId = voucher.Id;
                                }
                            }

                            await SaveAsync();
                        } else {
                            throw new Exception("Error while create order.");
                        }
                    } else if (shipOption.Equals(ShipOption.COD)) {
                        order.IsPaid = false;
                    }
                    order.OrderDate = DateTime.Now;
                    order.Status = OrderStatus.Processing;
                    order.IsAdminPaid = false;
                    await _context.Orders.AddAsync(order);
                    foreach (var orderProduct in order.OrderItems) {
                        orderProduct.OrderId = order.Id;
                        _context.OrdersProducts.Add(orderProduct);
                    }
                    await SaveAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
            return order;
        }

        public async Task<Statistic> GetStatistic(Store store, DateTime? fromDate, DateTime? toDate) {
            Statistic response = new() {
                StoreId = store.Id,
                Revenue = 0,
                ShippingCost = 0,
                OrderCount = 0
            };
            try {
                var ordersOfStore = await _context.Orders
                    .Where(o => o.StoreId == store.Id && o.Status == OrderStatus.Completed)
                    .ToListAsync();

                if (fromDate != null && toDate != null) {
                    ordersOfStore = ordersOfStore.Where(a => a.OrderDate >= fromDate && a.OrderDate <= toDate)
                        .ToList();
                }
                foreach (var order in ordersOfStore) {
                    response.Revenue += order.TotalCost;
                    response.ShippingCost += order.ShippingCost;
                    response.OrderCount++;
                }
            } catch (Exception ex) {
                throw;
            }
            return response;
        }

        public async Task<List<DailyRevenue>> GetRevenueFromLastMonth(Store store) {
            try {
                DateTime? fromDate = DateTime.Now.AddMonths(-1);
                DateTime? toDate = DateTime.Now;

                var ordersOfStore = await _context.Orders
                    .Where(o => o.StoreId == store.Id && o.Status == OrderStatus.Completed)
                    .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                    .ToListAsync();

                var dailyRevenues = ordersOfStore
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new DailyRevenue {
                        Date = g.Key,
                        TotalRevenue = g.Sum(o => o.TotalCost)
                    })
                    .OrderBy(dr => dr.Date)
                    .ToList();

                return dailyRevenues;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<OrderCountStatistic> GetOrderCountStatistic(Store store) {
            var response = new OrderCountStatistic();
            try {
                var orderCounts = await _context.Orders
                    .Where(o => o.StoreId == store.Id)
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();
                foreach (var orderCount in orderCounts) {
                    switch (orderCount.Status) {
                        case OrderStatus.Completed:
                            response.CompletedCount = orderCount.Count;
                            break;
                        case OrderStatus.Canceled:
                            response.CanceledCount = orderCount.Count;
                            break;
                        case OrderStatus.Processing:
                            response.ProcessingCount = orderCount.Count;
                            break;
                        case OrderStatus.Confirmed:
                            response.ConfirmedCount = orderCount.Count;
                            break;
                    }
                }
            } catch (Exception ex) {
                throw;
            }
            return response;
        }

        public async Task<TopProductStatistic> GetTopProductStatistic(Store store) {
            var response = new TopProductStatistic();
            try {
                var listByView = await _context.Products.Where(a=>a.StoreId == store.Id).OrderByDescending(a=>a.ViewCount).Take(6).ToListAsync();
                var listBySale = await _context.Products.Where(a => a.StoreId == store.Id).OrderByDescending(a => a.NumberOfSales).Take(6).ToListAsync();
                response.TopProductsByViewCount = listByView;
                response.TopProductsBySale = listBySale;
            } catch (Exception ex) {
                throw;
            }
            return response;
        }



        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OrderCountStatistic> GetOrderCountStatistic() {
            var response = new OrderCountStatistic();
            try {
                var orderCounts = await _context.Orders
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();
                foreach (var orderCount in orderCounts) {
                    switch (orderCount.Status) {
                        case OrderStatus.Completed:
                            response.CompletedCount = orderCount.Count;
                            break;
                        case OrderStatus.Canceled:
                            response.CanceledCount = orderCount.Count;
                            break;
                        case OrderStatus.Processing:
                            response.ProcessingCount = orderCount.Count;
                            break;
                        case OrderStatus.Confirmed:
                            response.ConfirmedCount = orderCount.Count;
                            break;
                    }
                }
            } catch (Exception ex) {
                throw;
            }
            return response;
        }

        public async Task<List<DailyRevenue>> GetRevenueFromLastMonth() {
            try {
                DateTime? fromDate = DateTime.Now.AddMonths(-1);
                DateTime? toDate = DateTime.Now;
                var ordersOfStore = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Completed)
                    .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                    .ToListAsync();

                var dailyRevenues = ordersOfStore
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new DailyRevenue {
                        Date = g.Key,
                        TotalRevenue = g.Sum(o => o.TotalCost)
                    })
                    .OrderBy(dr => dr.Date)
                    .ToList();

                return dailyRevenues;
            } catch (Exception ex) {
                throw;
            }
        }

        public AdminDashboardInformation GetAdminDashboardInformation() {
            try {
                var totalRevenue = _context.Orders
                    .Where(o => o.Status == OrderStatus.Completed).Sum(a => a.TotalCost);
                var shopCount = _context.Stores.Count();
                var productCount = _context.Products.Count();
                var orderCount = _context.Orders.Count();
                var result = new AdminDashboardInformation() { 
                    TotalOrderCount = orderCount,
                    TotalPlatformRevenue = totalRevenue,
                    TotalProductCount = productCount,
                    TotalShopCount = shopCount
                };
                return result;
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
