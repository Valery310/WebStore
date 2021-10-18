using System;

namespace WebStore.TestConsole
{
    class Program
    {
        // private record Student(string LastName, string FirstName, int Age);

        private record Student() 
        {
            public string LastName { get; init; }
            public string FirstName { get; init; }
            public int Age { get; init; }
        }

        static void Main(string[] args)
        {
            var v1 = new Student
            {
                LastName = "Last1",
                FirstName = "First1",
                Age = 10
            };

            var v2 = new Student
            {
                LastName = "Last2",
                FirstName = "First2",
                Age = 11
            };

            if (v1 == v2)
            {
                Console.WriteLine("equals");
            }
        }
    }
}
