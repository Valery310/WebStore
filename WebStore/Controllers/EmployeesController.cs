using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Data;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    [Route("users")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _employees;

        public EmployeesController(IEmployeesData employeesData)
        {
            _employees = employeesData;
        }

        public IActionResult Index() => View(_employees.GetAll());

        [Route("id")]
        public IActionResult Details(int id)
        {
            //Получаем сотрудника по Id
            var employee = _employees.GetById(id);

            //Если такого не существует
            if (ReferenceEquals(employee, null))
                return NotFound();//возвращаем результат 404 Not Found

            //Иначе возвращаем сотрудника
            return View(employee);
        }

        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            EmployeeView model;
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
                model = new EmployeeView();
            }
            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(EmployeeView model)
        {
            if (model.Age< 18 && model.Age > 75)
            {
                ModelState.AddModelError("Age", "Возраст указан за пределами допустимого диапазона");
            }
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var dbitem = _employees.GetById(model.Id);

                    if (ReferenceEquals(dbitem, null))
                    {
                        return NotFound();
                    }

                    dbitem.FirstName = model.FirstName;
                    dbitem.SurName = model.SurName;
                    dbitem.Age = model.Age;
                    dbitem.Patronymic = model.Patronymic;
                    dbitem.Position = model.Position;
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
        public ActionResult Delete(int id)
        {
            _employees.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
