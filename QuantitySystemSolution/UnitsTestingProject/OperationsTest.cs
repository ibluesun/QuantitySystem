using QuantitySystem.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units.Metric.SI;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Units.English;

namespace UnitsTestingProject
{
    [TestClass()]
    public class OperationsTest
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


        [TestMethod()]
        public void DivideDifferentUnitsAndSameDimension()
        {
            Mass<double> on = 120.0;
            Mass<double> l = 2;

            on.Unit = new Ounce();
            l.Unit = new Gram();


            var g = on * l;
            var gd = on / l;


            Assert.AreEqual(gd.Value, 60.0);
            Assert.AreEqual(gd.Unit.Symbol, "<oz/kg>");


            Assert.AreEqual(g.Value, 240.0);
            Assert.AreEqual(g.Unit.Symbol, "<oz.kg>");


            //test kg.kg  == kg^2
            //  and in turn convert oz.kg into [kg or oz]
            // 




        }


        [TestMethod()]
        public void AddDifferentUnitsAndSameDimension()
        {
            Mass<double> on = 1;
            on.Unit = new Ounce();


            var l = MetricUnit.None<Gram>(0);


            var g =  on + l;

            Assert.AreEqual(g.Value, 1);


            
        }
    }
}
