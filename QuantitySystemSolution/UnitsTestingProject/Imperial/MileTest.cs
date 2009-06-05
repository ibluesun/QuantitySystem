using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using QuantitySystem.Units.English;
namespace UnitsTestingProject
{
    
    
    /// <summary>
    ///This is a test class for MileTest and is intended
    ///to contain all MileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MileTest
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
        ///A test for Mile Constructor
        ///</summary>
        [TestMethod()]
        public void MileConstructorTest()
        {
            Mile target = new Mile();

            Assert.AreEqual("mil", target.Symbol);
            Assert.AreEqual(typeof(Length<>), target.QuantityType);
            
            Assert.AreEqual(true, target.IsBaseUnit);
            Assert.AreEqual(false, target.IsDefaultUnit);
            Assert.AreEqual(typeof(Yard), target.ReferenceUnit.GetType());
        }
    }
}
