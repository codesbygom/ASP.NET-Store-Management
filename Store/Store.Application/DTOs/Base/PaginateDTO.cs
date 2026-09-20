namespace Store.Application.DTOs.Base
{
    public class PaginateDTO<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
