using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units.UnitSystems;
using QuantitySystem.Quantities;
using QuantitySystem.Units.SIUnits.BaseUnits;
using QuantitySystem.Units;
using QuantitySystem.Units.SIUnits;

namespace QuantitySimpleTest
{
    public partial class TestingTorqueForm : Form
    {
        public TestingTorqueForm()
        {
            InitializeComponent();
        }

        private void TestingTorque_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //equation of friction head is 
            // h = f * l/d * v^2/2g


            //where f in laminar   is   64/reynolds
            // in turbulent from moody chart.

            AnyQuantity rn = CalcReynolds();

            AnyQuantity head = CalcHead();

            lblReynolds.Text = rn.ToString();

            lblHead.Text = head.ToString();


        }

        private AnyQuantity CalcReynolds()
        {
            var rho = SIUnitSystem.GetUnitizedQuantityOf<Density>((double)numDensity.Value);

            var v = SIUnitSystem.GetUnitizedQuantityOf<Velocity>((double)numVelocity.Value);

            var l = SIUnitSystem.Default<Metre>((double)numLength.Value);

            var d = SIUnitSystem.Default<Metre>((double)numDiameter.Value);

            var mue = SIUnitSystem.GetUnitizedQuantityOf<Viscosity>((double)numViscosity.Value);


            return rho * v * d / mue;


        }


        private AnyQuantity CalcHead()
        {
            var rho = SIUnitSystem.GetUnitizedQuantityOf<Density>((double)numDensity.Value);

            var v = SIUnitSystem.GetUnitizedQuantityOf<Velocity>((double)numVelocity.Value);

            var l = SIUnitSystem.Default<Metre>((double)numLength.Value);

            var d = SIUnitSystem.Default<Metre>((double)numDiameter.Value);

            var mue = SIUnitSystem.GetUnitizedQuantityOf<Viscosity>((double)numViscosity.Value);


            var g2 = SIUnitSystem.GetUnitizedQuantityOf<Acceleration>(2 * 9.8);


            var reynolds = CalcReynolds();

            var f = 64 / reynolds.Value;

            var fl = l * f;
            
            var result = (fl / d) * (v * v / g2);

            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = SIUnitSystem.Default<Radian>((double)numAngle.Value);

            var time = SIUnitSystem.Default<Second>((double)numTime.Value);

            
            var rl = new Length(1, LengthType.Radius);
            rl.Unit = SIUnitSystem.UnitOf(rl);

            rl.Value = (double)numRL.Value;

            var l = SIUnitSystem.Default<Metre>((double)numRL.Value);


            var f = SIUnitSystem.Default<Newton>((double)numForce.Value);


            var Torque = f * rl;

            lblTorque.Text = Torque.ToString();

            var work = f * l;
            lblWork.Text = work.ToString();


            var AngularSpeed = a / time;

            lblAngularSpeed.Text = AngularSpeed.ToString();

            var power = Torque * AngularSpeed;
            lblPower.Text = power.ToString();

        }
    }
}
