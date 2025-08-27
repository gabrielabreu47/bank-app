namespace ClientDirectory.Domain.Common;

/// <summary>
/// Represents pagination and filtering options for queries.
/// </summary>
public class Filter
{
    /// <summary>
    /// The current page number for paged results.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// The number of items per page for paged results.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Optional filter string for advanced query filtering.
    /// </summary>
    public string? Filters { get; set; }
}