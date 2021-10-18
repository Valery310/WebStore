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
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (TestData.Employees.Contains(employee))
            {
               // throw new ArgumentException($"Такая запись сотрудника уже существует");
                return employee.Id;
            }

            employee.Id = _employees.Max(i => i.Id) + 1;
            TestData.Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee!=null)
            {
                _employees.Remove(employee);
                return true;
            }
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
                throw new ArgumentNullException(nameof(employee));
            }

            var temp = TestData.Employees.SingleOrDefault(t => t.Id == employee.Id);
            if (temp is null)
            {
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
