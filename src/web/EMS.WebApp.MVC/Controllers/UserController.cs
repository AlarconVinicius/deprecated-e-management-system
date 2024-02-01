using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class UserController : MainController
{
    public IActionResult Index()
    {
        return View();
    }
    [Route("perfil")]
    public IActionResult Profile()
    {
        return View();
    }

    [Route("assinantes")]
    public IActionResult Subscribers()
    {
        return View();
    }

    [Route("colaboradores")]
    public IActionResult Employees()
    {
        return View();
    }
}
