using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.ViewModel;

namespace WebStore.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с сотрудниками
    /// </summary>
    public interface IEmployeesData
    {
        /// <summary>
        /// Получение списка сотрудников
        /// </summary>
        /// <returns></returns>
        IEnumerable<EmployeeViewModel> GetAll();

        /// <summary>
        /// Получение сотрудника по id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        EmployeeViewModel GetById(int id);

        /// <summary>
        /// Добавить нового
        /// </summary>
        /// <param name="model"></param>
        int Add(EmployeeViewModel employee);

        /// <summary>
        /// Обновление сотрудника
        /// </summary>
        /// <param name="id">Id сотрудника</param>
        /// <param name="entity">Сотрудник для обновления</param>
        /// <returns></returns>
        EmployeeViewModel Update(EmployeeViewModel employee);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        bool Delete(int id);

        void Commit();
    }
}
