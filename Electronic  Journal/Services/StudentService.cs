using Electronic__Journal.Models;
using Microsoft.Data.Sqlite;

namespace Electronic__Journal.Services
{
    public class StudentService : IStudentService
    {
        private readonly string _connectionString;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IConfiguration configuration, ILogger<StudentService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        public async Task<LinkedList<Subject>> GetStudentSubjectsAsync(int groupId)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    var connectionTask = connection.OpenAsync();
                    string commandText = @"SELECT Groups_Subjects.SubjectId, Subjects.Name FROM Groups_Subjects
                                           JOIN Subjects ON Subjects.Id = Groups_Subjects.SubjectId
                                           WHERE Groups_Subjects.GroupId = @groupId";
                    var command = new SqliteCommand(commandText);
                    command.Parameters.Add(new SqliteParameter("@groupId", groupId));
                    await connectionTask;
                    command.Connection = connection;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        { 
                            LinkedList<Subject> subjectList = new LinkedList<Subject>();
                            while (await reader.ReadAsync())
                            {
                                subjectList.AddLast(new Subject() { Id = Convert.ToInt32(reader["SubjectId"]),  Name = reader["Name"].ToString() });
                            }
                            return subjectList;
                        }
                        else
                        {
                            _logger.LogError($"Не найдены предметы для групппы {groupId}");
                            return null!;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return null!;
            }
        }
    }
}
