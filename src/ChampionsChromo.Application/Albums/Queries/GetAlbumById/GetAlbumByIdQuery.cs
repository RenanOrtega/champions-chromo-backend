﻿using ChampionsChromo.Application.Common.Models;
using ChampionsChromo.Core.Models;
using MediatR;

namespace ChampionsChromo.Application.Albums.Queries.GetAlbumById;

public record GetAlbumByIdQuery(string Id) : IRequest<Result<AlbumDto>>;
