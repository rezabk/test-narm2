using Application.ViewModels.Public;
using Common.Enums.ClassEnum;

namespace Application.ViewModels.Class;



public class ShowClassForTeacher
{
    public int Id { get; set; }
    
    public string Description { get; set; }

    public string UniversityName { get; set; }

    public int CurrentStudents { get; set; }
    
    public int TotalAllowedStudent { get; set; }

    public ClassSemesterEnum Semester { get; set; }
    
    public string SemesterTitle { get; set; }
    
    public string CreationDate { get; set; }
}

public class ShowClassForUser : ShowClassForTeacher
{
    
}

public record ResponseGetAllClassByFilterViewModel : ResponseGetListViewModel
{
    public List<ShowClassForTeacher> Items { get; set; }
}