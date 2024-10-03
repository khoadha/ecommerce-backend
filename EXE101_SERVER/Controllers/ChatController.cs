using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.ChatRepository;
using DataAccessLayer.Shared;

using EXE_API.Services.CartService;
using EXE101_API.Services.CategoryMaterialService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ChatController : ControllerBase
    {
    
        private readonly IChatRepository _chatRepository;

        private readonly ILogger<SetupController> _logger;
        private readonly IMapper _mapper;

        public ChatController(IMapper mapper,ILogger<SetupController> logger, IChatRepository repo)
        {
            _chatRepository = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(ApplicationUserDto model)
        {
          
            if (_chatRepository.AddUserToList(model.Email))
            {
                return NoContent();
            }
            return BadRequest("User is logined in");
        }
    }
}
