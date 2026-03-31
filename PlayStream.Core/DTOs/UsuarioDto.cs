using System;
using System.Collections.Generic;
using System.Text;

namespace PlayStream.Core.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? FechaRegistro { get; set; } 
    }
}
