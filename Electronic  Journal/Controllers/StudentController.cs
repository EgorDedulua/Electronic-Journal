using Electronic__Journal.Models;
using Electronic__Journal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Electronic__Journal.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("subjects/{groupId}")]
        public async Task<IActionResult> GetStudentSubjects(int groupId)
        {
            try
            {
                LinkedList<Subject> subjects = await _studentService.GetStudentSubjectsAsync(groupId);
                if (subjects == null)
                {
                    return BadRequest("Не найдены предметы!");
                }
                _logger.LogInformation($"Найдены предметы у студента с группой {groupId}");
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return StatusCode(500, "На сервере произошла ошибка при обработке запроса!");
            }
        }

        [HttpGet("{studentId}/marks")]
        public async Task<IActionResult> GetStudentMarks(int studentId)
        {
            try
            {
                LinkedList<Mark> markList = await _studentService.GetStudentMarksAsync(studentId);
                if (markList == null)
                {
                    return NoContent();
                }
                _logger.LogInformation($"Найдены оценки для студента с id {studentId}");
                return Ok(markList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return StatusCode(500, "На сервере произошла ошибка при обработке запроса!");
            }
        }

        [HttpGet("{studentId}/absenses")]
        public async Task<IActionResult> GetStudentAbsenses(int studentId)
        {
            try
            {
                LinkedList<Absense> absenseList = await _studentService.GetStudentAbsensesAsync(studentId);
                if (absenseList == null)
                {
                    return NoContent();
                }
                _logger.LogInformation($"Найдены оценки для студента с id {studentId}");
                return Ok(absenseList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка на стороне сервера!");
                return StatusCode(500, "На сервере произошла ошибка при обработке запроса!");
            }
        }
    }
}
