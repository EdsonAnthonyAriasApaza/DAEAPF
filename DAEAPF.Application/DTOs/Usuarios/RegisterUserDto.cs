using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; }

        /// <summary>
        /// Rol permitido: "cliente" o "negocio"
        /// Validar en el Service antes de persistir
        /// </summary>
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }
    }

}