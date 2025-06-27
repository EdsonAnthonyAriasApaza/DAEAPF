using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs.Negocios
{
    public class CreateNegocioDto
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

        // Estado opcional al crear, o se asigna por defecto en el servicio
        public int? EstadoId { get; set; }
    }
}