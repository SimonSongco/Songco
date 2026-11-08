using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Songco
{
    //Encapsulator
    public class DenominationCalculator
    {
        private decimal _salary;
        private bool _includeCents;

        public decimal Salary { get; private set; }
        public int Thousand { get; private set; }
        public int FiveHundred { get; private set; }
        public int Hundred { get; private set; }
        public int Fifty { get; private set; }
        public int Twenty { get; private set; }
        public int Ten { get; private set; }
        public int Five { get; private set; }
        public int One { get; private set; }

        public int TwentyFiveCents { get; private set; }
        public int TenCents { get; private set; }
        public int FiveCents { get; private set; }
        public int OneCents { get; private set; }

        public DenominationCalculator(decimal salary, bool includeCents)
        {
            _salary = salary;
            _includeCents = includeCents;
            CalculateDenominations();
        }
        private void CalculateDenominations()
        {
            decimal Remaining = _salary;

            if (!_includeCents)
            {
                Remaining = (int)Remaining;
            }

            Salary = Remaining;

            Thousand = (int)(Remaining / 1000); Remaining = Remaining % 1000;
            FiveHundred = (int)(Remaining / 500); Remaining = Remaining % 500;
            Hundred = (int)(Remaining / 100); Remaining = Remaining % 100;
            Fifty = (int)(Remaining / 50); Remaining = Remaining % 50;
            Twenty = (int)(Remaining / 20); Remaining = Remaining % 20;
            Ten = (int)(Remaining / 10); Remaining = Remaining % 10;
            Five = (int)(Remaining / 5); Remaining = Remaining % 5;
            One = (int)(Remaining / 1); Remaining = Remaining % 1;

            if (_includeCents)
            {
                TwentyFiveCents = (int)(Remaining / 0.25m); Remaining = Remaining % 0.25m;
                TenCents = (int)(Remaining / 0.10m); Remaining = Remaining % 0.10m;
                FiveCents = (int)(Remaining / 0.05m); Remaining = Remaining % 0.05m;
                OneCents = (int)(Remaining / 0.01m); Remaining = Remaining % 0.01m;
            }
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
    //Polymorphism
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
        static void PrintRow(int rowNumber, string name, decimal salary, DenominationCalculator denomination, bool includeCents)
        {
            string displayName = name.Length <= 29 ? name : name.Substring(0, 29);

            if (includeCents)
            {
                Console.WriteLine($"{rowNumber,4} |{displayName,-31}|{salary,14:N2} |{denomination.Thousand,8} |{denomination.FiveHundred,8} |{denomination.Hundred,8} |{denomination.Fifty,8} |{denomination.Twenty,8} |{denomination.Ten,8} |{denomination.Five,8} |{denomination.One,8} |{denomination.TwentyFiveCents,7} |{denomination.TenCents,7} |{denomination.FiveCents,7} |{denomination.OneCents,7} |");
            }
            else
            {
                Console.WriteLine($"{rowNumber,4} |{displayName,-31}|{denomination.Salary,14:N0} |{denomination.Thousand,8} |{denomination.FiveHundred,8} |{denomination.Hundred,8} |{denomination.Fifty,8} |{denomination.Twenty,8} |{denomination.Ten,8} |{denomination.Five,8} |{denomination.One,8} |");
            }
        }
        static void Main(string[] args)
        {
            List<Employee> RegularEmployees = new List<Employee>();
            List<Employee> PartTimeEmployees = new List<Employee>();

            string FilePath = "Denomination.txt";

            //Categorized Regular and Part-Time
            if (File.Exists(FilePath))
            {
                foreach (string line in File.ReadAllLines(FilePath))
                {
                    string[] parts = line.Split('\t');

                    if (parts.Length >= 2)
                    {
                        string name = parts[0].Trim();
                        string value = parts[1].Trim();
                        string position = parts[2].Trim();

                        decimal.TryParse(value, out var salary);

                        if (parts[2].ToUpper() == "REGULAR")
                        {
                            RegularEmployees.Add(new RegularEmployee(name, salary));
                        }
                        else if (parts[2].ToUpper() == "PART-TIMER")
                        {
                            PartTimeEmployees.Add(new PartTimeEmployee(name, salary));
                        }
                    }
                }
            }

            bool Regulars = RegularEmployees.Count > 0;
            bool PartTimers = PartTimeEmployees.Count > 0;

            if (!Regulars && !PartTimers)
            {
                Console.WriteLine("Empty Files Found!");
                Console.ReadKey();
                return;
            }

            // Outer Loop To Allow Going Back To The Decision Menu
            while (true)
            {
                Console.Clear();
                List<Employee> CurrentSelected = new List<Employee>();
                string Status = "";

                if (Regulars && !PartTimers)
                {
                    CurrentSelected = RegularEmployees;
                    Status = CurrentSelected[0].EmploymentStatus();
                }
                else if (!Regulars && PartTimers)
                {
                    CurrentSelected = PartTimeEmployees;
                    Status = CurrentSelected[0].EmploymentStatus();
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("Both Regular and Part-Timer records detected.");
                        Console.WriteLine("[1] Regular Employees");
                        Console.WriteLine("[2] Part-Time Employees");
                        Console.WriteLine("[3] Show Saved Results");
                        Console.Write("Choose an option (1-3): ");
                        ConsoleKeyInfo choice = Console.ReadKey(true);

                        if (choice.Key == ConsoleKey.D1)
                        {
                            CurrentSelected = RegularEmployees;
                            Status = CurrentSelected[0].EmploymentStatus();
                            break;
                        }
                        else if (choice.Key == ConsoleKey.D2)
                        {
                            CurrentSelected = PartTimeEmployees;
                            Status = CurrentSelected[0].EmploymentStatus();
                            break;
                        }
                        else if (choice.Key == ConsoleKey.D3)
                        {
                            string historyFilePath = "History.txt";

                            if (File.Exists(historyFilePath) && new FileInfo(historyFilePath).Length > 0)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(new string('=', 171));
                                Console.WriteLine(new string(' ', 77) + "SAVED HISTORY LOG");
                                Console.WriteLine(new string('=', 171));
                                Console.ResetColor();

                                string historyContent = File.ReadAllText(historyFilePath);
                                Console.WriteLine(historyContent);

                                Console.WriteLine("\n----------------------------------------------------------------");
                                Console.Write("Press [C] to clear all history, or any other key to go back: ");
                                ConsoleKeyInfo keyChoice = Console.ReadKey(true);

                                if (keyChoice.Key == ConsoleKey.C)
                                {
                                    File.WriteAllText(historyFilePath, string.Empty);
                                    Console.WriteLine("\n\nHistory cleared successfully!");
                                    Thread.Sleep(1500);
                                    Console.Clear();
                                }
                                Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("History file is empty or hasn't been created yet!");
                                Console.WriteLine("\nPress any key to go back...");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input! Only 1, 2, or 3, Try Again!");
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }
                }

                Console.Clear();
                Console.Write("Include Cent Denominations? (.25, .10, .05, .01) \n(Press Y): ");
                bool CentsIncluded = Console.ReadLine().Trim().ToUpper() == "Y";

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGreen;

                //Header
                if (CentsIncluded)
                {
                    Console.WriteLine($"{"#",4} |{"Name",-31}|{"Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |{".25",7} |{".10",7} |{".05",7} |{".01",7} |");
                    Console.WriteLine(new string('=', 171));
                }
                else
                {
                    Console.WriteLine($"{"#",4} |{"Name",-31}|{"Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |");
                    Console.WriteLine(new string('=', 135));
                }
                Console.ResetColor();

                decimal TotalSalary = 0;
                int Total1000 = 0; int Total500 = 0; int Total100 = 0; int Total50 = 0;
                int Total20 = 0; int Total10 = 0; int Total5 = 0; int Total1 = 0;
                int Total25C = 0; int Total10C = 0; int Total5C = 0; int Total1C = 0;

                int Row = 0;
                //Calculating Denomination
                foreach (Employee employee in CurrentSelected)
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

                    DenominationCalculator denomination = new DenominationCalculator(employee.Salary, CentsIncluded);
                    PrintRow(Row + 1, employee.Name, employee.Salary, denomination, CentsIncluded);

                    TotalSalary += denomination.Salary;
                    Total1000 += denomination.Thousand;
                    Total500 += denomination.FiveHundred;
                    Total100 += denomination.Hundred;
                    Total50 += denomination.Fifty;
                    Total20 += denomination.Twenty;
                    Total10 += denomination.Ten;
                    Total5 += denomination.Five;
                    Total1 += denomination.One;
                    Total25C += denomination.TwentyFiveCents;
                    Total10C += denomination.TenCents;
                    Total5C += denomination.FiveCents;
                    Total1C += denomination.OneCents;

                    Row++;
                }

                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;

                //Label
                if (CentsIncluded)
                {
                    Console.WriteLine(new string('=', 171));
                    Console.WriteLine($"{"#",4} |{Status,-31}|{"Total Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |{".25",7} |{".10",7} |{".05",7} |{".01",7} |");
                }
                else
                {
                    Console.WriteLine(new string('=', 135));
                    Console.WriteLine($"{"#",4} |{Status,-31}|{"Total Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |");
                }

                //Denomination
                if (CentsIncluded)
                {
                    Console.WriteLine(new string('=', 171));
                    Console.WriteLine($"{"",4} |{"TOTAL",-31}|{TotalSalary,14:N2} |{Total1000,8} |{Total500,8} |{Total100,8} |{Total50,8} |{Total20,8} |{Total10,8} |{Total5,8} |{Total1,8} |{Total25C,7} |{Total10C,7} |{Total5C,7} |{Total1C,7} |");
                    Console.WriteLine(new string('=', 171));
                }
                else
                {
                    Console.WriteLine(new string('=', 135));
                    Console.WriteLine($"{"",4} |{"TOTAL",-31}|{TotalSalary,14:N0} |{Total1000,8} |{Total500,8} |{Total100,8} |{Total50,8} |{Total20,8} |{Total10,8} |{Total5,8} |{Total1,8} |");
                    Console.WriteLine(new string('=', 135));
                }
                Console.ResetColor();

                Console.WriteLine("\nPress Any Key to go back to the selection menu, or Type \"SAVE\" to save the result summary.");
                string Enter = Console.ReadLine();

                if (Enter.ToUpper().Trim() == "SAVE")
                {
                    string FilePathHistory = "History.txt";
                    List<string> Report = new List<string>();

                    if (CentsIncluded)
                    {
                        Report.Add(new string('=', 171));
                        Report.Add($"{"#",4} |{Status,-31}|{"Total Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |{".25",7} |{".10",7} |{".05",7} |{".01",7} |");
                        Report.Add(new string('=', 171));
                        Report.Add($"{"",4} |{"TOTAL",-31}|{TotalSalary,14:N2} |{Total1000,8} |{Total500,8} |{Total100,8} |{Total50,8} |{Total20,8} |{Total10,8} |{Total5,8} |{Total1,8} |{Total25C,7} |{Total10C,7} |{Total5C,7} |{Total1C,7} |");
                        Report.Add(new string('=', 171));
                    }
                    else
                    {
                        Report.Add(new string('=', 135));
                        Report.Add($"{"#",4} |{Status,-31}|{"Total Salary",14} |{"1000",8} |{"500",8} |{"100",8} |{"50",8} |{"20",8} |{"10",8} |{"5",8} |{"1",8} |");
                        Report.Add(new string('=', 135));
                        Report.Add($"{"",4} |{"TOTAL",-31}|{TotalSalary,14:N0} |{Total1000,8} |{Total500,8} |{Total100,8} |{Total50,8} |{Total20,8} |{Total10,8} |{Total5,8} |{Total1,8} |");
                        Report.Add(new string('=', 135));
                    }

                    File.AppendAllLines(FilePathHistory, Report);
                    Console.Clear();
                    Console.WriteLine("Summary Saved Successfully!");
                    Thread.Sleep(1500);
                }
                else
                {
                    continue;
                }
            }
        }
    }
}