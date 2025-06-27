using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs.Usuarios
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Correo { get; set; }

        /// <summary>
        /// Rol permitido: "cliente" o "negocio".
        /// Validar en Service o Controller.
        /// </summary>
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }
    }
}