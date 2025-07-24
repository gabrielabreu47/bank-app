using Microsoft.VisualBasic.CompilerServices;

namespace ClientDirectory.Domain.Common;

public class Filter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Filters { get; set; }
}