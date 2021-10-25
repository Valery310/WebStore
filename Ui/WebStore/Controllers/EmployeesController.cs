using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModel;
using Microsoft.Extensions.Logging;

namespace WebStore.Controllers
{
    [Route("users")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _employees;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
        {
            _employees = employeesData;
            _logger = logger;
        }

        public IActionResult Index() => View(_employees.GetAll());

        [Route("id")]
        public IActionResult Details(int id)
        {
            //Получаем сотрудника по Id
            _logger.LogInformation("Получаем сотрудника по Id = {0}", id);
            var employee = _employees.GetById(id);

            //Если такого не существует
            if (ReferenceEquals(employee, null))
            {
                _logger.LogWarning("сотрудника по Id = {0} не существует", id);
                return NotFound();//возвращаем результат 404 Not Found
            }
            //Иначе возвращаем сотрудника
            _logger.LogInformation("Cотрудник с Id = {0} найден", id);
            return View(employee);
        }

        [Route("edit/{id?}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int? id)
        {
            EmployeeViewModel model;
            if (id.HasValue)
            {
                model = _employees.GetById(id.Value);
                if (ReferenceEquals(model,null))
                {
                    return NotFound();
                }
            }
            else
            {
                model = new EmployeeViewModel();
            }
            return View(model);
        }

        [HttpPut]
        [HttpPost]
        [Route("edit/{id?}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Age< 18 && model.Age > 75)
            {
                _logger.LogWarning("Возраст указан за пределами допустимого диапазона");
                ModelState.AddModelError("Age", "Возраст указан за пределами допустимого диапазона");
            }
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    _employees.Update(model);
                }
                else
                {
                    _employees.Add(model);
                }

                return RedirectToAction(nameof(Index));
            }
            //Возвращаем модель, если не валидна.
            return View(model);
        }

        [Route("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            _employees.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
