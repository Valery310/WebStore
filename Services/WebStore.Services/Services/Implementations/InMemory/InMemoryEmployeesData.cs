using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModel;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
      //  private static int _CurrentMaxId = TestData.Employees.Count;
        private readonly ILogger<InMemoryEmployeesData> logger;
        private readonly List<EmployeeViewModel> _employees;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _employees = TestData.Employees;
            logger = Logger;
        }

        public int Add(EmployeeViewModel employee)
        {
            logger.LogInformation("Добавление сотрудника");

            if (employee is null)
            {
                logger.LogError("Данные пусты");
                throw new ArgumentNullException(nameof(employee));
            }

            if (TestData.Employees.Contains(employee))
            {
                logger.LogError("Такая запись сотрудника уже существует");
                // throw new ArgumentException($"Такая запись сотрудника уже существует");
                return employee.Id;
            }

            employee.Id = _employees.Max(i => i.Id) + 1;
            TestData.Employees.Add(employee);
            logger.LogInformation("Сотрудник усешно добавлен");

            return employee.Id;
        }

        public bool Delete(int id)
        {
            logger.LogInformation("Удаление сотрудника с id = {0}", id);

            var employee = GetById(id);
            if (employee!=null)
            {              
                _employees.Remove(employee);
                logger.LogInformation("Сотрудник успешно удален");
                return true;
            }
            logger.LogError("Сотрудник не найден");
            return false;
        }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _employees;
        }

        public EmployeeViewModel GetById(int id)
        {
            //Получаем сотрудника по Id
            var employee = _employees.SingleOrDefault(t => t.Id.Equals(id));

            //Если такого не существует
            if (ReferenceEquals(employee, null))
                throw new NullReferenceException();//возвращаем результат 404 Not Found

            //Иначе возвращаем сотрудника
            return employee;
        }

        public EmployeeViewModel Update(EmployeeViewModel employee)
        {
            if (employee is null)
            {
                logger.LogError("Данные пусты");
                throw new ArgumentNullException(nameof(employee));
            }

            var temp = TestData.Employees.SingleOrDefault(t => t.Id == employee.Id);
            if (temp is null)
            {
                logger.LogError("Сотрудник не существует");
                throw new InvalidOperationException("Сотрудник не существует");
            }
            // temp = employee;
            TestData.Employees.ElementAt(employee.Id-1).Age = employee.Age;
            TestData.Employees.ElementAt(employee.Id-1).FirstName = employee.FirstName;
            TestData.Employees.ElementAt(employee.Id-1).SurName = employee.SurName;
            TestData.Employees.ElementAt(employee.Id-1).Patronymic = employee.Patronymic;
            TestData.Employees.ElementAt(employee.Id-1).Position = employee.Position;

            return temp;
        }

        public void Commit() 
        {
        
        }
    }

}
