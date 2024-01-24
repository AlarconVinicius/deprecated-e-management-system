using EMS.WebApp.MVC.Models;
using EMS.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EMS.WebApp.MVC.Controllers;

public class AuthenticationController : MainController
{
    private readonly IAuthService _authService;
    private readonly ISubscriptionService _subscriptionService;

    public AuthenticationController(IAuthService authService, ISubscriptionService subscriptionService)
    {
        _authService = authService;
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    [Route("nova-conta/{id}")]
    public async Task<IActionResult> Register(Guid id)
    {
        var plan = await _subscriptionService.GetById(id);
        var registerUser = new RegisterUser();

        var viewModel = new PlanDetailViewModel
        {
            Plan = plan,
            RegisterUser = registerUser
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("nova-conta/{id}")]
    public async Task<IActionResult> Register(Guid id, RegisterUser registerUser)
    {
        var plan = await _subscriptionService.GetById(id);

        var viewModel = new PlanDetailViewModel
        {
            Plan = plan,
            RegisterUser = registerUser
        };
        if (!ModelState.IsValid) return View(viewModel);

        var resposta = await _authService.Register(registerUser);

        if (HasErrorsInResponse(resposta.ResponseResult!)) return View(viewModel);

        await PerformLogin(resposta);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login(string returnUrl = null!)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginUser loginUser, string returnUrl = null!)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(loginUser);

        var resposta = await _authService.Login(loginUser);

        if (HasErrorsInResponse(resposta.ResponseResult!)) return View(loginUser);

        await PerformLogin(resposta);

        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task PerformLogin(UserResponse resposta)
    {
        var token = GetFormattedToken(resposta.AccessToken!);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", resposta.AccessToken!));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static JwtSecurityToken GetFormattedToken(string jwtToken)
    {
        return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
    }
}