using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models.Dtos;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Users.API.Controllers
{
    [Route("api/users")]
    public class UsersController : MainController
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;

        public UsersController(INotifier notifier, IUserService userService, IClientService clientService) : base(notifier)
        {
            _userService = userService;
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddDto user)
        {
            await _userService.AddUser(user);
            return CustomResponse(user);
        }

        [HttpPut()]
        public async Task<IActionResult> PutClient(UserUpdDto user)
        {
            return CustomResponse(await _userService.UpdateUser(user));
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients(Guid userId)
        {
            return CustomResponse(await _clientService.GetAllClients(userId));
        }

        [HttpGet("clients/cpf/{cpf}")]
        public async Task<IActionResult> GetClientByCpf(string cpf, Guid userId)
        {
            return CustomResponse(await _clientService.GetByCpf(cpf, userId));
        }

    }
}
