using Common.Enums.TeacherEnum;

namespace Application.ViewModels.TeacherRequest;

public class ResponseTeacherRequestViewModel
{
    public TeacherRequestStatusEnum Status { get; set; }

    public string StatusTitle { get; set; }

    public string? Description { get; set; }
}