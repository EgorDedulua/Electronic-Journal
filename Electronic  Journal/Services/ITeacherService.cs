using System.Text.RegularExpressions;
using Electronic__Journal.Models;

namespace Electronic__Journal.Services
{
    public interface ITeacherService
    {
        Task<LinkedList<Electronic__Journal.Models.Group>> GetTeacherGroupsAsync(int teacherId);

        Task<LinkedList<Subject>> GetTeacherGroupsSubjectsAsync(int teacherId, int groupId);
    }
}
