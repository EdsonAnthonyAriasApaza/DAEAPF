namespace DAEAPF.Application.DTOs.Negocios
{
    public class NegocioResponseDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Categoria { get; set; }

        public string Estado { get; set; } // Nombre del estado (ej: "Abierto", "Cerrado")

        public DateTime FechaCreacion { get; set; }

        public int DueñoId { get; set; }

        public string DueñoNombre { get; set; }

        // Opcional: Productos reducidos
        public List<ProductoSimpleDto> Productos { get; set; }
    }

    public class ProductoSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string ImagenUrl { get; set; }
    }
}