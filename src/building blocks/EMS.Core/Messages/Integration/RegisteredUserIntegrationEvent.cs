namespace EMS.Core.Messages.Integration;

public class RegisteredUserIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; private set; }
    public Guid PlanId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    public bool IsActive { get; private set; }

    public RegisteredUserIntegrationEvent(Guid id, Guid planId, string name, string email, string cpf, bool isActive)
    {
        Id = id;
        PlanId = planId;
        Name = name;
        Email = email;
        Cpf = cpf;
        IsActive = isActive;
    }
}