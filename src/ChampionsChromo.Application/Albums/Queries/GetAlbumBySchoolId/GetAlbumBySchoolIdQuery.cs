﻿using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbumBySchoolId;

public record GetAlbumBySchoolIdQuery(string schoolId) : IRequest<Result<IEnumerable<AlbumDto>>>;
