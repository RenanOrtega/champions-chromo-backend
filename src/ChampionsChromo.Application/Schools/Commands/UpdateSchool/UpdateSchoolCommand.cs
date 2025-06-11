using System.Text.Json.Serialization;
using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.UpdateSchool;

public record UpdateSchoolCommand : IRequest<Result>
{
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Warning { get; set; } = string.Empty;
    public string BgWarningColor { get; set; } = string.Empty;

}
