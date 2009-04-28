using QuantitySystem.Quantities.BaseQuantities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem;
using QuantitySystem.Units;
using QuantitySystem.Units.Metric.SI;
namespace QuantitySystemTestingProject
{
    /// <summary>
    ///This is a test class for MassTest and is intended
    ///to contain all MassTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MassTest
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
        ///A test for Mass`1 Constructor
        ///</summary>
        public void MassConstructorTestHelper<T>()
        {
            Mass<T> target = new Mass<T>();

            QuantityDimension MassDimension = new QuantityDimension(1, 0, 0);


            Assert.AreEqual(MassDimension, target.Dimension);
        }



        [TestMethod()]
        public void MassConstructorTest()
        {
            MassConstructorTestHelper<GenericParameterHelper>();
        }


        [TestMethod()]
        public void MassAddTest()
        {
            Mass<double> l1 = 50;

            Mass<double> l2 = 50;

            var l = l1 + l2;

            Assert.AreEqual<double>(100, l.Value);

        }

        [TestMethod()]
        public void UnitMassAddTest()
        {
            var l1 = MetricUnit.Kilo<Gram>(2);

            var l2 = MetricUnit.None<Gram>(500);

            var l = l1 + l2;

            Assert.AreEqual<double>(2.5, l.Value);

            var lr = l2 + l1;
            Assert.AreEqual<double>(2500, lr.Value);


        }


        [TestMethod()]
        public void MassSubtractTest()
        {
            Mass<double> l1 = 150;

            Mass<double> l2 = 70;

            var l = l1 - l2;

            Assert.AreEqual<double>(80, l.Value);

            

        }

        [TestMethod()]
        public void MassMultiplyTest()
        {
            Mass<double> m1 = 20;

            Mass<double> m2 = 20;

            var m = m1 * m2;

            //test the value
            Assert.AreEqual(400, m.Value);

            
            //test the quantity

            Assert.AreEqual(m.Dimension, new QuantityDimension(2, 0, 0));

            
        }

        [TestMethod()]
        public void MassDivideTest()
        {
            Mass<double> m1 = 20;

            Mass<double> m2 = 20;

            var m = m1 / m2;

            Assert.AreEqual(1, m.Value);

            //test the quantity
            Assert.AreEqual(m.Dimension, new QuantityDimension());

        }

        [TestMethod()]
        public void MassComplexAddTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };

            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 + m2;

            ComplexNumber expected = new ComplexNumber { Real = 3, Imaginary = 14 };


            Assert.AreEqual(expected, m.Value);

        }



        [TestMethod()]
        public void MassComplexSubtractTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };

            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 - m2;

            ComplexNumber expected = new ComplexNumber { Real = -1, Imaginary = 6 };


            Assert.AreEqual(expected, m.Value);

        }

        [TestMethod()]
        public void MassComplexMultiplyTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };

            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 4 };

            var m = m1 * m2;

            ComplexNumber expected = new ComplexNumber { Real = 2, Imaginary = 40 };


            Assert.AreEqual(expected, m.Value);

            //test the quantity
            Assert.AreEqual(m.Dimension, new QuantityDimension(2,0,0));

        }



        [TestMethod()]
        public void MassComplexDivideTest()
        {
            Mass<ComplexNumber> m1 = new ComplexNumber { Real = 1, Imaginary = 10 };

            Mass<ComplexNumber> m2 = new ComplexNumber { Real = 2, Imaginary = 5 };

            var m = m1 / m2;

            ComplexNumber expected = new ComplexNumber { Real = 0.5, Imaginary = 2 };


            Assert.AreEqual(expected, m.Value);

            //test the quantity
            Assert.AreEqual(m.Dimension, new QuantityDimension());

        }



    }
}
