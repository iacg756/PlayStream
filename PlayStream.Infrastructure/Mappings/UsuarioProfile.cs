using System;
using System.Globalization;
using AutoMapper;
using PlayStream.Core.DTOs;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.FechaRegistro,
                    // Le decimos explícitamente a AutoMapper de dónde sacar la info
                    opt => opt.ConvertUsing(new DateTimeToStringConverter(), src => src.FechaRegistro));

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.FechaRegistro,
                    // Le decimos explícitamente a AutoMapper de dónde sacar la info
                    opt => opt.ConvertUsing(new StringToDateTimeConverter(), src => src.FechaRegistro));
        }
    }

    // Le agregamos el '?' al DateTime para que no explote si la fecha viene vacía o es Nula en la BD
    public class DateTimeToStringConverter : IValueConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, ResolutionContext context)
        {
            if (!source.HasValue) return string.Empty;
            return source.Value.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    public class StringToDateTimeConverter : IValueConverter<string, DateTime?>
    {
        public DateTime? Convert(string source, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            source = source.Trim();
            source = source.Replace("a. m.", "AM").Replace("p. m.", "PM")
                           .Replace("a.m.", "AM").Replace("p.m.", "PM")
                           .Replace("am", "AM").Replace("pm", "PM");

            string[] formats = new[]
            {
                "dd-MM-yyyy", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy hh:mm:ss tt",
                "dd-MM-yyyy H:mm:ss", "dd-MM-yyyy h:mm:ss tt", "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd hh:mm:ss tt"
            };

            if (DateTime.TryParseExact(source, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            if (DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;

            throw new FormatException($"No se pudo convertir la fecha '{source}' a DateTime.");
        }
    }
}