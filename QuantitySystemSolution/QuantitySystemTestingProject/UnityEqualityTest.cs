using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Units;
using QuantitySystem.Units.Metric.SI;
using QuantitySystem.Units.English;
namespace QuantitySystemTestingProject
{
    [TestClass()]
    public class UnityEqualityTest
    {
        [TestMethod()]
        public void ListTest()
        {
            Unit millimetre = new Metre() { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre() { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre() { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();
            List<Unit> units = new List<Unit>();
            units.Add(millimetre);
            units.Add(centimetre);
            units.Add(decimetre);
            units.Add(inch);

            //Theese should work, of course
            Assert.IsTrue(units.Contains(millimetre));
            Assert.IsTrue(units.Contains(centimetre));
            Assert.IsTrue(units.Contains(decimetre));
            Assert.IsTrue(units.Contains(inch));

            Unit newMillimetre = new Metre() { UnitPrefix = MetricPrefix.Milli };
            Unit newInch = new Inch();
            //Why this does not work? After all, isn't a millimetre always a millimetre?
            Assert.IsTrue(units.Contains(newMillimetre));
            Assert.IsTrue(units.Contains(newInch));

            bool containsNewMillimetre = false;
            bool containsNewInch = false;
            foreach (Unit unit in units)
            {
                if (unit == newMillimetre)
                {
                    containsNewMillimetre = true;
                }
                if (unit == newInch)
                {
                    containsNewInch = true;
                }
            }
            Assert.IsTrue(containsNewMillimetre);
            Assert.IsTrue(containsNewInch);
        }
        [TestMethod()]
        public void DictionaryTest()
        {
            Unit millimetre = new Metre() { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre() { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre() { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();
            Dictionary<Unit, string> units = new Dictionary<Unit, string>();
            units.Add(millimetre, millimetre.Symbol);
            units.Add(centimetre, centimetre.Symbol);
            units.Add(decimetre, decimetre.Symbol);
            units.Add(inch, inch.Symbol);

            //Theese should work, of course
            Assert.IsTrue(units.ContainsKey(millimetre));
            Assert.IsTrue(units.ContainsKey(centimetre));
            Assert.IsTrue(units.ContainsKey(decimetre));
            Assert.IsTrue(units.ContainsKey(inch));

            Unit newMillimetre = new Metre() { UnitPrefix = MetricPrefix.Milli };
            Unit newInch = new Inch();
            //Why this does not work? After all, isn't a millimetre always a millimetre?
            Assert.IsTrue(units.ContainsKey(newMillimetre));
            Assert.IsTrue(units.ContainsKey(newInch));

            bool containsNewMillimetre = false;
            bool containsNewInch = false;
            foreach (Unit unit in units.Keys)
            {
                if (unit == newMillimetre)
                {
                    containsNewMillimetre = true;
                }
                if (unit == newInch)
                {
                    containsNewInch = true;
                }
            }
            Assert.IsTrue(containsNewMillimetre);
            Assert.IsTrue(containsNewInch);

            Assert.IsTrue(units[newMillimetre] == millimetre.Symbol);
            Assert.IsTrue(units[newInch] == inch.Symbol);
        }

        private bool AreNotEqual(Unit first, Unit other)
        {
            return first != other;
        }

        [TestMethod()]
        public void MoreEqualityTestsTest()
        {
            Unit millimetre = new Metre() { UnitPrefix = MetricPrefix.Milli };
            Unit centimetre = new Metre() { UnitPrefix = MetricPrefix.Centi };
            Unit decimetre = new Metre() { UnitPrefix = MetricPrefix.Deci };
            Unit inch = new Inch();

            Assert.IsTrue(millimetre != centimetre);
            Assert.IsTrue(AreNotEqual(millimetre,centimetre));
        }
    }
}
