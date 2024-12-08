namespace Application.ViewModels.Public;

public record SelectOptionViewModel
{
    public int? Id { get; set; }
    public string Title { get; set; }
}

public record UnionWithUnionUseType : SelectOptionViewModel
{
    public int UnionUseTypeId { get; set; }
}

public record UnionWithCountyUnion : SelectOptionViewModel
{
    public int CountyUnionId { get; set; }
}

public record UnionWithCountiesUnion : UnionWithCountyUnion
{
    public int CountyId { get; set; }
}

public record CountyWithProvince : SelectOptionViewModel
{
    public string ProvinceTitle { get; set; }
    public int ProvinceId { get; set; }
}

public record CityOrRuralWithCountyAndProvince : CountyWithProvince
{
    public int CountyId { get; set; }
    public string CountyTitle { get; set; }
}

public record RuralWithVillagesSelectOption : SelectOptionViewModel
{
    public List<SelectOptionViewModel> Villages { get; set; }
}