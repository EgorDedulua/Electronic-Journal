using Microsoft.AspNetCore.Mvc;
using Electronic__Journal.Services;
using Electronic__Journal.Models;

namespace Electronic__Journal.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger)
        {
            _teacherService = teacherService;
            _logger = logger;
        }

        [HttpGet("groups/{teacherId}")]
        public async Task<IActionResult> GetTeacherGroups(int teacherId)
        {
            try
            {
                LinkedList<Electronic__Journal.Models.Group> teacherGroups = await _teacherService.GetTeacherGroupsAsync(teacherId);
                if (teacherGroups == null)
                {
                    _logger.LogError($"Не найдены группы для преподавателя с id {teacherId}");
                    return BadRequest("Не найдены группы!");
                }
                _logger.LogInformation($"Найдены группы для преподавателя с id {teacherId}");
                return Ok(teacherGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return StatusCode(500, "Произошла ошибка на стороне сервера!");
            }
        }
    }
}
