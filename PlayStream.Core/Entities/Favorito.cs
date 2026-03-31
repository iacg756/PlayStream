using System;
using System.Collections.Generic;

namespace PlayStream.Core.Entities;

public partial class Favorito : BaseEntity
{

    public int PerfilId { get; set; }

    public int ContenidoId { get; set; }

    public virtual Contenido Contenido { get; set; } = null!;

    public virtual Perfil Perfil { get; set; } = null!;
}
