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
                    string commandText = "SELECT SubjectId FROM Groups_Subjects WHERE GroupId=@groupId";
                    var command = new SqliteCommand(commandText);
                    command.Parameters.Add(new SqliteParameter("@groupId", groupId));
                    await connectionTask;
                    command.Connection = connection;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            int subjectId;
                            string subjectName;
                            commandText = "SELECT Name FROM Subjects WHERE Id=@id";
                            command = new SqliteCommand(commandText, connection);
                            LinkedList<Subject> subjectList = new LinkedList<Subject>();
                            while (await reader.ReadAsync())
                            {
                                subjectId = Convert.ToInt32(reader["SubjectId"]);
                                command.Parameters.AddWithValue("@id", subjectId);
                                subjectName= Convert.ToString(await command.ExecuteScalarAsync())!;
                                subjectList.AddLast(new Subject() { Id = subjectId,  Name = subjectName});
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
