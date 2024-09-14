namespace Implementation_Pagination_and_Filtering.Models
{
    public class ProductQueryParameters
    {
        public string Category { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
