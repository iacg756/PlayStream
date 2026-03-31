using System;
using System.Collections.Generic;
    
namespace PlayStream.Core.Entities;

public partial class Usuario : BaseEntity
{
    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public int RolId { get; set; }

    public virtual ICollection<Perfil> Perfils { get; set; } = new List<Perfil>();

    public virtual Rol Rol { get; set; } = null!;
}
