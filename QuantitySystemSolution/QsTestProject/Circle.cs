using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsTestProject
{
    public class Circle
    {

        string name;

        Particle[] _Particles;

        public Circle(string name)
        {
            this.name = name;

            _Particles = new Particle[30];

            Random r = new Random();
            for (int i = 0; i < 30; i++)
                _Particles[i] = new Particle
                {
                    M = r.Next(), N=r.Next()
                };
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        Circle _red = null;
        public Circle RedCircle
        {
            get
            {
                if (_red == null) _red = new Circle("Red");
                return _red;
            }
        }

        public double Radius { get; set; }

        public Particle[] Particles
        {
            get
            {
                return _Particles;

            }
        }

        public override string ToString()
        {
            return name;
        }




        private int _ov = new Random().Next();

        public int this[int o]
        {
            get
            {
                return _ov;
            }

            set
            {
                _ov = value;
            }

        }


        public string Tag { get; set; }

    }
}
