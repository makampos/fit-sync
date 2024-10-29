namespace FitSync.Domain.Results;

public class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T>? items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public IReadOnlyList<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public PagedResult<TVm> ToViewModel<TVm>(Func<T, TVm> converter)
    {
        return new PagedResult<TVm>(
            Items?.Select(converter).ToList(),
            TotalCount,
            PageSize,
            CurrentPage
        );
    }
}