using EMS.WebAPI.Core.User;
using EMS.WebApp.MVC.Models;
using EMS.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
[Route("cliente")]
public class ClientController : MainController
{
    private readonly IClientService _clientService;
    private readonly IAspNetUser _appUser;

    public ClientController(IClientService clientService, IAspNetUser appUser)
    {
        _clientService = clientService;
        _appUser = appUser;
    }

    public async Task<IActionResult> Index()
    {
        if (!_appUser.IsAuthenticated()) return RedirectToAction("Index", "Home");
        IEnumerable<ClientViewModel> clients = await _clientService.GetAll(_appUser.GetUserId());

        var viewModel = new ClientViewModels(new ClientViewModel(), clients);
        return View(viewModel);
    }

    [Route("detalhes/{cpf}")]
    public async Task<IActionResult> Details(string cpf)
    {
        var user = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        return View(user);
    }

    [Route("adicionar")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("adicionar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,Cpf,IsDeleted,SubscriberId")] ClientViewModel client)
    {
        if (!ModelState.IsValid) return View(client);

        var response = await _clientService.AddClient(client);

        if (HasErrorsInResponse(response.ResponseResult!)) return View(client);

        return RedirectToAction("Index", "Client");
    }

    [Route("editar/{cpf}")]
    public async Task<IActionResult> Edit(string cpf)
    {
        var client = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    [HttpPost]
    [Route("editar/{cpf}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string cpf, [Bind("Id,Name,Email,Cpf,IsDeleted,SubscriberId")] ClientViewModel client)
    {
        if (!ModelState.IsValid) return View(client);

        var clientDb = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        clientDb.Name = client.Name;
        clientDb.Email = client.Email;
        var response = await _clientService.UpdateClient(clientDb);

        if (HasErrorsInResponse(response.ResponseResult!)) return View(client);

        return RedirectToAction("Index", "Client");
    }

    [HttpPost("deletar/{cpf}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string cpf)
    {
        var userId = _appUser.GetUserId();
        var user = await _clientService.GetByCpf(cpf, userId);

        var response = await _clientService.DeleteClient(user.Id, userId);

        if (HasErrorsInResponse(response.ResponseResult!)) return View("Index");

        return RedirectToAction("Index", "Client");
    }

    [HttpPost]
    [Route("bloquear/{cpf}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlockClient(string cpf)
    {
        var clientDb = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        clientDb.IsDeleted = !clientDb.IsDeleted;
        var response = await _clientService.UpdateClient(clientDb);

        if (HasErrorsInResponse(response.ResponseResult!)) return View();

        return RedirectToAction("Index", "Client");
    }
}