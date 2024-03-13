using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using Qs.Types;
using QuantitySystem.Units;
using System.Drawing;
using QuantitySystem.Units.Shared;
using QuantitySystem.Quantities;

namespace QsGraphics
{
    public class Circle : Shape
    {
        readonly AnyQuantity<double> _x, _y, _radius;



        public Circle(AnyQuantity<double> x, AnyQuantity<double> y, AnyQuantity<double> radius)
        {
            _x = x;
            _y = y;
            _radius = radius;

            var t = new System.Timers.Timer();
        }


        QsFunction xfunc, yfunc, radfunc;


        /// <summary>
        /// constructor accepts quantities and qs functions also
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public Circle(QsScalar x, QsScalar y, QsScalar radius)
        {
            if (x.ScalarType == ScalarTypes.FunctionQuantity)
                xfunc = x.FunctionQuantity.Value;
            else _x = x.NumericalQuantity;

            if (y.ScalarType == ScalarTypes.FunctionQuantity)
                yfunc = y.FunctionQuantity.Value;
            else _y = y.NumericalQuantity;

            if (radius.ScalarType == ScalarTypes.FunctionQuantity)
                radfunc = radius.FunctionQuantity.Value;
            else _radius = radius.NumericalQuantity;

        }

        private Length<double> zm = (Length<double>)Unit.ParseQuantity("0<m>");

        // milli second quantity
        private AnyQuantity<double> zmt = Unit.ParseQuantity("0<m/ms>");


        public override void Draw(Graphics graphics, float pixelPerMeter)
        {
            PointF p1 = new PointF();
            PointF p2 = new PointF();

            // include time into the picture
            Time<double> TQ = (Time<double>)MetricUnit.Milli<Second>(0);

            // prepare for time
            if (Timer.IsRunning)
                TQ = (Time<double>)MetricUnit.Milli<Second>(Timer.ElapsedMilliseconds);

            Length<double> ActualRadius;

            if (radfunc != null)
            {
                ActualRadius = (Length<double>)(zm + radfunc.Invoke(TQ));
            }
            else if (_radius.GetType().Equals(typeof(Speed<double>)))
            {
                ActualRadius = (Length<double>)(zm + (_radius * TQ));
            }
            else
            {
                ActualRadius = (Length<double>)(zm + _radius);
            }

            Length<double> ActualX;

            if (xfunc != null)
            {
                ActualX = (Length<double>)(zm + xfunc.Invoke(TQ));
            }
            else if (_x.GetType().Equals(typeof(Speed<double>)))
            {
                ActualX = (Length<double>)(zm + (_x * TQ));
            }
            else
            {
                ActualX = (Length<double>)(zm + _x);
            }

            Length<double> ActualY;

            if (yfunc != null)
            {
                ActualY = (Length<double>)(zm + yfunc.Invoke(TQ));
            }
            else if (_y.GetType().Equals(typeof(Speed<double>)))
            {
                ActualY = (Length<double>)(zm + (_y * TQ));
            }
            else
            {
                ActualY = (Length<double>)(zm + _y);
            }


            p1.X = (float)((ActualX - ActualRadius).Value * pixelPerMeter);
            p2.X = (float)((ActualX + ActualRadius).Value * pixelPerMeter);

            p1.Y = (float)((ActualY - ActualRadius).Value * pixelPerMeter);
            p2.Y = (float)((ActualY + ActualRadius).Value * pixelPerMeter);


            SizeF sf = new SizeF(p2.X - p1.X, p2.Y - p1.Y);

            RectangleF rc = new RectangleF(p1, sf);

            graphics.DrawEllipse(
                Pens.Blue
                , rc
                );

            // start the timer after drawing the first round
            if (!Timer.IsRunning) Timer.Start();
        }
    }
}
