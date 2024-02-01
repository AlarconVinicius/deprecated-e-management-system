namespace EMS.Users.API.Models.Dtos;

public record ClientDto(Guid Id, string Name, string Email, string Cpf, bool IsDeleted, Guid? SubscriberId);
