using AutoMapper;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Mappings
{
    public class PerfilProfile : Profile
    {
        public PerfilProfile()
        {
            CreateMap<Perfil, PerfilDto>().ReverseMap();
        }
    }
}