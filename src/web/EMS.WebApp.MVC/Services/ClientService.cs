using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace EMS.WebApp.MVC.Services;

public class ClientService : Service, IClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient,
        IOptions<AppSettings> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.UserUrl!);

        _httpClient = httpClient;
    }

    public async Task<ClientViewModel> GetByCpf(string cpf, Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/clients/cpf/{cpf}?userId={userId}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<ClientViewModel>(response);
    }

    public async Task<IEnumerable<ClientViewModel>> GetAll(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/clients?userId={userId}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<IEnumerable<ClientViewModel>>(response);
    }
}