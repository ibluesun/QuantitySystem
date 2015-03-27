using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using Qs.Scripting;
using Qs.Types;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QsTestProject
{
    [TestClass()]
    public class QsTest
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



        ScriptRuntime qsrt = QsContext.CreateRuntime();
        




        [TestMethod()]
        public void TestIssue1()
        {
            var qs = qsrt.GetEngine("Qs");
            // tell qs evaluator that we have external place for variables

            Qs.Runtime.QsEvaluator.CurrentEvaluator.Scope.RegisterScopeStorage("CircleStorage", new CircleStorage());

            var ev = Qs.Runtime.QsEvaluator.CurrentEvaluator;

            DimensionlessQuantity<double> g = 34.2;
            DimensionlessQuantity<string> h = "hello there";

            dynamic r = qs.Execute("_circle->Particles[10]->M");

            //qs.Execute("_c[55]=20");
            qs.Execute("_c->Tag =\"hello\"");

            qs.Execute("_c->RedCircle->Tag=\"hello\"");

            qs.Execute("_c->RedCircle->Particles[20]->M=30");

            var ms = (QsScalar)ev.Evaluate("_c->RedCircle->Particles[20]->M");

            Assert.AreEqual(ms.NumericalQuantity.Value, 30);




            qs.Execute("_c->RedCircle[10]=20");
            var s = (QsScalar)ev.Evaluate("_c->RedCircle[10]");
            Assert.AreEqual(s.NumericalQuantity.Value, 20);
        }
    }
}
