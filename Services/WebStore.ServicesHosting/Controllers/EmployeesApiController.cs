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
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;
        private readonly ILogger<EmployeesApiController> _logger;

        public EmployeesApiController(IEmployeesData employeesData, ILogger<EmployeesApiController> logger)
        {
            _employeesData = employeesData ?? throw new ArgumentNullException(nameof(employeesData));
            _logger = logger;
        }

        [HttpPost, ActionName("Post")]
        public int Add([FromBody]EmployeeViewModel employee)
        {
           return _employeesData.Add(employee);
        }

        [NonAction]
        public void Commit()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _employeesData.Delete(id);
        }

        [HttpGet, ActionName("Get")]
        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _employeesData.GetAll();
        }

        [HttpGet("{id}"), ActionName("Get")]
        public EmployeeViewModel GetById(int id)
        {
            return _employeesData.GetById(id);
        }

        [HttpPut("{id}"), ActionName("Put")]
        public EmployeeViewModel Update([FromBody]EmployeeViewModel employee)
        {
            return _employeesData.Update(employee);
        }
    }
}
