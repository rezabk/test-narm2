namespace Application.ViewModels.Public;

public record DropDownViewModel<TKey>
{
    public TKey Id { get; set; }
    public string Title { get; set; }
}