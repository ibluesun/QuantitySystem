using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.Runtime
{
    public enum TokenClass
    {
        Unknown,
        Char,
        Word,
        Number,
        Unit,
        UnitizedNumber,
        Group,
        LeftParenthesis,
        RightParenthesis
    }
}
