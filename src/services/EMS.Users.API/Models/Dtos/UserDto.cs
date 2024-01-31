namespace EMS.Users.API.Models.Dtos;

public record UserAddDto(Guid Id, string Name, string Email, string Cpf, double Salary, double Commission, string HardSkills, bool IsDeleted, EUserType UserType, Address? Address, Guid? SubscriberId);
public record UserUpdDto(Guid Id, string Name, string Email, double Salary, double Commission, string HardSkills, bool IsDeleted, EUserType UserType, Address? Address, Guid? SubscriberId);