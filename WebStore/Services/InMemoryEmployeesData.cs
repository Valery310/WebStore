using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private static int _CurrentMaxId;
        private readonly ILogger<InMemoryEmployeesData> logger;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            logger = Logger;
        }

        int IEmployeesData.Add(EmployeeView employee)
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

        bool IEmployeesData.Delete(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<EmployeeView> IEmployeesData.GetAll()
        {
            throw new NotImplementedException();
        }

        EmployeeView IEmployeesData.GetById(int id)
        {
            throw new NotImplementedException();
        }

        void IEmployeesData.Update(EmployeeView employee)
        {
            throw new NotImplementedException();
        }
    }
}
