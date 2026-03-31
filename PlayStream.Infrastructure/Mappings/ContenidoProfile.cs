using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Mappings
{
    public class ContenidoProfile : Profile
    {
        public ContenidoProfile()
        {
            CreateMap<Contenido, ContenidoDto>().ReverseMap();
        }
    }
}