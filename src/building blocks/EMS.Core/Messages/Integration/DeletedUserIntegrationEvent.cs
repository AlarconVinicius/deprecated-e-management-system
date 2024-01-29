namespace EMS.Core.Messages.Integration;

public class DeletedUserIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; private set; }

    public DeletedUserIntegrationEvent(Guid id)
    {
        AggregateId = id;
        Id = id;
    }
}