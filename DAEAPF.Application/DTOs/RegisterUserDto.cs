namespace DAEAPF.Application.DTOs;

public class RegisterUserDto
{
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }
    public string Rol { get; set; } = "cliente"; // por defecto
}