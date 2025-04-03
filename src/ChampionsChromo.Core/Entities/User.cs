namespace ChampionsChromo.Core.Entities;

public class User : Entity
{
    public string FirebaseId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}
