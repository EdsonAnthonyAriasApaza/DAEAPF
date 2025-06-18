using System;
using System.Collections.Generic;

namespace DAEAPF.Domain.Models;
public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public string? Rol { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();
}
