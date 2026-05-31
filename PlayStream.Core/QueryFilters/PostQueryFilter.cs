namespace PlayStream.Core.QueryFilters
{
    public class PostQueryFilter : PaginationQueryFilter
    {
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }
}
