namespace ChampionsChromo.Core.Models
{
    public class CustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public CustomerAddressDto? Address { get; set; }
    }

    public class CustomerAddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Complement { get; set; } = string.Empty;
    }
}
