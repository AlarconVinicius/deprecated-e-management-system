using EMS.WebApp.MVC.Extensions;
using EMS.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace EMS.WebApp.MVC.Services;

public class SubscriptionService : Service, ISubscriptionService
{
    private readonly HttpClient _httpClient;

    public SubscriptionService(HttpClient httpClient,
        IOptions<AppSettings> settings)
    {
        httpClient.BaseAddress = new Uri(settings.Value.SubscriptionUrl!);

        _httpClient = httpClient;
    }

    public async Task<PlanViewModel> GetById(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/plans/{id}");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<PlanViewModel>(response);
    }

    public async Task<IEnumerable<PlanViewModel>> GetAll()
    {
        var response = await _httpClient.GetAsync("/api/plans/");

        HandleErrorResponse(response);

        return await DeserializeResponseObject<IEnumerable<PlanViewModel>>(response);
    }
}