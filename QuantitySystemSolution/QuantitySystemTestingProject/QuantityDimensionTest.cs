using QuantitySystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace QuantitySystemTestingProject
{
    
    
    /// <summary>
    ///This is a test class for QuantityDimensionTest and is intended
    ///to contain all QuantityDimensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QuantityDimensionTest
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
        ///A test for QuantityDimension Constructor
        ///</summary>
        [TestMethod()]
        public void QuantityDimensionConstructorTest1()
        {
            QuantityDimension target = new QuantityDimension();

            Assert.AreEqual(0, target.Mass.Exponent);
            Assert.AreEqual(0, target.Length.Exponent);
            Assert.AreEqual(0, target.Time.Exponent);

            Assert.AreEqual(0, target.Temperature.Exponent);
            Assert.AreEqual(0, target.LuminousIntensity.Exponent);
            Assert.AreEqual(0, target.ElectricCurrent.Exponent);
            Assert.AreEqual(0, target.AmountOfSubstance.Exponent);

        }

        /// <summary>
        ///A test for QuantityDimension Constructor
        ///</summary>
        [TestMethod()]
        public void QuantityDimensionConstructorTest()
        {
            int mass = 1; // TODO: Initialize to an appropriate value
            int length = 2; // TODO: Initialize to an appropriate value
            int time = 3; // TODO: Initialize to an appropriate value
            QuantityDimension target = new QuantityDimension(mass, length, time);

            Assert.AreEqual(1, target.Mass.Exponent);
            Assert.AreEqual(2, target.Length.Exponent);
            Assert.AreEqual(3, target.Time.Exponent);


            Assert.AreEqual(0, target.Temperature.Exponent);
            Assert.AreEqual(0, target.LuminousIntensity.Exponent);
            Assert.AreEqual(0, target.ElectricCurrent.Exponent);
            Assert.AreEqual(0, target.AmountOfSubstance.Exponent);


            
        }
    }
}
