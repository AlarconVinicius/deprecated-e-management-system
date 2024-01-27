namespace EMS.Core.Messages.Integration;

public class DeletedUserIntegrationEvent : IntegrationEvent
{
    public string Cpf { get; private set; }

    public DeletedUserIntegrationEvent(string cpf)
    {
        Cpf = cpf;
    }
}