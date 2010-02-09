using Qs.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Qs.Types;
namespace QsTestProject
{
    
    
    /// <summary>
    ///This is a test class for QsFunctionTest and is intended
    ///to contain all QsFunctionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QsFunctionTest
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


        public QsFunction GetTestFunction(string function, params string[] paramNames)
        {
            List<QsParamInfo> lpi = new List<QsParamInfo>();

            foreach (string prm in paramNames)
                lpi.Add(new QsParamInfo() { Name = prm, Type = QsParamType.Value });

            QsFunction target = new QsFunction(function);
            target.Parameters = lpi.ToArray();

            return target;
        }

        /// <summary>
        ///A test for GetParameterOrdinal
        ///</summary>
        [TestMethod()]
        public void GetParameterOrdinalTest()
        {

            var target = GetTestFunction("TestOrdinal", "x", "xx", "yy", "ZezO", "g", "tT");

            string parameterName = "zezo"; // TODO: Initialize to an appropriate value

            int expected = 3; 


            int actual = target.GetParameterOrdinal(parameterName);
            
            Assert.AreEqual(expected, actual);

            parameterName = "ZeZo";  // case incensitive test.
            actual = target.GetParameterOrdinal(parameterName);

            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ContainsParameter
        ///</summary>
        [TestMethod()]
        public void ContainsParameterTest()
        {
            QsFunction target = GetTestFunction("TestContains", "x", "Gh", "FyI", "Df");



            Assert.AreEqual<bool>(true, target.ContainsParameter("gh"));
            Assert.AreEqual<bool>(true, target.ContainsParameter("fyi"));
            Assert.AreEqual<bool>(false, target.ContainsParameter("fluid"));
            Assert.AreEqual<bool>(false, target.ContainsParameter("do"));
            Assert.AreEqual<bool>(true, target.ContainsParameter("x"));



            
        }

        /// <summary>
        ///A test for ContainsParameters
        ///</summary>
        [TestMethod()]
        public void ContainsParametersTest()
        {

            QsFunction target = GetTestFunction("TestContains", "fluid", "x", "h", "u", "s", "v");


            Assert.AreEqual<bool>(false, target.ContainsParameters("gh"));

            Assert.AreEqual<bool>(true, target.ContainsParameters("fluid", "x"));

            Assert.AreEqual<bool>(false, target.ContainsParameters("fluid", "y"));

            Assert.AreEqual<bool>(true, target.ContainsParameters("fluid", "u", "x", "s", "v"));

            Assert.AreEqual<bool>(true, target.ContainsParameters("h", "s"));

            Assert.AreEqual<bool>(true, target.ContainsParameters("u", "v"));


        }

        /// <summary>
        ///A test for IsItFunctionScopeName
        ///</summary>
        [TestMethod()]
        public void IsItFunctionScopeNameTest()
        {
            string name = "F#2"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = QsFunction.IsItFunctionSymbolicName(name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FormFunctionSymbolicName
        ///</summary>
        [TestMethod()]
        public void FormFunctionSymbolicNameTest()
        {
            string functionName = "f"; // TODO: Initialize to an appropriate value
            string[] paramNames = {"u","v"}; // TODO: Initialize to an appropriate value
            string expected = "f#2_u_v"; // TODO: Initialize to an appropriate value
            string actual;
            actual = QsFunction.FormFunctionSymbolicName(functionName, paramNames);
            Assert.AreEqual(expected, actual);
            
        }
    }
}
