using EkonLayer.API.GlobalEvents;
using EkonLayer.Core.DbModels;
using EkonLayer.Core.Services;
using EkonLayer.Helpers.Models.CustomModels;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Logging.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EkonLayer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User, UserDto> _userService;
        private readonly LogWorker _logger;

        public UserController(IGenericService<User, UserDto> userService, LogWorker logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<BaseResponse<IEnumerable<UserDto>>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<BaseResponse<UserDto>> GetById(int id)
        {
            return await _userService.GetByIdAsync(id);
        }
    }
}
