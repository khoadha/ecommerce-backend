using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.CartRepository;
using Microsoft.EntityFrameworkCore;

namespace EXE_API.Services.CartService
{
    public class CartService : ICartService {

        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo) {
            _productRepo = productRepo;
            _cartRepo = cartRepo;
        }

        public async Task<ServiceResponse<Cart>> AddCart(Cart cart) {
            var serviceResponse = new ServiceResponse<Cart>();
            try {
                var response = await _cartRepo.AddCart(cart);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Cart>> GetCartById(int id) {
            var serviceResponse = new ServiceResponse<Cart>();
            try {
                var response = await _cartRepo.GetCartById(id);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Cart>> ClearAllItemsFromCart(int id) {
            var serviceResponse = new ServiceResponse<Cart>();
            try {
                var response = await _cartRepo.ClearAllItemsFromCart(id);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Cart>> AddItemToCart(Cart cart, int itemId) {
            var serviceResponse = new ServiceResponse<Cart>();
            try {
                var item = await _productRepo.GetProductById(itemId);
                var response = await _cartRepo.AddItemToCart(cart, item);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Cart>> RemoveItemFromCart(Cart cart, int itemId) {
            var serviceResponse = new ServiceResponse<Cart>();
            try {
                var response = await _cartRepo.RemoveItemFromCart(cart, itemId);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<CartItem>> UpdateCartItemQuantity(int itemId, bool isIncrease, int value) {
            var serviceResponse = new ServiceResponse<CartItem>();
            try {
                var response = await _cartRepo.UpdateCartItemQuantity(itemId, isIncrease, value);
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SaveAsync() {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var response = await _cartRepo.SaveAsync();
                serviceResponse.Data = response;
            } catch (DbUpdateException ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task RemoveItemFromCartAfterCheckout(List<int> cartItemIds) {
             await _cartRepo.RemoveItemFromCartAfterCheckout(cartItemIds);
        }
    }
}
