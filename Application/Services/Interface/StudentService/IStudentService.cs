using Application.ViewModels.Class;
using Application.ViewModels.Public;
using Application.ViewModels.Teacher;

namespace Application.Services.Interface.StudentService;

public interface IStudentService
{
    public Task<List<ShowTeacherForUser>> GetAllTeacher();

    Task<List<ShowClassForUser>> GetAllClassByTeacherId(int teacherId);

    Task<bool> JoinClass(int classId);
}