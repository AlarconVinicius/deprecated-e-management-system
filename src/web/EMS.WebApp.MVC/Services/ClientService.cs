using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Models;
using Microsoft.Extensions.Options;
using System.Net;

namespace EMS.WebApp.MVC.Services;

public class ClientService : Service, IClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient,
        IOptions<AppSettings> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.ClientUrl!);

        _httpClient = httpClient;
    }

    public async Task<ClientViewModel> GetByCpf(string cpf, Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/clients/cpf/{cpf}?userId={userId}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<ClientViewModel>(response);
    }

    public async Task<IEnumerable<ClientViewModel>> GetAll(Guid userId)
    {
        var response = await _httpClient.GetAsync($"/api/clients?userId={userId}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<IEnumerable<ClientViewModel>>(response);
    }

    public async Task<ClientResponse> AddClient(ClientViewModel client)
    {
        var clientContent = GetContent(client);

        var response = await _httpClient.PostAsync("/api/clients", clientContent);

        if (!HandleErrorResponse(response))
        {
            return new ClientResponse
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return new ClientResponse();
        }

        return await DeserializeResponseObject<ClientResponse>(response);
    }

    public async Task<ClientResponse> UpdateClient(ClientViewModel client)
    {
        var clientContent = GetContent(client);

        var response = await _httpClient.PutAsync("/api/clients", clientContent);

        if (!HandleErrorResponse(response))
        {
            return new ClientResponse
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return new ClientResponse();
        }

        return await DeserializeResponseObject<ClientResponse>(response);
    }

    public async Task<ClientResponse> DeleteClient(Guid id, Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"/api/clients/{id}?userId={userId}");

        if (!HandleErrorResponse(response))
        {
            return new ClientResponse
            {
                ResponseResult = await DeserializeResponseObject<ResponseResult>(response)
            };
        }


        return new ClientResponse();
    }
}