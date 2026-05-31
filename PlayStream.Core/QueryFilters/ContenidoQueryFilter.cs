namespace PlayStream.Core.QueryFilters
{
    public class ContenidoQueryFilter : PaginationQueryFilter
    {
        public string? Categoria { get; set; }
        public int? AnioLanzamiento { get; set; }
        public string? Titulo { get; set; }
    }
}