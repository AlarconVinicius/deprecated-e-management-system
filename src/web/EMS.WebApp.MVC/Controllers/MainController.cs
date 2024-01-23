using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

public class MainController : Controller
{
    protected bool HasErrorsInResponse(ResponseResult response)
    {
        if (response != null && response.Errors!.Messages!.Any())
        {
            foreach (var message in response.Errors!.Messages!)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            return true;
        }

        return false;
    }

}