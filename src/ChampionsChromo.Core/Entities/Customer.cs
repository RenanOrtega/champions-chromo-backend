namespace ChampionsChromo.Core.Entities;

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public CustomerAddress? Address { get; set; }
}


public class CustomerAddress
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;
}