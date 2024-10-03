using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;

namespace EXE101_API.Services.FeedbackService {
    public interface IFeedbackService {
        Task<ServiceResponse<Feedback>> AddFeedback(Feedback fb, List<IFormFile>? listFiles);
        Task<ServiceResponse<GetFeedbackPaginationDto>> GetFeedbacksByProductId(int productId, int offset);
        Task<ServiceResponse<bool>> IsAvailableToAddFeedback(int productId, string userId);
        Task DeleteFeedback(int feedbackId);
    }
}
