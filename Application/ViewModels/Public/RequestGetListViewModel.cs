using Common;

namespace Application.ViewModels.Public;

public record RequestGetListViewModel : IPaginate
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}