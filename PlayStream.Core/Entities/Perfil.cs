using System;
using System.Collections.Generic;

namespace PlayStream.Core.Entities;

public partial class Perfil : BaseEntity
{

    public int UsuarioId { get; set; }

    public string NombrePerfil { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Usuario Usuario { get; set; } = null!;
}
