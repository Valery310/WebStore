using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{/// <summary>
/// Информация о сотруднике
/// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Имя сотрудника
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество сотрудника
        /// </summary>
        [MaxLength(100)]
        public string Patronymic { get; set; }

        /// <summary>
        /// Возраст сотрудника
        /// </summary>
        public int Age { get; set; }
    }
}
