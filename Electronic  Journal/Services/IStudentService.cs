

using Electronic__Journal.Models;

namespace Electronic__Journal.Services
{
    public interface IStudentService
    {
        Task<LinkedList<Subject>> GetStudentSubjectsAsync(int groupId);

        Task<LinkedList<Mark>> GetStudentMarksAsync(int studentId);

        Task<LinkedList<Absense>> GetStudentAbsensesAsync(int studentId);
    }
}
