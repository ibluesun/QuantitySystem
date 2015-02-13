using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities;

namespace QuantitySystemTestingProject
{
    
    
    /// <summary>
    ///This is a test class for AnyQuantityTest and is intended
    ///to contain all AnyQuantityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AnyQuantityTest
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


        [TestMethod()]
        public void VelocityQuantityTest()
        {
            //testing the quantities in general

            Length<double> l = 20;
            Time<double> t = 10;

            var q = l / t;

            var expected = new Speed<double> { Value = 2 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);
            
        }

        [TestMethod()]
        public void AccelerationQuantityTest()
        {
            //testing the quantities in general

            Speed<double> l = 20;
            Time<double> t = 10;

            var q = l / t;

            var expected = new Acceleration<double> { Value = 2 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);

        }

        [TestMethod()]
        public void ForceQuantityTest()
        {
            //testing the quantities in general

            Acceleration<double> a = 20;
            Mass<double> m = 10;

            var q = m * a;

            var expected = new Force<double> { Value = 200 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);

        }


        [TestMethod()]
        public void PressureQuantityTest()
        {
            //testing the quantities in general

            Force<double> f = 20;
            Area<double> a = 10;

            var q = f / a;

            var expected = new Pressure<double> { Value = 2 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);

        }

        [TestMethod()]
        public void EnergyQuantityTest()
        {
            //testing the quantities in general

            Force<double> f = 20;
            Length<double> l = 10;

            var q = f * l;

            var expected = new Energy<double> { Value = 200 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);

        }

        [TestMethod()]
        public void PowerQuantityTest()
        {
            //testing the quantities in general

            Energy<double> e = 20;
            Time<double> t = 10;

            var q = e / t;

            var expected = new Power<double> { Value = 2 };

            //compare the quantities
            Assert.AreEqual(expected, q);

            //compare the values
            Assert.AreEqual(expected.Value, q.Value);

        }


        /// <summary>
        /// Testing to reach power after several calculation
        /// </summary>
        [TestMethod()]
        public void PowerIncrementalQuantityTest()
        {
            //testing the quantities in general

            Length<double> l = 100;
            Time<double> t = 2;

            var v = l / t;                      //50
                
            var a = v / t;                      //25 m/s^2

            Mass<double> m = 10;

            var f = m * a;                      //250 Newton

            var work = f * l;                   //25000 joule

            var power = work / t;               //12500 watt


            var expected = new Power<double> { Value = 12500 };

            //compare the quantities
            Assert.AreEqual(expected, power);

            //compare the values
            Assert.AreEqual(expected.Value, power.Value);

        }


        /// <summary>
        /// Testing to reach power but in one line calculation.
        /// </summary>
        [TestMethod()]
        public void PowerInOneLineQuantityTest()
        {
            //testing the quantities in general

            Length<double> l = 100;
            Time<double> t = 2;


            Mass<double> m = 10;


            var power = ((m * ((l / t) / t)) * l) / t;               //12500 watt


            var expected = new Power<double> { Value = 12500 };

            //compare the quantities
            Assert.AreEqual(expected, power);

            //compare the values
            Assert.AreEqual(expected.Value, power.Value);

        }

    }
}
