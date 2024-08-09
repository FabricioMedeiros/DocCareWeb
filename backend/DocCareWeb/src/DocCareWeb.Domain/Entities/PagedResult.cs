namespace DocCareWeb.Domain.Entities
{
    public class PagedResult<TDto>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<TDto>? Items { get; set; }
    }
}
