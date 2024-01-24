using EMS.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EMS.Subscription.API.Model;

public class Plan : Entity, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    [JsonIgnore]
    public ICollection<PlanUser> PlanUsers { get; set; } = new List<PlanUser>();

    public Plan() { }
}