using EMS.Users.API.Business.Interfaces.Service;
using EMS.Users.API.Models.Dtos;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Users.API.Controllers
{
    [Route("api/clients")]
    public class ClientsController : MainController
    {
        private readonly IClientService _clientService;

        public ClientsController(INotifier notifier, IClientService clientService) : base(notifier)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(ClientAddDto client)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _clientService.AddClient(client);
            return CustomResponse();
        }

        [HttpPut()]
        public async Task<IActionResult> PutClient(ClientUpdDto client)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _clientService.UpdateClient(client);
            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id, Guid userId)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _clientService.DeleteClient(id, userId);
            return CustomResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(Guid userId)
        {
            return CustomResponse(await _clientService.GetAllClients(userId));
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetClientByCpf(string cpf, Guid userId)
        {
            return CustomResponse(await _clientService.GetByCpf(cpf, userId));
        }

    }
}
