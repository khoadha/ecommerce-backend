using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.FeedbackRepository {
    public class FeedbackRepository : IFeedbackRepository {

        private readonly EXEContext _context;

        public FeedbackRepository(EXEContext context) {
            _context = context;
        }

        public async Task<Feedback> AddFeedback(Feedback fb, List<FeedbackImage>? listImages) {
            try {
                fb.CreatedDate = DateTime.Now;
                fb.FeedbackImages = listImages;
                var addedFeedback = await _context.Feedbacks.AddAsync(fb);
                await SaveAsync();
                return addedFeedback.Entity;
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task DeleteFeedback(int feedbackId) {
            try {
                var feedback = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == feedbackId);
                if (feedback != null) {
                    _context.Feedbacks.Remove(feedback);
                    await SaveAsync();
                }
            } catch (Exception ex) {
                throw;
            }
        }

        public async Task<List<Feedback>> GetFeedbacksByProductId(int productId, int offset) {
            int sizePerPage = 5;
            return await _context.Feedbacks
                .Include(a => a.FeedbackImages)
                .Include(a=>a.User)
                .Where(a => a.ProductId == productId)
                .OrderByDescending(a=>a.Id)
                .Skip(sizePerPage * offset)
                .Take(sizePerPage)
                .ToListAsync();
        }

        public async Task<int> GetFeedbackCountAsync(int productId) {
            return await _context.Feedbacks
                .Where(a => a.ProductId == productId)
                .CountAsync();
        }

        public async Task<bool> IsAvailableToAddFeedback(int productId, string userId) {
            bool result = true;
            try {
                result = await _context.OrdersProducts
               .Include(op => op.Order)
               .AnyAsync(op => op.Order.CustomerId == userId && 
                         op.Order.Status == OrderStatus.Completed && 
                         op.ProductId == productId);
            } catch (Exception ex) {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
