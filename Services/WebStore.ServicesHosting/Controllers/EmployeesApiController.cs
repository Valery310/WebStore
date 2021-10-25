using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var result = _employeesData.Add(employee);
            return result > 0 ? NotFound() : Ok(result);
        }

        [NonAction]
        public void Commit()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _employeesData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }

        [HttpGet, ActionName("Get")]
        public IActionResult GetAll()
        {
            var result = _employeesData.GetAll();
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public IActionResult GetById(int id)
        {
            var result = _employeesData.GetById(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("{id}"), ActionName("Put")]
        public IActionResult Update([FromBody]EmployeeViewModel employee)
        {
            var result = _employeesData.Update(employee);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
