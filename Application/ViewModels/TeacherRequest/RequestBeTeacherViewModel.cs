using Common.Enums.TeacherEnum;

namespace Application.ViewModels.Teacher;

public class RequestBeTeacherViewModel
{
    public string? Description { get; set; }

    public TeacherFieldEnum Field { get; set; }
}