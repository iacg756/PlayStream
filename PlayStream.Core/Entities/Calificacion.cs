using System;
using System.Collections.Generic;

namespace PlayStream.Core.Entities;

public partial class Calificacion : BaseEntity
{

    public int PerfilId { get; set; }

    public int ContenidoId { get; set; }

    public int Puntuacion { get; set; }

    public string? Comentario { get; set; }

    public virtual Contenido Contenido { get; set; } = null!;

    public virtual Perfil Perfil { get; set; } = null!;
}
