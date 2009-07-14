using QuantitySystem.DimensionDescriptors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace QuantitySystemTestingProject
{
    
    
    /// <summary>
    ///This is a test class for LengthDescriptorTest and is intended
    ///to contain all LengthDescriptorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LengthDescriptorTest
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
        ///A test for LengthDescriptor Constructor
        ///</summary>
        [TestMethod()]
        public void LengthDescriptorConstructorTest()
        {
            float normalExponent = 1; // TODO: Initialize to an appropriate value
            float radiusExponent = -1; // TODO: Initialize to an appropriate value

            LengthDescriptor target = new LengthDescriptor(normalExponent, radiusExponent);

            Assert.AreEqual<float>(0, target.Exponent);
        }

        /// <summary>
        ///A test for Subtract
        ///</summary>
        [TestMethod()]
        public void LengthDescriptorSubtractTest()
        {
            LengthDescriptor target = new LengthDescriptor(3,2); // TODO: Initialize to an appropriate value

            

            LengthDescriptor dimensionDescriptor = new LengthDescriptor(1,1); // TODO: Initialize to an appropriate value

            LengthDescriptor expected = new LengthDescriptor(2,1); // TODO: Initialize to an appropriate value

            LengthDescriptor actual;


            actual = target.Subtract(dimensionDescriptor);

            Assert.AreEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for Multiply
        ///</summary>
        [TestMethod()]
        public void LengthDescriptorMultiplyTest()
        {
            LengthDescriptor target = new LengthDescriptor(3,2); // TODO: Initialize to an appropriate value
            int exponent = 2; // TODO: Initialize to an appropriate value
            LengthDescriptor expected = new LengthDescriptor(6,4); // TODO: Initialize to an appropriate value
            LengthDescriptor actual;
            actual = target.Multiply(exponent);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void LengthDescriptorAddTest()
        {
            LengthDescriptor target = new LengthDescriptor(3,2); // TODO: Initialize to an appropriate value
            LengthDescriptor dimensionDescriptor = new LengthDescriptor(1,1); // TODO: Initialize to an appropriate value
            LengthDescriptor expected = new LengthDescriptor(4,3); // TODO: Initialize to an appropriate value
            LengthDescriptor actual;
            actual = target.Add(dimensionDescriptor);
            Assert.AreEqual(expected, actual);
            
        }
    }
}
