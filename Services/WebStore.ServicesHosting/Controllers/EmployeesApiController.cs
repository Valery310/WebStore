using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _employeesData;
        private readonly ILogger<EmployeesApiController> _logger;

        public EmployeesApiController(IEmployeesData employeesData, ILogger<EmployeesApiController> logger)
        {
            _employeesData = employeesData ?? throw new ArgumentNullException(nameof(employeesData));
            _logger = logger;
        }

        [HttpPost, ActionName("Post")]
        public IActionResult Add([FromBody]EmployeeViewModel employee)
        {
            _logger.LogInformation("Добавление сотрудника пользователем {0}", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = _employeesData.Add(employee);
            return result > 0 ? NotFound() : Ok(result);
        }

        [NonAction]
        public void Commit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удалить сотрудника по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Удаление сотрудника с id = {0} пользователем {1}", id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = _employeesData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }

        /// <summary>
        /// Получение всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("Get")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Получение всех сотрудников пользователем {0}", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = _employeesData.GetAll();
            if (result is null)
            {
                _logger.LogInformation("Сотрудники отсутствуют");
                return NotFound();
            }

            _logger.LogInformation("Сотрудники найдены");
            return Ok(result);
        }

         /// <summary>
         /// Получить сотрудника по идентификатору
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
        [HttpGet("{id}"), ActionName("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            using (_logger.BeginScope("Получение сотрудника по id"))
            {
                _logger.LogInformation("Удаление сотрудника с id = {0} пользователем {1}", id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = _employeesData.GetById(id);
                if (result is null)
                {
                    _logger.LogInformation("Не существует сотрудника с id = {0}", id);
                    return NotFound();
                }
                _logger.LogInformation("Сотрудник найден");

                return Ok(result);
            }
        }

        /// <summary>
        /// Обновить информацию о сотруднике по идентифтикатору
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id}"), ActionName("Put")]
        public IActionResult Update([FromBody]EmployeeViewModel employee)
        {
            using (_logger.BeginScope("Редактирование сотрудника по id"))
            {
                _logger.LogInformation("Удаление сотрудника с id = {0} пользователем {1}", employee.Id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = _employeesData.Update(employee);
                if (result is null)
                {
                    _logger.LogInformation("Не существует сотрудника с id = {0}", employee.Id);
                    return NotFound();
                }
                return Ok(result);
            }               
        }
    }
}
