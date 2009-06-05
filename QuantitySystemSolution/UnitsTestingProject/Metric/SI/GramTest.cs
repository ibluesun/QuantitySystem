using QuantitySystem.Units.Metric.SI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
namespace UnitsTestingProject
{
    
    
    /// <summary>
    ///This is a test class for GramTest and is intended
    ///to contain all GramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GramTest
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
        ///A test for Gram Constructor
        ///</summary>
        [TestMethod()]
        public void GramConstructorTest()
        {
            Gram target = new Gram();

            Assert.AreEqual("kg", target.Symbol);
            Assert.AreEqual(typeof(Mass<>), target.QuantityType);
            Assert.AreEqual(MetricPrefix.Kilo, target.UnitPrefix);

            

            Assert.AreEqual(true, target.IsBaseUnit);
            Assert.AreEqual(true, target.IsDefaultUnit);


        }


    }
}
