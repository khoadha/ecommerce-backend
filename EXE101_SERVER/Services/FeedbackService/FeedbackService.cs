using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.FeedbackRepository;
using DataAccessLayer.Shared;

namespace EXE101_API.Services.FeedbackService {
    public class FeedbackService : IFeedbackService {

        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IBlobService blobService, IMapper mapper) {
            _feedbackRepository = feedbackRepository;
            _blobService = blobService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<Feedback>> AddFeedback(Feedback fb, List<IFormFile>? files) {
            var serviceResponse = new ServiceResponse<Feedback>();
            try {
                var fis = new List<FeedbackImage>();
                if(files is not null) {
                    foreach (var file in files) {
                        string callbackUrl = await _blobService.UploadFileAsync(file);
                        FeedbackImage fi = new() {
                            Url = callbackUrl
                        };
                        fis.Add(fi);
                    }
                }
                var rp = await _feedbackRepository.AddFeedback(fb, fis);
                serviceResponse.Data = rp;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task DeleteFeedback(int feedbackId) {
            await _feedbackRepository.DeleteFeedback(feedbackId);
        }

        public async Task<ServiceResponse<GetFeedbackPaginationDto>> GetFeedbacksByProductId(int productId, int offset) {
            var serviceResponse = new ServiceResponse<GetFeedbackPaginationDto>();
            try {
                var rp = await _feedbackRepository.GetFeedbacksByProductId(productId, offset);
                var response = _mapper.Map<List<GetFeedbackDto>>(rp);
                var total = await _feedbackRepository.GetFeedbackCountAsync(productId);

                var paginationData = new GetFeedbackPaginationDto() {
                    Data = response,
                    Total = total
                };

                serviceResponse.Data = paginationData;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsAvailableToAddFeedback(int productId, string userId) {
            var serviceResponse = new ServiceResponse<bool>();
            try {
                var rp = await _feedbackRepository.IsAvailableToAddFeedback(productId, userId);
                serviceResponse.Data = rp;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
