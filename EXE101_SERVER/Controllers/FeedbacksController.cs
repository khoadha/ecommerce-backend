using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_API.Services.FeedbackService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers {
    [ApiController]
    [Route("api/v1/")]
    public class FeedbacksController : ControllerBase {

        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbacksController(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }

        [HttpGet("feedback/{productId}")]
        public async Task<IActionResult> GetFeedbacksByProductId([FromRoute] int productId ,[FromQuery] int offset) {

            var response = await _feedbackService.GetFeedbacksByProductId(productId , offset);
            
            return Ok(response.Data);
        }

        [HttpGet("feedback/available")]
        [Authorize]
        public async Task<IActionResult> IsAvailableToAddFeedback([FromQuery] int id, [FromQuery] string userId) {

            var serviceResponse = await _feedbackService.IsAvailableToAddFeedback(id, userId);

            var data = serviceResponse.Data;

            return Ok(new { data });
        }

        [HttpPost("feedback")]
        [Authorize]
        public async Task<IActionResult> AddFeedback([FromForm] AddFeedbackDto dto) {

            var fb = _mapper.Map<Feedback>(dto);

            var serviceResponse = await _feedbackService.AddFeedback(fb, dto.Files);

            var response = _mapper.Map<GetFeedbackDto>(serviceResponse.Data);

            return Ok(response);
        }

        [HttpDelete("feedback/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id) {
            await _feedbackService.DeleteFeedback(id);
            return NoContent();
        }
    }
}
