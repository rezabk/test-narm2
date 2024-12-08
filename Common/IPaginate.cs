namespace Common;

public interface IPaginate
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}