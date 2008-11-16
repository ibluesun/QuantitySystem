using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem;

namespace QuantitySystemTestingProject
{
    
    
    /// <summary>
    ///This is a test class for LengthTest and is intended
    ///to contain all LengthTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LengthTest
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
        ///A test for Dimension
        ///</summary>
        public void DimensionTestHelper<T>()
        {
            Length<T> target = new Length<T>(); // TODO: Initialize to an appropriate value
            

            QuantityDimension LengthDimension = new QuantityDimension(0, 1, 0);


            Assert.AreEqual(LengthDimension, target.Dimension);

        }

        [TestMethod()]
        public void DimensionTest()
        {
            DimensionTestHelper<GenericParameterHelper>();
        }

    }

}
