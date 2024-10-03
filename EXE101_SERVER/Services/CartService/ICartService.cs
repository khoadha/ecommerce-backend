using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using Newtonsoft.Json.Linq;

namespace EXE_API.Services.CartService
{
    public interface ICartService {
        Task<ServiceResponse<Cart>> GetCartById(int id);
        Task<ServiceResponse<Cart>> AddCart(Cart cart);
        Task<ServiceResponse<Cart>> AddItemToCart(Cart cart, int itemId);
        Task<ServiceResponse<Cart>> ClearAllItemsFromCart(int id);
        Task<ServiceResponse<Cart>> RemoveItemFromCart(Cart cart, int itemId);
        Task RemoveItemFromCartAfterCheckout(List<int> cartItemIds);
        Task<ServiceResponse<CartItem>> UpdateCartItemQuantity(int itemId, bool isIncrease, int value);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
