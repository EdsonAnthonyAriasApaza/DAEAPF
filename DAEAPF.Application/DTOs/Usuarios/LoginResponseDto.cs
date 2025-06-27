namespace DAEAPF.Application.DTOs.Usuarios
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}