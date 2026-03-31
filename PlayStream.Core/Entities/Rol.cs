using System;
using System.Collections.Generic;

namespace PlayStream.Core.Entities;

public partial class Rol : BaseEntity
{

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
