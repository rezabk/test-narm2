using Common.Enums.TeacherEnum;

namespace Application.ViewModels.Teacher;

public class ShowTeacherForUser
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public TeacherFieldEnum TeacherField { get; set; }
    public string TeacherFieldTitle { get; set; }
    
    public int TotalClasses { get; set; }
}

public class ResponseGetTeacherViewModel
{
    
}