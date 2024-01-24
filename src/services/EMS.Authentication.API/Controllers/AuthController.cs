using EMS.Authentication.API.Business;
using EMS.Authentication.API.Models;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Authentication.API.Controllers;

[Route("api/auth")]
public class AuthController : MainController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, INotifier notifier) : base(notifier)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Registrar(RegisterUser registerUser)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authService.RegisterUserAsync(registerUser);
        return CustomResponse(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUser usuarioLogin)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authService.LoginUserAsync(usuarioLogin);
        return CustomResponse(result);
    }

    [HttpPost("claim")]
    public async Task<ActionResult> AddClaim(AddUserClaim userClaim)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var result = await _authService.AddClaimAsync(userClaim);
        return CustomResponse(result);
    }
}