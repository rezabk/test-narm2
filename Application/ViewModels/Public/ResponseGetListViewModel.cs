namespace Application.ViewModels.Public;

public record ResponseGetListViewModel
{
    public int Count { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
}

public record PaginationResult<T> : ResponseGetListViewModel
{
    public List<T>? Items { get; set; }
}