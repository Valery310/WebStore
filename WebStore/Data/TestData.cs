using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<EmployeeView> Employees { get; } = new ()
            {
                new EmployeeView(
                    1,
                    "Иван",
                    "Иванов",
                    "Иванович",
                    22)

                ,
                new EmployeeView(
                    2,
                    "Владислав",
                    "Петров",
                    "Иванович",
                    35)
            };

    }
}
