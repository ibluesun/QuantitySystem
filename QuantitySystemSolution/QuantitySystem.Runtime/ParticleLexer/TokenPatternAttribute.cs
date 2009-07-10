using System;

namespace ParticleLexer
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TokenPatternAttribute : Attribute
    {
        public string RegexPattern { get; set; }
    }
}
