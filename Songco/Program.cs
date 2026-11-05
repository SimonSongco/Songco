using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Songco
{
    public class Person
    {
        public string Name { get; private set; }
        public decimal Salary { get; private set; }

        public Person(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }
    }

    internal class Program
    {
        static void PrintRow(string name, decimal salary, decimal thousand,
                             decimal fivehundred, decimal hundred, decimal fifty,
                             decimal twenty, decimal ten, decimal five, decimal one)
        {
            Console.WriteLine($"{name,-15}|{salary,6}  |{thousand,4}  |{fivehundred,4}  |{hundred,4}" +
                              $"  |{fifty,4}  |{twenty,4}  |{ten,4}   |{five,4}   |{one,4}   |");
        }

        static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= maxLength ? text : text.Substring(0, maxLength);
        }

        static void Main(string[] args)
        {
            List<Person> People = new List<Person>();
            string FilePath = "DenominationCopyPaste.txt";

            foreach (string Line in File.ReadAllLines(FilePath))
            {
                string[] parts = Line.Split('\t');

                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    decimal.TryParse(parts[1], out decimal salary);

                    People.Add(new Person(name, salary));
                }
            }

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"{"Name",-15}|{" Salary",6} |{" 1000",4} |{"  500",4} |{"  100",4} |{"50",4}  |{"20",4}  |{"10",4}   |{"5",4}   |{"1",4}   |");
            Console.WriteLine(new string('=', 84));
            Console.ResetColor();

            int rowCount = 0;
            foreach (Person p in People)
            {
                if (rowCount % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor= ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }

                decimal Remaining = p.Salary;
                int Thousand = (int)(Remaining / 1000); Remaining = Remaining % 1000;
                int FiveHundred = (int)(Remaining / 500); Remaining = Remaining % 500;
                int Hundred = (int)(Remaining / 100); Remaining = Remaining % 100;
                int Fifty = (int)(Remaining / 50); Remaining = Remaining % 50;
                int Twenty = (int)(Remaining / 20); Remaining = Remaining % 20;
                int Ten = (int)(Remaining / 10); Remaining = Remaining % 10;
                int Five = (int)(Remaining / 5); Remaining = Remaining % 5;
                int One = (int)(Remaining / 1); Remaining = Remaining % 1;

                string displayName = Truncate(p.Name, 15);
                

                PrintRow(displayName, p.Salary, Thousand, FiveHundred, Hundred, Fifty, Twenty, Ten, Five, One);
                rowCount++;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('=', 84));

            Console.ResetColor();
            Console.ReadKey();
        }
    }
}