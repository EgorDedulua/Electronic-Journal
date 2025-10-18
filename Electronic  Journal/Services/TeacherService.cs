using Microsoft.Data.Sqlite;
using Electronic__Journal.Models;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Electronic__Journal.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly string _connectionString;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IConfiguration configuration, ILogger<TeacherService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        public async Task<LinkedList<Electronic__Journal.Models.Group>> GetTeacherGroupsAsync(int teacherId)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    var connectionTask = connection.OpenAsync();
                    string commandText = @"SELECT DISTINCT Groups_Subjects.GroupId, Groups.Name FROM Groups_Subjects
                                           JOIN Groups ON Groups.Id = Groups_Subjects.GroupId
                                           WHERE Groups_Subjects.TeacherId = @teacherId";
                    var command = new SqliteCommand(commandText);
                    command.Parameters.AddWithValue("@teacherId", teacherId);
                    await connectionTask;
                    command.Connection = connection;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            LinkedList<Electronic__Journal.Models.Group> teacherGroups = new LinkedList<Electronic__Journal.Models.Group>();
                            while (await reader.ReadAsync())
                            {
                                teacherGroups.AddLast(new Electronic__Journal.Models.Group() { Id = Convert.ToInt32(reader["GroupId"]), Name = reader["Name"].ToString() });
                            }
                            return teacherGroups;
                        }
                        else
                        {
                            _logger.LogError($"Не найдены группы для преподавателя с Id {teacherId}");
                            return null!;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неизвестная ошибка на стороне сервера!");
                return null!;
            }
        }
    }
}
