using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Units;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using QuantitySystem.Units.Shared;
using System.Threading;
using Qs.Types;

namespace QsGraphics
{
    
    public class Screen
    {

        private readonly int _Width;
        private readonly int _Height;
        private double _PixelPerMeter;

        public double PixelPerMeter
        {
            get { return _PixelPerMeter; }
            set { _PixelPerMeter = value; }
        }

        private readonly ScreenForm _Form;
        private readonly Func<DialogResult> _FormDialog;
        private readonly Task _FormTask;


        private readonly Graphics OffGraphics;
        private readonly Bitmap OffBitmap;


        private bool loop = true;

        /// <summary>
        /// Screen Constructor
        /// </summary>
        /// <param name="width">width in pixels</param>
        /// <param name="height">height in pixels</param>
        /// <param name="pixelPerMeter">Pixels Per Meter</param>
        public Screen(int width, int height, double pixelPerMeter)
        {
            _Width = width;
            _Height = height;
            _PixelPerMeter = pixelPerMeter;

            _Form = new ScreenForm();
            _Form.Width = _Width;
            _Form.Height = _Height;
            
            // show the form in separeate task i.e. thread
            _FormDialog = _Form.ShowDialog;

            _FormTask = Task.Factory.StartNew(_FormDialog);

            // here we will go into infinite loop based on timer
            // and 

            OffBitmap = new Bitmap(_Width, _Height);

            OffGraphics = Graphics.FromImage(OffBitmap);            

        }

        public void End()
        {
            Action a = _Form.Close;
            if(_Form.IsHandleCreated)
                _Form.Invoke(a);          //execute the code of the form in its running thread by invoke
        }

        ~Screen()
        {
            End();
        }


        private AnyQuantity<double> zm = Unit.ParseQuantity("0<m>");
        //private AnyQuantity<double> zrm = Unit.ParseQuantity("0<m!>");


        List<Shape> Shapes = new List<Shape>();

       
        /// <summary>
        /// Draw a circle on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public Shape BareCircle(AnyQuantity<double> x, AnyQuantity<double> y, AnyQuantity<double> radius)
        {
            Shape s;
            lock (Shapes)
            {
                s = new Circle(x, y, radius);
                Shapes.Add(s);
            }

            return s;
        }


        public Shape Circle(QsScalar x, QsScalar y, QsScalar radius)
        {
            Shape s;
            lock (Shapes)
            {
                s = new Circle(x, y, radius);
                Shapes.Add(s);
            }

            return s;
        }

        public void Reset()
        {
            loop = false;
            foreach (Shape sh in Shapes) sh.Reset();
        }


        /// <summary>
        /// Clear to white colour
        /// </summary>
        public void Clear()
        {
            Shapes.Clear();
        }

        public void Update()
        {
            // clear the off screen
            OffGraphics.Clear(Color.White);

            // draw the shapes
            foreach (Shape sp in Shapes)
                sp.Draw(OffGraphics, (float)_PixelPerMeter);

            // draw the offscreen into the displayed screen

            _Form.FormGraphics.DrawImage(OffBitmap, 0, 0);

            Thread.Sleep(0);

        }

        public void UpdateFor(Time<double> time)
        {
            Stopwatch sw = new Stopwatch();
            var mtime = MetricUnit.Milli<Second>(0) + time;

            loop = true;

            sw.Start();

            Task.Factory.StartNew(() =>
            {
                while (sw.ElapsedMilliseconds < mtime.Value && loop==true)
                    Update();

                //stop the timer.
                sw.Stop();
            });

        }
        
        public void UpdateForever()
        {
            Stopwatch sw = new Stopwatch();

            loop = true;

            sw.Start();

            Task.Factory.StartNew(() =>
            {
                while (loop)
                    Update();

                //stop the timer.
                sw.Stop();
            });

        }

        public void Stop()
        {
            loop = false;
        }


    }
}
