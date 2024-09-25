namespace StudentManagementSystem.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name";
        public string SortDirection { get; set; } = "asc";
        public string Filter { get; set; }
    }

}
