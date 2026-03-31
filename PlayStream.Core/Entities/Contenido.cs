using System;
using System.Collections.Generic;

namespace PlayStream.Core.Entities;

public partial class Contenido : BaseEntity
{

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Categoria { get; set; } = null!;

    public int AnioLanzamiento { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
