using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EXE_API.Services.CartService;
using DataAccessLayer.Shared;
using DataAccessLayer.BusinessModels;
using Microsoft.AspNetCore.Authorization;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<SetupController> _logger;
        private readonly IMapper _mapper;

        public CartsController(
            IMapper mapper,
            ICartService cartService,
            ILogger<SetupController> logger)
        {
            _cartService = cartService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("cart/search/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCartDto>>> GetCartItemsByCartId([FromRoute] int id)
        {
            var cartFromDb = await _cartService.GetCartById(id);
            if (cartFromDb == null)
            {
                return NotFound($"Cart with id {id} is not existed!");
            }
            var response = _mapper.Map<GetCartDto>(cartFromDb.Data);
            return Ok(response);
        }

        [HttpPost]
        [Route("cart/{id}/add/{itemId}")]
        public async Task<ActionResult<ServiceResponse<GetCartDto>>> AddItemToCart([FromRoute] int id, [FromRoute] int itemId)
        {
            var cartFromDb = await _cartService.GetCartById(id);
            if (cartFromDb.Data == null)
            {
                return NotFound($"Cart with id {id} is not existed!");
            }
            var cartResponse = await _cartService.AddItemToCart(cartFromDb.Data, itemId);
            return Ok(cartResponse.Data);
        }

        [HttpPut]
        [Route("cart/update")]
        public async Task<IActionResult> UpdateQuantity([FromQuery] int itemId, [FromQuery] int value, [FromQuery] bool isIncrease)
        {
            await _cartService.UpdateCartItemQuantity(itemId, isIncrease, value);
            return Ok();
        }

        [HttpDelete]
        [Route("cart/clear/{id}/")]
        public async Task<IActionResult> ClearAllItemsFromCart([FromRoute] int id)
        {
            var cart = await _cartService.ClearAllItemsFromCart(id);
            return Ok();
        }

        [HttpDelete]
        [Route("cart/{id}/delete/{itemId}")]
        public async Task<IActionResult> RemoveItemFomCart([FromRoute] int id, [FromRoute] int itemId)
        {
            var cartFromDb = await _cartService.GetCartById(id);
            if (cartFromDb.Data == null)
            {
                return NotFound($"Cart with id {id} is not existed!");
            }
            var cartResponse = await _cartService.RemoveItemFromCart(cartFromDb.Data, itemId);
            return Ok(cartResponse.Data);
        }

        [HttpDelete]
        [Route("cart/delete-after-checkout")]
        public async Task<IActionResult> RemoveItemFromCartAfterCheckout(List<int> cartItemIds) {
            await _cartService.RemoveItemFromCartAfterCheckout(cartItemIds);
            return NoContent();
        }

    }
}
