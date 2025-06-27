namespace DAEAPF.Application.DTOs.Productos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int NegocioId { get; set; }
        public string NegocioNombre { get; set; } // opcional, útil para vistas
    }
}