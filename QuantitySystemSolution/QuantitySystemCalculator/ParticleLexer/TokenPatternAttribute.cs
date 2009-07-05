using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleLexer
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TokenPatternAttribute : Attribute
    {
        public string RegexPattern { get; set; }
    }
}
