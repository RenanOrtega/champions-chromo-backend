﻿namespace ChampionsChromo.Core.Options;

public class JwtOptions
{
    public const string JwtOptionsKey = "Jwt";

    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
}
