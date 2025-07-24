namespace Application.Helpers;

public class Paged<T> where T : class
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    private Paged(List<T> items, int totalRecords, int currentPage, int pageSize)
    {
        Items = items;
        PageNumber = currentPage;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = Convert.ToInt32(Math.Ceiling(totalRecords / (double)pageSize));
    }
    public static Paged<T> Create(List<T> items, int totalRecords, int currentPage, int pageSize)
    {
        return new Paged<T>(items, totalRecords, currentPage, pageSize);
    }

    public static (int skip, int top) GetPagination(int pageNumber, int pageSize)
    {
        var page = pageNumber <= 0 ? 1 : pageNumber;
        var skip = (page - 1) * pageSize;
        return (skip, pageSize);
    }
}