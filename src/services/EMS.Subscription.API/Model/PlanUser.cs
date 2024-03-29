﻿using EMS.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EMS.Subscription.API.Model;

public class PlanUser : Entity, IAggregateRoot
{
    public Guid PlanId { get; set; }
    public Guid ClientId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserCpf { get; set; } = string.Empty;  
    public bool IsActive { get; set; }

    [JsonIgnore]
    public Plan? Plan { get; set; }

    public PlanUser() { }

    public PlanUser(Guid planId, Guid clientId, string username, string userEmail, string userCpf, bool isActive)
    {
        PlanId = planId;
        ClientId = clientId;
        UserName = username;
        UserEmail = userEmail;
        UserCpf = userCpf;
        IsActive = isActive;
    }
}