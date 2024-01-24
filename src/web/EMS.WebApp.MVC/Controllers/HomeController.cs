using EMS.WebApp.MVC.Models;
using EMS.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ISubscriptionService _subscriptionService;

    public HomeController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IActionResult> Index()
    {
        var plans = await _subscriptionService.GetAll();
        return View(plans);
    }
    [Route("plano-detalhe/{id}")]
    public async Task<IActionResult> PlanDetail(Guid id)
    {
        var plan = await _subscriptionService.GetById(id);
        var registerUser = new RegisterUser();

        var viewModel = new PlanUserViewModel
        {
            Plan = plan,
            RegisterUser = registerUser
        };

        return View(viewModel);
    }

    [Route("sistema-indisponivel")]
    public IActionResult SistemaIndisponivel()
    {
        var modelErro = new ErrorViewModel
        {
            Message = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários.",
            Title = "Sistema indisponível.",
            ErrorCode = 500
        };

        return View("Error", modelErro);
    }


    [Route("erro/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if (id == 500)
        {
            modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.Title = "Ocorreu um erro!";
            modelErro.ErrorCode = id;
        }
        else if (id == 404)
        {
            modelErro.Message =
                "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.Title = "Ops! Página não encontrada.";
            modelErro.ErrorCode = id;
        }
        else if (id == 403)
        {
            modelErro.Message = "Você não tem permissão para fazer isto.";
            modelErro.Title = "Acesso Negado";
            modelErro.ErrorCode = id;
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelErro);
    }
}