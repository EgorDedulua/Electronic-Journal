using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Text.Json;
using Electronic__Journal.Models;
using Electronic__Journal.Services;

namespace Electronic__Journal.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement request)
        {
            try
            {
                if (request.ValueKind == JsonValueKind.Null)
                {
                    _logger.LogError("Тело запроса для авторизации пользователя было пустым");
                    return BadRequest("тело запроса не может быть пустым");
                }

                request.TryGetProperty("Login", out JsonElement login);
                request.TryGetProperty("Password", out JsonElement password);
                if (login.ValueKind == JsonValueKind.Undefined || password.ValueKind == JsonValueKind.Undefined)
                {
                    _logger.LogError("Тело запроса не содержало логина или пароля");
                    return BadRequest("Тело запроса не содержало логина или пароля!");
                }

                User user = await _userService.LoginAsync(login.GetString()!, password.GetString()!);

                if (user == null)
                {
                    _logger.LogInformation("Введен неверный логин или пароль");
                    return Unauthorized("Неверный логин или пароль!");
                }
                else
                {
                    _logger.LogInformation($"Пользователь {login.GetString()} Id: {user.Id} Фамилия: {user.LastName} Имя: {user.FirstName} Отчество: {user.MiddleName} Группа: {user.Group} успешно вошел");
                    return Ok(user);
                }
            }
            catch (InvalidOperationException)
            {
                _logger.LogError("Некорректный формат Json!");
                return BadRequest("Некорректный формат Json!");
            }
            catch (Exception ex)
            {
                request.TryGetProperty("Login", out JsonElement login);
                _logger.LogError(ex, $"Ошибка на стороне сервера при обработке запроса авторизации пользователя {login.GetString()}");
                return StatusCode(500, "Ошибка сервера!");
            }
        }
    }
}
