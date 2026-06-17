using AutoMapper;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Mappings
{
    public class CalificacionProfile : Profile
    {
        public CalificacionProfile()
        {
            CreateMap<Calificacion, CalificacionDto>();

            CreateMap<CalificacionDto, Calificacion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCalificacion, opt => opt.Ignore());
        } 
    }
}