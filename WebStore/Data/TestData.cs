using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<EmployeeView> Employees { get; } = new(3)
            {
                new EmployeeView
                {
                    Id = 1,
                    FirstName = "Вася",
                    SurName = "Пупкин",
                    Patronymic = "Иванович",
                    Age = 22,
                    Position = "Директор"
                },
                new EmployeeView()
    {
        Id = 2,
                    FirstName = "Иван",
                    SurName = "Холявко",
                    Patronymic = "Александрович",
                    Age = 30,
                    Position = "Программист"
                },
                new EmployeeView()
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
