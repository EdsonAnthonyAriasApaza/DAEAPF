namespace DAEAPF.Application.DTOs.Usuarios
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}