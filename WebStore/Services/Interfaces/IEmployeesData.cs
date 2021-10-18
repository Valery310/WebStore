using System.Collections.Generic;
using WebStore.ViewModel;

namespace WebStore.Services.Interfaces
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
