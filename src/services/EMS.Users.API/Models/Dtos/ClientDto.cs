namespace EMS.Users.API.Models.Dtos;

public record ClientDto(Guid Id, string Name, string Email, string Cpf, bool IsDeleted, Guid? SubscriberId);
public record ClientAddDto(Guid Id, Guid SubscriberId, string Name, string Email, string Cpf,bool IsDeleted,Address? Address);
public record ClientUpdDto(Guid Id, Guid SubscriberId, string Name, string Email, bool IsDeleted, Address? Address);
