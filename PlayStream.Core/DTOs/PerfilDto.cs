using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayStream.Core.DTOs
{
    public class PerfilDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombrePerfil { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }
}
