using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE_API.Services.OrderService;
using EXE101_API.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public OrdersController(
            IUserContext userContext,
            IMapper mapper,
            IOrderService orderService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet]
        [Route("order/user/{userId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetOrderByUserDto>>>> GetOrdersByUserId([FromRoute] string userId)
        {
            try
            {
                var currentUser = _userContext.GetCurrentUser(HttpContext);

                if(currentUser == null || currentUser.UserId != userId) {
                    return Forbid();
                }

                var orders = await _orderService.GetOrdersByUserId(userId);
                var response = _mapper.Map<List<GetOrderByUserDto>>(orders.Data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get the orders: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("order/store/{storeId}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<GetOrderByStoreDto>>>> GetOrdersByStoreId([FromRoute] int storeId)
        {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser == null || currentUser.ManagedStoreId != storeId) {
                return Forbid();
            }

            try
            {
                var orders = await _orderService.GetOrdersByStoreId(storeId);
                var response = _mapper.Map<List<GetOrderByStoreDto>>(orders.Data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get the orders: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("order/{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> GetOrderById([FromRoute] int id)
        {
            try
            {
                var currentUser = _userContext.GetCurrentUser(HttpContext);

                var order = await _orderService.GetOrderById(id);
                var response = _mapper.Map<GetOrderDto>(order.Data);

                if (response.CustomerId != currentUser.UserId && order.Data.StoreId != currentUser.ManagedStoreId) {
                    return Forbid();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get the order: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("order/{id}/state/{state}")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<GetOrderDto>>> UpdateOrderState([FromRoute] int id, [FromRoute] OrderStatus state)
        {
            try
            {
                var order = await _orderService.UpdateOrderState(id, state);
                var response = _mapper.Map<GetOrderDto>(order.Data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get the order: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("order/add")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<AddOrderDto>>> AddOrder([FromBody] AddOrderDto orderDto)
        {
            try
            {
                var currentUser = _userContext.GetCurrentUser(HttpContext);

                if(currentUser == null || currentUser.UserId != orderDto.CustomerId) {
                    return Forbid();
                }

                var order = _mapper.Map<Order>(orderDto);
                var orderResponse = await _orderService.AddOrder(order, orderDto.ShipOption, orderDto.PointToPayment, orderDto.AppliedVoucherCode);

                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to create the order: " + ex.Message);
            }
        }
    }
}
