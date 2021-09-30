using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Data;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEnumerable<EmployeeView> _employees;

        public EmployeesController()
        {
            _employees = TestData.Employees;
        }

        public IActionResult Index() => View(_employees);

        public IActionResult Details(int id)
        {
            //Получаем сотрудника по Id
            var employee = _employees.SingleOrDefault(t => t.Id.Equals(id));

            //Если такого не существует
            if (ReferenceEquals(employee, null))
                return NotFound();//возвращаем результат 404 Not Found

            //Иначе возвращаем сотрудника
            return View(employee);
        }
    }
}
