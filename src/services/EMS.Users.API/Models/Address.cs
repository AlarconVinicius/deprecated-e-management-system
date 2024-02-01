using EMS.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace EMS.Users.API.Models;

public class Address : Entity
{
    public Guid UserId { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }


    [JsonIgnore]
    public Subscriber? Subscriber { get; protected set; }

    [JsonIgnore]
    public Worker? Worker { get; protected set; }

    [JsonIgnore]
    public Client? Client { get; protected set; }

    public Address(string street, string number, string complement, string neighborhood, string zipCode, string city, string state)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        ZipCode = zipCode;
        City = city;
        State = state;
    }
}

