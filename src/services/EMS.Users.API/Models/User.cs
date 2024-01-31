using EMS.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EMS.Users.API.Models;

public abstract class User : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public Address? Address { get; private set; }
    public bool IsDeleted { get; private set; }

    protected User() { }

    public User(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = new Email(email);
        IsDeleted = false;
    }

    public User(Guid id, string name, string email, string cpf)
    {
        Id = id;
        Name = name;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        IsDeleted = false;
    }

    public void ChangeEmail(string email)
    {
        Email = new Email(email);
    }
    public void AssignAddress(Address address)
    {
        Address = address;
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
    
}
public class Subscriber : User
{
    [JsonIgnore]
    public ICollection<Worker>? Workers { get; set; }

    [JsonIgnore]
    public ICollection<Client>? Clients { get; set; }
    protected Subscriber() { }

    public Subscriber(Guid id, string name, string email) : base(id, name, email)
    {
    }

    public Subscriber(Guid id, string name, string email, string cpf) : base(id, name, email, cpf)
    {
    }

}
public class Worker : User
{
    public Guid SubscriberId { get; }
    public double Salary { get; private set; }
    public double Commission { get; private set; }
    public string HardSkills { get; private set; }

    [JsonIgnore]
    public Subscriber? Subscriber { get; set; }
    public Worker()
    {
        
    }

    public Worker(Guid id, string name, string email, Guid subscriberId, double salary, double commission, string hardSkills) : base(id, name, email)
    {
        SubscriberId = subscriberId;
        Salary = salary;
        Commission = commission;
        HardSkills = hardSkills;
    }

    public Worker(Guid id, string name, string email, string cpf, Guid subscriberId, double salary, double commission, string hardSkills) : base(id, name, email, cpf)
    {
        SubscriberId = subscriberId;
        Salary = salary;
        Commission = commission;
        HardSkills = hardSkills;
    }
    public void ChangeSalary(double salary)
    {
        Salary = salary;
    }
    public void ChangeCommission(double commission)
    {
        Commission = commission;
    }
    public void ChangeHardSkills(string hardSkills)
    {
        HardSkills = hardSkills;
    }

}
public class Client : User
{
    public Guid SubscriberId { get; }

    [JsonIgnore]
    public Subscriber? Subscriber { get; set; }
    protected Client() { }

    public Client(Guid id, string name, string email, Guid subscriberId) : base(id, name, email)
    {
        SubscriberId = subscriberId;
    }

    public Client(Guid id, string name, string email, string cpf, Guid subscriberId) : base(id, name, email, cpf)
    {
        SubscriberId = subscriberId;
    }

}