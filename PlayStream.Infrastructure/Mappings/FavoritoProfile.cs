using AutoMapper;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;
namespace PlayStream.Infrastructure.Mappings
{
    public class FavoritoProfile : Profile
    {
        public FavoritoProfile() { CreateMap<Favorito, FavoritoDto>().ReverseMap(); }
    }
}