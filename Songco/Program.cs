using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Songco
{
    //Encapsulator
    public class DenominationCalculator
    {
        private decimal _salary;
        public int Thousand { get; private set; }
        public int FiveHundred { get; private set; }
        public int Hundred { get; private set; }
        public int Fifty { get; private set; }
        public int Twenty { get; private set; }
        public int Ten { get; private set; }
        public int Five { get; private set; }
        public int One { get; private set; }

        public DenominationCalculator(decimal salary)
        {
            _salary = salary;
            CalculateDenominations();
        }
        private void CalculateDenominations()
        {
            decimal Remaining = _salary;
            Thousand = (int)(Remaining / 1000); Remaining = Remaining % 1000;
            FiveHundred = (int)(Remaining / 500); Remaining = Remaining % 500;
            Hundred = (int)(Remaining / 100); Remaining = Remaining % 100;
            Fifty = (int)(Remaining / 50); Remaining = Remaining % 50;
            Twenty = (int)(Remaining / 20); Remaining = Remaining % 20;
            Ten = (int)(Remaining / 10); Remaining = Remaining % 10;
            Five = (int)(Remaining / 5); Remaining = Remaining % 5;
            One = (int)(Remaining / 1); Remaining = Remaining % 1;
        }
    }

    //Abstract
    public abstract class Employee
    {
        public string Name { get; protected set; }
        public decimal Salary { get; protected set; }

        protected Employee(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }

        public abstract string EmploymentStatus();
    }

    //Inheritance
    public class RegularEmployee : Employee
    {
        public RegularEmployee(string name, decimal salary) : base(name, salary) { }

        public override string EmploymentStatus()
        {
            return "Regular";
        }
    }
    public class PartTimeEmployee : Employee
    {
        public PartTimeEmployee(string name, decimal salary) : base(name, salary) { }
        public override string EmploymentStatus()
        {
            return "Part-Timer";
        }
    }


    internal class Program
    {
        static void PrintRow(string name, decimal salary, DenominationCalculator denomination)
        {
            string displayName = name.Length <= 26 ? name : name.Substring(0, 26);

            Console.WriteLine($"{displayName,-28}|{salary,11} |{denomination.Thousand,5} |{denomination.FiveHundred,5} |{denomination.Hundred,5} |{denomination.Fifty,5} |{denomination.Twenty,5} |{denomination.Ten,5} |{denomination.Five,5} |{denomination.One,5} |");
        }
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();

            string RegularFilePath = "Regular.txt";
            string PartTimerFilePath = "PartTimer.txt";

            foreach (string line in File.ReadAllLines(RegularFilePath))
            {
                string[] parts = line.Split('\t');

                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    string value = parts[1].Trim().Replace(",", "");

                    decimal.TryParse(value , out var salary);

                    employees.Add(new RegularEmployee(name, salary));
                }
            }

            foreach (string line in File.ReadAllLines(PartTimerFilePath))
            {
                string[] parts = line.Split('\t');

                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    string value = parts[1].Trim().Replace(",", "");

                    decimal.TryParse(value, out var salary);

                    employees.Add(new PartTimeEmployee(name, salary));
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine($"{"Name",-28}|{"Salary",11} |{"1000",5} |{"500",5} |{"100",5} |{"50",5} |{"20",5} |{"10",5} |{"5",5} |{"1",5} |");
            Console.WriteLine(new string('=', 98));
            Console.ResetColor();

            decimal TotalSalary = 0;
            int TotalThousand = 0;
            int TotalFiveHundred = 0;
            int TotalHundred = 0;
            int TotalFifty = 0;
            int TotalTwenty = 0;
            int TotalTen = 0;
            int TotalFive = 0;
            int TotalOne = 0;

            int Row = 0;
            foreach (Employee employee in employees)
            {
                if (Row % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }

                DenominationCalculator denomination = new DenominationCalculator(employee.Salary);
                PrintRow(employee.Name, employee.Salary, denomination);

                TotalSalary += employee.Salary;
                TotalThousand += denomination.Thousand;
                TotalFiveHundred += denomination.FiveHundred;
                TotalHundred += denomination.Hundred;
                TotalFifty += denomination.Fifty;
                TotalTwenty += denomination.Twenty;
                TotalTen += denomination.Ten;
                TotalFive += denomination.Five;
                TotalOne += denomination.One;

                Row++;
            }

            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('=', 98));

            Console.WriteLine($"{"TOTAL",-28}|{TotalSalary,11:N2} |{TotalThousand,5} |{TotalFiveHundred,5} |{TotalHundred,5} |{TotalFifty,5} |{TotalTwenty,5} |{TotalTen,5} |{TotalFive,5} |{TotalOne,5} |");
            Console.ReadKey();
        }
    }
}