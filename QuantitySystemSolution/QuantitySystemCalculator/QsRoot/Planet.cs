using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Types;
using QuantitySystem.Units;
using Qs;

namespace QsRoot
{

    public struct PlanetElement
    {
        public string ElementName { get; set; }
        public int ElementNumber { get; set; }
        public bool Rare { get; set; }
    }

    public class Planet
    {

        public Mass<double> Mass { get; set; }

        PlanetElement _pe = new PlanetElement() { ElementName = "7agar", ElementNumber = 34 };

        public PlanetElement Element
        {
            get
            {
                return _pe;
            }
            set
            {
                _pe = value;
            }

        }


        public bool Rare { get; set; }

        

        public Planet()
        {
            planets++;
            Name = "Planet " + planets.ToString();    
        }

        public Planet(PlanetElement pe)
        {
            _pe = pe;
        }

        public QsScalar Volume
        {
            get;
            set;
        }

        public string Name { get; set; }


        public long[] Rocks { get; set; }




        static int planets = 0;

        public Planet NextPlanet
        {
            get
            {
                
                return new Planet();
            }
        }

        //public double this[int ix]
        //{
        //    get
        //    {
        //        var rr = new Random(System.Environment.TickCount);
        //        return rr.NextDouble() * ix;
        //    }
        //}

        public Planet this[string planetName]
        {
            get
            {
                var p = new Planet() { Name = planetName };
                return p;
            }
        }


        public int sum(int a, int b)
        {
            return a+b;
        }

        public Planet GetNextPlanet()
        {
            return new Planet();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
