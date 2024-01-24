using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace EMS.WebApp.MVC.Services;

public class AuthService : Service, IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient,
                               IOptions<AppSettings> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl!);

        _httpClient = httpClient;
    }

    public async Task<UserResponse> Login(LoginUser usuarioLogin)
    {
        var loginContent = GetContent(usuarioLogin);

        var response = await _httpClient.PostAsync("/api/auth/login", loginContent);

        if (!HandleErrorResponse(response))
        {
            return new UserResponse
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        return await DeserializeResponseObject<UserResponse>(response);
    }

    public async Task<UserResponse> Register(RegisterUser usuarioRegistro)
    {
        var registroContent = GetContent(usuarioRegistro);

        var response = await _httpClient.PostAsync("/api/auth/register", registroContent);

        if (!HandleErrorResponse(response))
        {
            return new UserResponse
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        return await DeserializeResponseObject<UserResponse>(response);
    }
}