using System.Collections.Generic;
using WebStore.Domain.ViewModel;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<EmployeeViewModel> GetAll();

        EmployeeViewModel GetById(int id);

        int Add(EmployeeViewModel employee);

        void Update(EmployeeViewModel employee);

        bool Delete(int id);
    }
}
