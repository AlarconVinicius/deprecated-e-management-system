using EMS.Users.API.Models;
using FluentValidation.Results;

namespace EMS.Users.API.Business;

public interface ISubscriberService
{
    Task<ValidationResult> AddSubscriber(Subscriber subscriber);
    Task<ValidationResult> UpdateSubscriber(Subscriber subscriber);
    Task<ValidationResult> DeleteSubscriber(Guid id);
    Task<IEnumerable<Subscriber>> GetAllSubscribers();
    Task<Subscriber> GetByCpf(string cpf);
}