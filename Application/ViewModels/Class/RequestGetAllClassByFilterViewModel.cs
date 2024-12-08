using Application.ViewModels.Public;
using Common.Enums.ClassEnum;

namespace Application.ViewModels.Class;

public record RequestGetAllClassByFilterViewModel : RequestGetListViewModel
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? UniversityName { get; set; }


    public ClassSemesterEnum? Semester { get; set; }
}