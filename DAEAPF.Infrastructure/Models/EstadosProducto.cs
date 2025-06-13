using System;
using System.Collections.Generic;

namespace DAEAPF.Infrastructure.Models;

public partial class EstadosProducto
{
    public int Id { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
