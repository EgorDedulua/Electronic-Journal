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

        [HttpGet("{teacherId}/{groupId}/subjects")]
        public async Task<IActionResult> GetTeacherGroupsSubjects(int teacherId, int groupId)
        {
            try
            {
                LinkedList<Subject> groupSubjectsList = await _teacherService.GetTeacherGroupsSubjectsAsync(teacherId, groupId);
                if (groupSubjectsList == null)
                {
                    return BadRequest($"Не найдены предметы, которые ведет преподаватель с id {teacherId} у группы с id {groupId}");
                }
                _logger.LogInformation($"Найдены предметы, которые ведет преподаватель с id {teacherId} у группы с id {groupId}");
                return Ok(groupSubjectsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизветсная ошибка на стороне сервера!");
                return StatusCode(500, "Произошла неизвестная ошибка на стороне сервера!");
            }
        }
    }
}
