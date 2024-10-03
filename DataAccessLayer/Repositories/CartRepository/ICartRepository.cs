using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.CartRepository {
    public interface ICartRepository {
        Task<Cart> GetCartById(int id);
        Task<Cart> AddCart(Cart cart);
        Task<Cart> AddItemToCart(Cart cart, Product item);
        Task<Cart> RemoveItemFromCart(Cart cart, int itemId);
        Task<CartItem> UpdateCartItemQuantity(int itemId, bool isIncrease, int value);
        Task<Cart> ClearAllItemsFromCart(int id);
        Task RemoveItemFromCartAfterCheckout(List<int> cartItemIds);
        Task<bool> SaveAsync();
    }
}
