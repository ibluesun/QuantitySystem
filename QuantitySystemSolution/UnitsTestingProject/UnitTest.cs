﻿using QuantitySystem.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QuantitySystem.Quantities.BaseQuantities;

using QuantitySystem.Units.SI;
using QuantitySystem.Units.Imperial;
using QuantitySystem.Quantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace UnitsTestingProject
{
    
    
    /// <summary>
    ///This is a test class for UnitTest and is intended
    ///to contain all UnitTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnitTest
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
        ///A test for GetSIUnitTypeOf
        ///</summary>
        [TestMethod()]
        public void GetSIUnitTypeOfTest()
        {
            Type quantityType = typeof(Length<>); // TODO: Initialize to an appropriate value

            Type expected = typeof(Metre); // TODO: Initialize to an appropriate value

            Type actual;
            actual = Unit.GetSIUnitTypeOf(quantityType);
            
            Assert.AreEqual(expected, actual);


            quantityType = typeof(Mass<>);
            expected = typeof(Gram);
            actual = Unit.GetSIUnitTypeOf(quantityType);

            Assert.AreEqual(expected, actual);



            quantityType = typeof(Area<>);
            expected = null;
            actual = Unit.GetSIUnitTypeOf(quantityType);

            Assert.AreEqual(expected, actual);



            quantityType = typeof(Length<double>);
            expected = typeof(Metre);
            actual = Unit.GetSIUnitTypeOf(quantityType);

            Assert.AreEqual(expected, actual);

            quantityType = typeof(Mass<int>);
            expected = typeof(Gram);
            actual = Unit.GetSIUnitTypeOf(quantityType);

            Assert.AreEqual(expected, actual);


            
        }

        /// <summary>
        ///A test for GetDefaultUnitTypeOf
        ///</summary>
        [TestMethod()]
        public void GetDefaultUnitTypeOfTest()
        {
            Type quantityType = typeof(AmountOfSubstance<>); // TODO: Initialize to an appropriate value
            
            string unitSystem = "SI"; // TODO: Initialize to an appropriate value

            
            Type expected = typeof(Mole); // TODO: Initialize to an appropriate value
            Type actual;
            actual = Unit.GetDefaultUnitTypeOf(quantityType, unitSystem);
        
            Assert.AreEqual(expected, actual);


            quantityType = typeof(Length<>);
            expected = typeof(Metre);

            actual = Unit.GetDefaultUnitTypeOf(quantityType, unitSystem);
            Assert.AreEqual(expected, actual);


            unitSystem = "imperial";
            expected = typeof(Foot);

            actual = Unit.GetDefaultUnitTypeOf(quantityType, unitSystem);
            Assert.AreEqual(expected, actual);


            quantityType = typeof(Volume<>);
            expected = null;

            actual = Unit.GetDefaultUnitTypeOf(quantityType, unitSystem);
            Assert.AreEqual(expected, actual);


            quantityType = typeof(Length<decimal>);
            expected = typeof(Foot);

            actual = Unit.GetDefaultUnitTypeOf(quantityType, unitSystem);
            Assert.AreEqual(expected, actual);

        }



        
        /// <summary>
        ///A test for GetThisUnitQuantity
        ///</summary>
        public void GetThisUnitQuantityTestHelper<T>()
        {
            Unit target = new Yard(); // TODO: Initialize to an appropriate value
            AnyQuantity<T> expected = new Length<T>(); // TODO: Initialize to an appropriate value
            
            AnyQuantity<T> actual;
            actual = target.GetThisUnitQuantity<T>();

            Assert.AreEqual(expected, actual);

            target = new Kelvin();

            expected = new Temperature<T>();

            actual = target.GetThisUnitQuantity<T>();

            Assert.AreEqual(expected, actual);

            
        }


        [TestMethod()]
        public void GetThisUnitQuantityTest()
        {
            GetThisUnitQuantityTestHelper<GenericParameterHelper>();
        }


        [TestMethod()]
        public void TestDynamicUnitCreationFlatUnits()
        {
            //when conducting this test, 
            // the type of quantity is sent to the Unit constructor
            // the return value should be units in its base form which makes the unit

            Unit unit = new Unit(typeof(Area<>));

            Assert.AreEqual("<m^2>", unit.Symbol);


            unit = new Unit(typeof(Force<>));

            Assert.AreEqual("<N>", unit.Symbol);


            unit = new Unit(typeof(Pressure<>));

            Assert.AreEqual("<Pa>", unit.Symbol);


            unit = new Unit(typeof(Viscosity<>));

            Assert.AreEqual("<kg/m.s>", unit.Symbol);

            unit = new Unit(typeof(Density<>));

            Assert.AreEqual("<kg/m^3>", unit.Symbol);


            unit = new Unit(typeof(Mass<>));
            Assert.AreEqual("<kg>", unit.Symbol);


            unit = new Unit(typeof(Angle<Double>));
            Assert.AreEqual("<1>", unit.Symbol);

            unit = new Unit(typeof(Reynolds<>));
            Assert.AreEqual("<1>", unit.Symbol);


            unit = new Unit(typeof(Torque<>));
            Assert.AreEqual("<kg.m^2/s^2>", unit.Symbol);

            unit = new Unit(typeof(Energy<>));
            Assert.AreEqual("<J>", unit.Symbol);



        }



        [TestMethod()]
        public void TestDynamicUnitCreationQuantityUnits()
        {
            // this test is extracting the units in its derived symbols from the running 
            // instance of the quantity.


            Unit unit = new Unit(new Length<double>());

            Assert.AreEqual("<m>", unit.Symbol);

            unit = new Unit(new Area<double>());

            Assert.AreEqual("<m^2>", unit.Symbol);


            unit = new Unit(new Force<double>());

            Assert.AreEqual("<N>", unit.Symbol);

            unit = new Unit(new Volume<double>());

            Assert.AreEqual("<m^3>", unit.Symbol);

            unit = new Unit(new Density<double>());

            Assert.AreEqual("<kg/m^3>", unit.Symbol);

            unit = new Unit(new Pressure<double>());

            Assert.AreEqual("<Pa>", unit.Symbol);


            unit = new Unit(new Viscosity<double>());

            Assert.AreEqual("<Pa.s>", unit.Symbol);


            unit = new Unit(new Mass<double>());
            Assert.AreEqual("<kg>", unit.Symbol);

            unit = new Unit(new Angle<double>());
            Assert.AreEqual("<m/m>", unit.Symbol);

            unit = new Unit(new Reynolds<double>());
            Assert.AreEqual("<<kg/m^3>.<m/s>.m/Pa.s>", unit.Symbol);


            unit = new Unit(new Torque<double>());
            Assert.AreEqual("<N.m>", unit.Symbol);

            unit = new Unit(new Energy<double>());
            Assert.AreEqual("<J>", unit.Symbol);


        }

    }
}
