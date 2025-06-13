using System;
using System.Collections.Generic;

namespace DAEAPF.Infrastructure.Models;

public partial class Producto
{
    public int Id { get; set; }

    public int NegocioId { get; set; }

    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? EstadoId { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual CategoriasProducto Categoria { get; set; } = null!;

    public virtual EstadosProducto? Estado { get; set; }

    public virtual Negocio Negocio { get; set; } = null!;
}
