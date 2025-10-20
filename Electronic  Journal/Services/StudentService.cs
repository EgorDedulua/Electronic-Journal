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
            catch (SqliteException ex)
            {
                _logger.LogError(ex, "Произошла ошибка в базе данных!");
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return null!;
            }
        }

        public async Task<LinkedList<Mark>> GetStudentMarksAsync(int studentId)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    var connectionTask = connection.OpenAsync();
                    string commandText = @"SELECT Marks.Id, Marks.""Date"", Marks.Mark, Subjects.Name AS SubjectName,
                                           group_concat(Users.LastName || ' ' || Users.FirstName || ' ' || Users.MiddleName, ',') AS TeacherName
                                           FROM Marks
                                           JOIN Subjects ON Marks.SubjectId = Subjects.Id
                                           JOIN Users ON Users.Id = Marks.TeacherId
                                           WHERE Marks.StudentId = @studentId
                                           GROUP BY Marks.Id, Marks.""Date"", Subjects.Name;
                                            ";
                    var command = new SqliteCommand(commandText);
                    command.Parameters.AddWithValue("@studentId", studentId);
                    await connectionTask;
                    command.Connection = connection;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            LinkedList<Mark> markList = new LinkedList<Mark>();
                            while (await reader.ReadAsync())
                            {
                                markList.AddLast(new Mark()
                                {
                                    Id = Convert.ToInt32(reader["Id"].ToString()),
                                    StudentId = studentId,
                                    Value = Convert.ToInt32(reader["Mark"]),
                                    SubjectName = reader["SubjectName"].ToString(),
                                    TeacherName = reader["TeacherName"].ToString(),
                                    Date = reader["Date"].ToString()
                                });
                            }
                            return markList;
                        }
                        else
                        {
                            _logger.LogInformation($"Не найдены оценки для пользователя с Id {studentId}");
                            return null!;
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, "Произошла ошибка в базе данных!");
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера");
                return null!;
            }
        }

        public async Task<LinkedList<Absense>> GetStudentAbsensesAsync(int studentId)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                var connectionTask = connection.OpenAsync();
                string commandText = @"SELECT Absenses.Id, Absenses.""Date"", Subjects.Name AS SubjectName,
                                       group_concat(Users.LastName || ' ' || Users.FirstName || ' ' || Users.MiddleName, ',') AS TeacherName
                                       FROM Absenses
                                       JOIN Subjects ON Absenses.SubjectId = Subjects.Id
                                       JOIN Users ON Users.Id = Absenses.TeacherId
                                       WHERE Absenses.StudentId = @studentId
                                       GROUP BY Absenses.Id, Absenses.""Date"", Subjects.Name;";
                var command = new SqliteCommand(commandText);
                command.Parameters.AddWithValue("@studentId", studentId);
                await connectionTask;
                command.Connection = connection;
                using var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    LinkedList<Absense> studentAbsenses = new LinkedList<Absense>();
                    while (await reader.ReadAsync())
                    {
                        studentAbsenses.AddLast(new Absense()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            StudentId = studentId,
                            TeacherName = reader["TeacherName"].ToString(),
                            SubjectName = reader["SubjectName"].ToString(),
                            Date = reader["Date"].ToString()
                        });
                    }
                    return studentAbsenses;
                }
                _logger.LogInformation($"Не найдены отсутствия для студента с id {studentId}");
                return null!;
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, "Произошла ошибка в базе данных!");
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неизветсная ошибка на стороне сервера!");
                return null!;
            }
        }
    }
}
