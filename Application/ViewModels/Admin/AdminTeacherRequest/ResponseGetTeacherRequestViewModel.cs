using Common.Enums.TeacherEnum;

namespace Application.ViewModels.Admin.AdminTeacherRequest;

public class ResponseGetTeacherRequestViewModel
{
}

public class ShowTeacherRequestForAdmin
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; }

    public string? Description { get; set; }

    public TeacherFieldEnum Field { get; set; }

    public string FieldTitle { get; set; }

    public TeacherRequestStatusEnum Status { get; set; }
    
    public string StatusTitle { get; set; }
    
    public string? RequestTime { get; set; }
        
}