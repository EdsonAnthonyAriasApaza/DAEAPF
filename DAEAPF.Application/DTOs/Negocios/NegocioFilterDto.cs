namespace DAEAPF.Application.DTOs.Negocios
{
    public class NegocioFilterDto
    {
        public string? Categoria { get; set; }

        //public string Distrito { get; set; }  si tu dirección incluye distrito

        public int? EstadoId { get; set; }

        public string? NombreBusqueda { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}