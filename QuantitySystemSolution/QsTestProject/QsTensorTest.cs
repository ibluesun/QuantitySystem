using Qs.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qs;
using Microsoft.Scripting.Hosting;


namespace QsTestProject
{
    
    
    /// <summary>
    ///This is a test class for QsTensorTest and is intended
    ///to contain all QsTensorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QsTensorTest
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
        ///A test for GetScalar
        ///</summary>
        [TestMethod()]
        public void GetScalarTest()
        {


            ScriptRuntime qsruntime =  Qs.Qs.CreateRuntime();
            ScriptEngine QsEngine = qsruntime.GetEngine("Qs");

            QsTensor  t = QsEngine.Execute("<|3 4; 3 1 | 9 8; 4 7  |>") as QsTensor;


            var sv = t.GetScalar(0, 0, 0);

            Assert.AreEqual("3 <1>", sv.ToShortString());


            t = QsEngine.Execute("<| <|3 4; 3 1 | 9 8; 4 7|> | <|30 4; 3 1 | 9 18; 4 7|> |>") as QsTensor;

            // getting scalar from 4th rank tensor.
            sv = t.GetScalar(1, 1, 0, 1);

            Assert.AreEqual("18 <1>", sv.ToShortString());


            t = QsEngine.Execute("<| <| <|3 4; 3 1 | 9 8; 4 7|> | <|30 4; 3 1 | 9 18; 4 7|> |>  |  <| <|3 4;4 1 | 9 8; 4 7|> | <|30 403<L>; 302 1 | 19 180; 40 700|> |> |>") as QsTensor;

            // getting scalar from 5th rank tensor.
            sv = t.GetScalar(1, 1, 0, 0, 1);

            Assert.AreEqual("403 <L>", sv.ToShortString());
            
        }
    }
}
