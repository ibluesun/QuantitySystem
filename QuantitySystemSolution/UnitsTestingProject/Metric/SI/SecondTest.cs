using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units;
using QuantitySystem.Units.Metric;
using QuantitySystem.Units.Metric.SI;
using QuantitySystem.Units.Shared;

namespace UnitsTestingProject
{
    
    
    /// <summary>
    ///This is a test class for SecondTest and is intended
    ///to contain all SecondTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SecondTest
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
        ///A test for Second Constructor
        ///</summary>
        [TestMethod()]
        public void SecondConstructorTest()
        {
            Second target = new Second();
            Assert.AreEqual("s", target.Symbol);
            Assert.AreEqual(typeof(Time<>), target.QuantityType);
            Assert.AreEqual(MetricPrefix.None, target.UnitPrefix);

            Assert.AreEqual(true, target.IsBaseUnit);
            Assert.AreEqual(true, target.IsDefaultUnit);
        }
    }
}
