﻿using ChampionsChromo.Application.Common.Models;
using MediatR;

namespace ChampionsChromo.Application.Schools.Commands.CreateSchool;

public record CreateSchoolCommand : IRequest<Result>
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
}
