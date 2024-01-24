using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Services;

public interface IAuthService
{
    Task<UserResponse> Login(LoginUser usuarioLogin);
    Task<UserResponse> Register(RegisterUser usuarioRegistro);
}