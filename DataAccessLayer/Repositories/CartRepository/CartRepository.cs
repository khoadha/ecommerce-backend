using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.CartRepository {
    public class CartRepository : ICartRepository {

        private readonly EXEContext _context;

        public CartRepository(EXEContext context) {
            _context = context;
        }

        public async Task<Cart> AddCart(Cart cart) {
            try {
                await _context.Carts.AddAsync(cart);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return cart;
        }

        public async Task<Cart> ClearAllItemsFromCart(int id) {
            var cart = await _context.Carts
              .Include(c => c.Items)
              .FirstOrDefaultAsync(a => a.Id == id);

            foreach (var item in cart.Items.ToList()) {
                _context.CartItems.Remove(item);
            }

            await SaveAsync();
            return cart;
        }

        public async Task<Cart> AddItemToCart(Cart cart, Product item) {
            try {
                if (cart != null) {
                    CartItem cartItem = new CartItem {
                        ProductId = item.Id,
                        Quantity = 1,
                        CartId = cart.Id
                    };
                    cart.Items.Add(cartItem);
                    await SaveAsync();
                }
            } catch (Exception ex) {
                throw;
            }
            return cart;
        }

        public async Task<Cart> RemoveItemFromCart(Cart cart, int itemId) {
            try {
                if (cart != null) {
                    var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == itemId);
                    cart.Items.Remove(cartItem);
                    await SaveAsync();
                }
            } catch (Exception ex) {
                throw;
            }
            return cart;
        }

        public async Task<Cart> GetCartById(int id) {
            var cart = await _context.Carts
              .Include(c => c.Items)
              .ThenInclude(a => a.Product)
              .ThenInclude(a => a.Store)
              .FirstOrDefaultAsync(a => a.Id == id);
            return cart;
        }

        public async Task<CartItem> UpdateCartItemQuantity(int itemId, bool isIncrease, int value) {
            var cartItem = await _context.CartItems.FirstAsync(a => a.Id == itemId);
            try {
                if (isIncrease == true) {
                    cartItem.Quantity += value;
                } else if (isIncrease == false) {
                    cartItem.Quantity -= value;
                }
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return cartItem;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task RemoveItemFromCartAfterCheckout(List<int> cartItemIds) {
            try {
                var cartItems = await _context.CartItems.Where(a => cartItemIds.Contains(a.Id)).ToListAsync();
                _context.CartItems.RemoveRange(cartItems);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
