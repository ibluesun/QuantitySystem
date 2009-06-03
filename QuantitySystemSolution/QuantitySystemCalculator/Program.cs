using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystemCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = string.Empty;
            QsEvaluator qse = new QsEvaluator();

            
            StartConsole();


            while (line.ToLower()!= "quit" && line.ToLower()!="exit")
            {
                
                if (!string.IsNullOrEmpty(line))
                {
                    qse.Evaluate(line);
                }

                Console.WriteLine();
                Console.Write("Qs> ");

                line = Console.ReadLine();

            }
        }


        static void StartConsole()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.WriteLine("Quantity System Calculator ver 1.0");
            Console.WriteLine("Copyright 2009 By Ahmed Sadek");
            Console.WriteLine("http://QuantitySystem.CodePlex.com");
            Console.WriteLine();
            Console.WriteLine();
        }



    }
}
