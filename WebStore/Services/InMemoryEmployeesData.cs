using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Services.Interfaces;
using WebStore.ViewModel;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private static int _CurrentMaxId = TestData.Employees.Count;
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
                return employee.Id;
            }
            
            employee.Id = ++_CurrentMaxId;
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

        IEnumerable<EmployeeViewModel> IEmployeesData.GetAll()
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

        public void Update(EmployeeViewModel employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var temp = TestData.Employees.SingleOrDefault(t => t.Id == employee.Id);
            temp = employee;
        }
    }

}
