using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs.Productos
{
    public class CreateProductDto
    {
        [Required]
        [MinLength(2)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        public int NegocioId { get; set; }
    }
}