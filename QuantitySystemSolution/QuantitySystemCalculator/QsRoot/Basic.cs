using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Types;

namespace QsRoot
{
    /// <summary>
    /// The whole class is for testing new native binding of Qs
    /// However the Basic name which will be Qs Namespace is very nostalgic
    /// I may even put MSX basic commands in it 
    /// I mean POKE, and PEEK   commands
    /// VPOKE, VPEEK  commands
    /// Yes
    /// I may end with MSX virtual machine :)  WHO Knows
    /// </summary>
    public static class Basic
    {
        public static void Print(string text)
        {
            Console.WriteLine(text);
        }

        public static string Input()
        {
            return Console.ReadLine();
        }

        public static string Input(string alert)
        {
            Console.Write(alert);
            return Console.ReadLine();

        }

        /// <summary>
        /// Testing binding to from vector to array of integers.
        /// </summary>
        /// <param name="a"></param>
        public static void PrintVector(int[] a)
        {
            foreach (var o in a)
                Console.WriteLine(o);
        }

    }

}