using QuantitySystem.Units.Metric;
using QuantitySystem.Units.Metric.SI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Units;

namespace UnitsTestingProject
{
    
    
    /// <summary>
    ///This is a test class for SIUnitTest and is intended
    ///to contain all SIUnitTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SIUnitTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion




        /// <summary>
        ///A test for PathToDefaultUnit
        ///</summary>
        [TestMethod()]
        public void PathToDefaultUnitTest()
        {
            //from pico Gram to default Kilo Gram
            Unit target = new Gram();

            ((MetricUnit)target).UnitPrefix = MetricPrefix.None;


            UnitPathStack actual = target.PathToDefaultUnit();
            double expected = System.Math.Pow(10, -3);

            Assert.AreEqual(expected, actual.ConversionFactor);


            target = new Metre();
            ((MetricUnit)target).UnitPrefix = MetricPrefix.Tera;
            actual = target.PathToDefaultUnit();
            expected = System.Math.Pow(10, 12);

            Assert.AreEqual(expected, actual.ConversionFactor);



            target = new Unit(typeof(QuantitySystem.Quantities.Volume<>));
            actual = target.PathToDefaultUnit(); 
            expected = 1;
            Assert.AreEqual(expected, actual.ConversionFactor);



            target = new QuantitySystem.Units.Metric.Litre();
            //((SIUnit)target).UnitPrefix = SIPrefix.Kilo;
            actual = target.PathToDefaultUnit(); // from Litre to m^3
            expected = 1e-3;
            Assert.AreEqual(expected, actual.ConversionFactor);

            target = new QuantitySystem.Units.Metric.Litre();
            ((MetricUnit)target).UnitPrefix = MetricPrefix.Kilo;
            actual = target.PathToDefaultUnit(); // from Litre to m^3
            expected = 1;
            Assert.AreEqual(expected, actual.ConversionFactor);

            target = new QuantitySystem.Units.Metric.Bar();
            //((SIUnit)target).UnitPrefix = SIPrefix.Kilo;
            actual = target.PathToDefaultUnit(); // from Litre to m^3
            expected = 1e+5;
            Assert.AreEqual(expected, actual.ConversionFactor);



            target = new QuantitySystem.Units.Metric.Cgs.Dyne();
            actual = target.PathToDefaultUnit();
            expected = 1e-5;
            Assert.AreEqual(expected, actual.ConversionFactor);


            
        }


        /// <summary>
        ///A test for PathFromDefaultUnit
        ///</summary>
        [TestMethod()]
        public void PathFromDefaultUnitTest()
        {
            MetricUnit target = new Gram();

            target.UnitPrefix = MetricPrefix.Milli;


            UnitPathStack actual = target.PathFromDefaultUnit();
            double expected = System.Math.Pow(10, 6);

            Assert.AreEqual(expected, actual.ConversionFactor);


            target = new Metre();
            target.UnitPrefix = MetricPrefix.Tera;
            actual = target.PathFromDefaultUnit();
            expected = System.Math.Pow(10, -12);

            Assert.AreEqual(expected, actual.ConversionFactor);

        }
    }
}
