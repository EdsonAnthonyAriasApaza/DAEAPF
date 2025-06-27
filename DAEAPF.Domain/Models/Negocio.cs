using System;
using System.Collections.Generic;

namespace DAEAPF.Domain.Models;
public partial class Negocio
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Categoria { get; set; }

    public int? EstadoId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual EstadosNegocio? Estado { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual Usuario Usuario { get; set; } = null!;
}
