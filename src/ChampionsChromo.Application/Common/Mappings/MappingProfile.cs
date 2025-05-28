using AutoMapper;
using ChampionsChromo.Application.Albums.Queries;
using ChampionsChromo.Application.Cupoms.Queries;
using ChampionsChromo.Application.Schools.Queries;
using ChampionsChromo.Application.StickerCollection.Queries;
using ChampionsChromo.Core.Entities;

namespace ChampionsChromo.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<School, SchoolDto>();

        CreateMap<Album, AlbumDto>();

        CreateMap<UserAlbum, UserAlbumDto>();
        CreateMap<UserAlbumEntry, UserAlbumEntryDto>();

        CreateMap<Cupom, CupomDto>();
    }
}
