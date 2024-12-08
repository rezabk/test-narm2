using System.ComponentModel;

namespace Common.Enums.TeacherEnum;

public enum TeacherRequestStatusEnum
{
    [Description("جدید")] New = 1,
    [Description("عدم تایید")] Rejected = 2,
    [Description("تایید شده")] Approved = 3,
}