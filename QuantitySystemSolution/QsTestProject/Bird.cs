using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QsTestProject
{
    class Bird
    {

        public readonly Guid Id = Guid.NewGuid();

        public void SooSoo()
        {
            Debug.Print("Soo soo soo soo ..");
        }
    }
}
