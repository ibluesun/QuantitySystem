using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities;
using QuantitySystem.Units;
using QuantitySystem.Units.SIUnits;
using QuantitySystem.Units.SIUnits.BaseUnits;
using QuantitySystem.Units.UnitSystems;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySimpleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            test1();

            test2();

            test3();

            test4();

            test5();

            test6();


            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new TestingTorqueForm());



        }

        static void test6()
        {

            Console.WriteLine();
            Console.WriteLine("Test 6");


            var pre = SIUnitSystem.Default<Pascal>(100);
            var ts = SIUnitSystem.Default<Second>(12);

            var tpr = pre * ts;
            Console.WriteLine(tpr.ToString());

            var vis = SIUnitSystem.GetUnitizedQuantityOf<Viscosity>(100);

            Console.WriteLine(vis.ToString());


            var a = SIUnitSystem.Default<Radian>(100);
            var t = SIUnitSystem.Milli<Second>(35.5);

            var angularSpeed = a / t;
            Console.WriteLine(a / t);

            Torque tor = (Torque)SIUnitSystem.GetUnitizedQuantityOf<Torque>(100);

            Console.WriteLine(tor.ToString());
            Console.WriteLine(tor * angularSpeed);
        }

        static void test5()
        {
            Console.WriteLine();
            Console.WriteLine("Test 5");

            var t = SIUnitSystem.GetUnitizedQuantityOf<Torque>(100);

            var f = SIUnitSystem.Default<Newton>(5);

            var a = SIUnitSystem.GetUnitizedQuantityOf<Angle>(Math.PI);

            var e = t * a;

            var l = t / f;

            

            var wr = f * l;

            Console.WriteLine(e.ToString());
            Console.WriteLine(l.ToString());


            var a2 = e / t;

            Console.WriteLine(a2.ToString());
        }

        static void test4()
        {
            Console.WriteLine();
            Console.WriteLine("Test 4");

            var l = SIUnitSystem.Default<Metre>(100);
            var s = SIUnitSystem.Default<Second>(5);
            var speed = l / s;
            Console.WriteLine(speed.ToString());

            var acceleration = speed / s;
            Console.WriteLine(acceleration.ToString());

            var f1 = SIUnitSystem.Default<Newton>(10);

            var mass = SIUnitSystem.Mega<Gram>(10);

            var f2 = mass * acceleration;

            Console.WriteLine(f2.ToString());

            var m = SIUnitSystem.Default<Metre>(10);

            var result = (f2 - f1) * m;
            Console.WriteLine(result.ToString());
        }

        static void test3()
        {
            Console.WriteLine();
            Console.WriteLine("Test 3");

            var kPa = SIUnitSystem.Kilo<Pascal>(100);
            var s = SIUnitSystem.Default<Second>(10);

            var result = kPa * s;    //viscosity
            Console.WriteLine(result);
        }

        static void test2()
        {
            Console.WriteLine();
            Console.WriteLine("Test 2");

            var kw = SIUnitSystem.Kilo<Watt>(100);
            var s = SIUnitSystem.Default<Second>(10);
            var a = SIUnitSystem.GetUnitizedQuantityOf<Area>(50);
            var p = SIUnitSystem.Kilo<Pascal>(5.5);

            var result = kw * s / a / p;
            Console.WriteLine(result);
        }

        static void test1()
        {
            Console.WriteLine();
            Console.WriteLine("Test 1");

            var g = SIUnitSystem.Default<Gram>(100);
            var m = SIUnitSystem.Default<Metre>(10);

            var result = g * m;                       //1000 <g.m>
            Console.WriteLine(result.ToString());

            var kg = SIUnitSystem.Kilo<Gram>(100);
            var km = SIUnitSystem.Kilo<Metre>(100);

            result = kg * km;                        //10000 <kg.km>
            Console.WriteLine(result.ToString());    //should be fixed

            var acc = SIUnitSystem.GetUnitizedQuantityOf<Acceleration>(10);

            result = kg * acc;                       //1000 N
            Console.WriteLine(result.ToString());  //error, fixed


            var l = SIUnitSystem.Kilo<Metre>(20);
            var t = SIUnitSystem.Milli<Second>(10);

            var velocity = l / t;                  // kilo/milli == 3 - -3 = 6=> 2 Mm/s
            Console.WriteLine(velocity.ToString()); //velocity.

            var kacc = velocity / t;               // Mm/s /ms== 6 --3=9 => Gm/s^2
            Console.WriteLine(kacc.ToString());  // acceleration.

            result = g * kacc;                        //MN
            Console.WriteLine(result.ToString());

            result = kg * kacc;                       //GN
            Console.WriteLine(result.ToString());

            var Mg = SIUnitSystem.Mega<Gram>(200);
            result = Mg * kacc;                       //TN
            Console.WriteLine(result.ToString());

            var kw = SIUnitSystem.Kilo<Watt>(100);
            result = kw * t;                          //Kilo*Milli 3 + -3 =0 J
            Console.WriteLine(result.ToString());

            result = result / SIUnitSystem.Milli<Newton>(3);  // J/mN == km
            Console.WriteLine(result);
        }
    }
}
