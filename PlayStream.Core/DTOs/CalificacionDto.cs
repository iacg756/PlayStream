namespace PlayStream.Core.DTOs
{
    public class CalificacionDto
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public int ContenidoId { get; set; }
        public int Puntuacion { get; set; }
        public string? Comentario { get; set; }
    }
}