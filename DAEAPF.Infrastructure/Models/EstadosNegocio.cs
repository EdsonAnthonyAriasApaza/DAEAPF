using System;
using System.Collections.Generic;

namespace DAEAPF.Infrastructure.Models;

public partial class EstadosNegocio
{
    public int Id { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();
}
