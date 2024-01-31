using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models.Dtos;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Users.API.Controllers
{
    [Route("api/users")]
    public class UsersController : MainController
    {
        private readonly IUserService _userService;

        public UsersController(INotifier notifier, IUserService userService) : base(notifier)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddDto user)
        {
            await _userService.AddUser(user);
            return CustomResponse(user);
        }
    }
}
