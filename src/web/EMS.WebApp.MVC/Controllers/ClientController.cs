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
        var user = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        return View(user);
    }

    [Route("deletar/{cpf}")]
    public async Task<IActionResult> Delete(string cpf)
    {
        var user = await _clientService.GetByCpf(cpf, _appUser.GetUserId());
        return View(user);
    }
}