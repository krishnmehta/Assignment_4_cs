using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Assignment4 // Note: actual namespace depends on the project name.
{
    public class Program
    {
        IList employeeList;
        IList salaryList;

        public Program()
        {
            employeeList = new List<Employee> {
            new Employee(){ EmployeeID = 1, EmployeeFirstName = "Rajiv", EmployeeLastName = "Desai", Age = 49},
            new Employee(){ EmployeeID = 2, EmployeeFirstName = "Karan", EmployeeLastName = "Patel", Age = 32},
            new Employee(){ EmployeeID = 3, EmployeeFirstName = "Sujit", EmployeeLastName = "Dixit", Age = 28},
            new Employee(){ EmployeeID = 4, EmployeeFirstName = "Mahendra", EmployeeLastName = "Suri", Age = 26},
            new Employee(){ EmployeeID = 5, EmployeeFirstName = "Divya", EmployeeLastName = "Das", Age = 20},
            new Employee(){ EmployeeID = 6, EmployeeFirstName = "Ridhi", EmployeeLastName = "Shah", Age = 60},
            new Employee(){ EmployeeID = 7, EmployeeFirstName = "Dimple", EmployeeLastName = "Bhatt", Age = 53}
        };

            salaryList = new List<Salary> {
            new Salary(){ EmployeeID = 1, Amount = 1000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 1, Amount = 500, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 1, Amount = 100, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 2, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 2, Amount = 1000, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 3, Amount = 1500, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 4, Amount = 2100, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 2800, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 600, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 5, Amount = 500, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 6, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 6, Amount = 400, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 7, Amount = 4700, Type = SalaryType.Monthly}
        };
        }

        public static void Main(string[] args)
        {
            Program program = new Program();

            program.Task1();

            program.Task2();

            program.Task3();
        }

        public void Task1()
        {
            //Implementation
            var result = from emp in employeeList.Cast<Employee>()
                         join sal in salaryList.Cast<Salary>() on emp.EmployeeID equals sal.EmployeeID
                         group sal by new { emp.EmployeeFirstName, emp.EmployeeLastName } into g

                         select new
                         {
                             Name = g.Key.EmployeeFirstName + " " + g.Key.EmployeeLastName,
                             TotalSalary = g.Sum(s => s.Amount)
                         }
                         into tempResult
                         orderby tempResult.TotalSalary ascending
                         select tempResult;

            Console.WriteLine("*-----Task-1-----*");
            Console.WriteLine("Total salary of all employees in ascending order is:\n");
            
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name}: {item.TotalSalary}");
                
            }
        }

        public void Task2()
        {
            //Implementation
            var result = from emp in employeeList.Cast<Employee>()
                         where emp.Age > 0
                         orderby emp.Age descending
                         select emp;

            var SecondOldestemp = result.Skip(1).Take(1);
            var monthlySalary = from emp in SecondOldestemp
                                join sal in salaryList.Cast<Salary>() on emp.EmployeeID equals sal.EmployeeID
                                where sal.Type == SalaryType.Monthly
                                group sal by new { emp.EmployeeFirstName, emp.EmployeeLastName, emp.Age, emp.EmployeeID} into g
                                select new
                                {
                                    Id = g.Key.EmployeeID,
                                    Name = g.Key.EmployeeFirstName + " " + g.Key.EmployeeLastName,
                                    age = g.Key.Age,
                                    TMonthlySalary = g.Sum(s => s.Amount),
                                    
                                }
                                into tempResult
                                select tempResult;

            Console.WriteLine("\n*-----Task-2-----*");
            Console.WriteLine("Employee details of 2nd oldest employee including his/her total monthly salary.\n");
            foreach(var item in monthlySalary)
            {
                Console.WriteLine($"Id: {item.Id}, Name: {item.Name} \nAge: {item.age}, Monthly Salary: {item.TMonthlySalary}");
                
            }
            
        }

        public void Task3()
        {
            //Implementation
            var result = from emp in employeeList.Cast<Employee>()
                         join sal in salaryList.Cast<Salary>()
                         on emp.EmployeeID equals sal.EmployeeID
                         where emp.Age > 30
                         group sal by new { emp.EmployeeID, emp.EmployeeFirstName, emp.EmployeeLastName } into g
                         select new
                         {
                             Id = g.Key.EmployeeID,
                             Name = g.Key.EmployeeFirstName + " " + g.Key.EmployeeLastName,
                             AvgSal = g.Select(s => s.Amount).Average()
                         };

            Console.WriteLine("\n*-----Task-3-----*");
            Console.WriteLine("Print means of Monthly, Performance, Bonus salary of employees whose age is greater than 30.");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Id} {item.Name}: {item.AvgSal:N2}");
            }

        }
    }

    public enum SalaryType
    {
        Monthly,
        Performance,
        Bonus
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public int Age { get; set; }
    }

    public class Salary
    {
        public int EmployeeID { get; set; }
        public int Amount { get; set; }
        public SalaryType Type { get; set; }
    }
}