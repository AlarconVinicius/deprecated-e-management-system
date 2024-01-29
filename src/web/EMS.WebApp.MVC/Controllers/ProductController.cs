using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class ProductController : MainController
{
    [Route("produtos")]
    public IActionResult Products()
    {
        return View();
    }
}
