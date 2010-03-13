using ParticleSymbolic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace QsTestProject
{
    
    
    /// <summary>
    ///This is a test class for SymbolicOperationsTest and is intended
    ///to contain all SymbolicOperationsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SymbolicOperationsTest
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
        ///A test for Diff
        ///</summary>
        [TestMethod()]
        public void DiffTest()
        {
            string equation = "3*x^2+4*x^3";

            string variable = "x"; 

            string expected = "6*x+12*x^2"; 

            string actual;

            actual = SymbolicOperations.Diff(equation, variable);

            Assert.AreEqual(expected, actual);
        }
    }
}
