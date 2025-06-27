using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs.Negocios
{
    public class UpdateNegocioDto
    {
        [Required]
        [MinLength(2)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public int EstadoId { get; set; }
    }
}