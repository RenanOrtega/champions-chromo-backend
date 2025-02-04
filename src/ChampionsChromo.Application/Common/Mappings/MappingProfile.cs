using AutoMapper;
using ChampionsChromo.Application.Albums.Queries;
using ChampionsChromo.Application.Schools.Queries;
using ChampionsChromo.Application.StickerCollection.Queries;
using ChampionsChromo.Application.Users.Queries;
using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<School, SchoolDto>();

        CreateMap<Album, AlbumDto>();

        CreateMap<User, UserDto>();

        CreateMap<UserAlbum, UserAlbumDto>();
        CreateMap<UserAlbumEntry, UserAlbumEntryDto>();
    }
}
