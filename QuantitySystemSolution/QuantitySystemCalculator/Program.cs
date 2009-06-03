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

            PrintDescription();


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


        static void PrintDescription()
        {
            Console.WriteLine("Quantity System Calculator ver 0.1");
            Console.WriteLine("Copyright Ahmed Sadek");
            Console.WriteLine("----------------------");
            Console.WriteLine("http://QuantitySystem.CodePlex.com");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

        }



    }
}
