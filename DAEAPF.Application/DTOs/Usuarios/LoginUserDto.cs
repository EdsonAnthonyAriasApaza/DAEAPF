using System.ComponentModel.DataAnnotations;

namespace DAEAPF.Application.DTOs;

public class LoginUserDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Contrasena { get; set; }
}