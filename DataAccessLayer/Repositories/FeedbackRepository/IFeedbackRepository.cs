using DataAccessLayer.Models;
namespace DataAccessLayer.Repositories.FeedbackRepository {
    public interface IFeedbackRepository {
        Task<Feedback> AddFeedback(Feedback fb, List<FeedbackImage>? listImages);
        Task<List<Feedback>> GetFeedbacksByProductId(int productId, int offset);
        Task<int> GetFeedbackCountAsync(int productId);
        Task<bool> IsAvailableToAddFeedback(int productId, string userId);
        Task DeleteFeedback(int feedbackId);
    }
}
