using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using QuantitySystem;
using QuantitySystem.Quantities;
using QuantitySystem.Units;
using QuantitySystem.Units.Metric;
using QuantitySystem.Units.Metric.SI;
using QuantitySystem.Units.Metric.Cgs;
using QuantitySystem.Units.Metric.Mts;

using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.Shared;


namespace UnitsTestingProject
{
    /// <summary>
    /// Summary description for MetricTests
    /// </summary>
    [TestClass]
    public class MetricTests
    {
        public MetricTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestBaryeUnit()
        {
            Barye u = new Barye();

            UnitPath up =  u.PathToDefaultUnit();

            Assert.AreEqual(0.1, up.ConversionFactor);


        }

        [TestMethod]
        public void TestDefaultUnits()
        {
            //the scenario is to get the default unit of the metric system
            // if we are in si then si units are always default units.
            // if we are in inherited system then we search for the native unit for this system
            // if we couldn't find it we should return the SI default unit.
            //     but there are units like cm in cgs are the default units in the corresponding inherited metric system
            //       the return value will be the SI unit type as it is
            //          however in creation of this unit for specific system it should be created based on its default prefix attribute




            Type mt = Unit.GetDefaultUnitTypeOf(typeof(Mass<>), "Metric.Mts");

            Assert.AreEqual(typeof(MetricTonne), mt);


            Type mm = Unit.GetDefaultUnitTypeOf(typeof(Length<>), "Metric.mts");
            Assert.AreEqual(typeof(Metre), mm);

            mm = Unit.GetDefaultUnitTypeOf(typeof(Length<>), "Metric.cgs");
            Assert.AreEqual(typeof(Metre), mm);

            mm = Unit.GetDefaultUnitTypeOf(typeof(Time<>), "Metric.cgs");
            Assert.AreEqual(typeof(Second), mm);

            mm = Unit.GetDefaultUnitTypeOf(typeof(Volume<>), "Metric.cgs");
            Assert.AreEqual(null, mm);

            mm = Unit.GetDefaultUnitTypeOf(typeof(Volume<>), "Metric.mts");
            Assert.AreEqual(typeof(Stère), mm);

            mm = Unit.GetDefaultUnitTypeOf(typeof(Frequency<>), "Metric.cgs");
            Assert.AreEqual(typeof(Hertz), mm);






        }



        [TestMethod]
        public void TestPrefixMultiplication()
        {

            var l1 = Unit.ParseQuantity("5<km>");
            var l2 = Unit.ParseQuantity("2<km>");
            var l3 = Unit.ParseQuantity("20<m>");

            var v = l1 * l2 * l3;

            Assert.AreEqual<double>(actual: v.Value, expected: 200);
            Assert.AreEqual(actual: v.Unit.Symbol, expected: "<hm^3>");

        }
    
    
    }
}
