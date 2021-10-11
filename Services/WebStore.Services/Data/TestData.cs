using System.Collections.Generic;
using WebStore.Domain.ViewModel;

namespace WebStore.Services.Data
{
    public static class TestData
    {
        public static List<EmployeeViewModel> Employees { get; } = new(3)
            {
                new EmployeeViewModel
                {
                    Id = 1,
                    FirstName = "Вася",
                    SurName = "Пупкин",
                    Patronymic = "Иванович",
                    Age = 22,
                    Position = "Директор"
                },
                new EmployeeViewModel
    {
        Id = 2,
                    FirstName = "Иван",
                    SurName = "Холявко",
                    Patronymic = "Александрович",
                    Age = 30,
                    Position = "Программист"
                },
                new EmployeeViewModel
    {
        Id = 3,
                    FirstName = "Роберт",
                    SurName = "Серов",
                    Patronymic = "Сигизмундович",
                    Age = 50,
                    Position = "Зав. склада"
                }
};
        //public static List<EmployeeView> Employees { get; } = new ()
        //    {
        //        new EmployeeView(
        //            1,
        //            "Иван",
        //            "Иванов",
        //            "Иванович",
        //            22)

        //        ,
        //        new EmployeeView(
        //            2,
        //            "Владислав",
        //            "Петров",
        //            "Иванович",
        //            35)
        //    };

    }
}
