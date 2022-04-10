using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantitySystem.Units;


namespace UnitsTestingProject
{
    [TestClass()]
    public class IssuesTests
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


        [TestMethod]
        public void TestPowerUnitsFromRPM()
        {
            var rpm = Unit.ParseQuantity("28<rpm>");
            var hp = Unit.ParseQuantity("0.75<HP>");
            var r = hp / rpm;

            var rn = Unit.ParseQuantity("0<N.m!>") + r;

            Assert.AreEqual(rn.Value, 190.73864029067661);
            var hp_result_a = r * rpm;
            Assert.AreEqual(hp.ToShortString(), hp_result_a.ToShortString());

            var hp_result_b = rn * rpm;


            var hp_res = Unit.ParseQuantity("0<HP>") + hp_result_b;

            Assert.AreEqual(hp.ToShortString(), hp_res.ToShortString());


        }


        [TestMethod]
        public void PathToSIBaseUnitsMultiTest()
        {

            Unit u2 = Unit.Parse("HP/rpm");
            Assert.AreEqual(false, u2.SubUnits[0].IsInverted);
            Assert.AreEqual(true, u2.SubUnits[1].IsInverted);


            Unit mt = Unit.Parse("N.m!");

            var pt1 = u2.PathToUnit(mt);

            Assert.AreEqual(7120.9092375185937, pt1.ConversionFactor);

            Unit rpm =  Unit.Parse("rpm");
            Assert.AreEqual(false, rpm.IsInverted);

            Unit dpm = Unit.Parse("deg/min");

            var pt2 = rpm.PathToUnit(dpm);

            Assert.AreEqual(360.0, Math.Ceiling(pt2.ConversionFactor));


        }


    }
}
