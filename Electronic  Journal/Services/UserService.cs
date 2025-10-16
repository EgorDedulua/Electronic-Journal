using Electronic__Journal.Models;
using Microsoft.Data.Sqlite;

namespace Electronic__Journal.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;
        private readonly ILogger<UserService> _logger;
        public UserService(IConfiguration configuration, ILogger<UserService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }
        public async Task<User> LoginAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Попытка входа с пустым логином или паролем!");
                return null!;
            }

            try
            {
                using (var coneection = new SqliteConnection(_connectionString))
                {
                    await coneection.OpenAsync();
                    string commandText = "SELECT * FROM Users WHERE Login=@login AND Password=@password";
                    var command = new SqliteCommand(commandText, coneection);
                    command.Parameters.Add(new SqliteParameter("@login", login));
                    command.Parameters.Add(new SqliteParameter("@password", password));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            User user = new User()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                            };

                            if (reader["Type"].ToString() == "Преподаватель")
                            {
                                user.Type = UserType.Teacher;
                                user.Group = null;
                            }
                            else
                            {
                                user.Type = UserType.Student;
                                user.Group = reader["Group"].ToString();
                            }
                            _logger.LogInformation($"Успешная авторизация пользователя {login}");
                            return user;
                        }
                        _logger.LogError($"Неудачная ошибка аутентификации пользователя {login}");
                        return null!;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при аутентификации пользователя {login}");
                return null!;
            }
        }
    }
}
