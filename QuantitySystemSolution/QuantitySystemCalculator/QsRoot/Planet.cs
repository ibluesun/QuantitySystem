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

    }
}
