using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.RuntimeTypes;
using Qs.Runtime;

namespace Qs.Modules
{
    /// <summary>
    /// This class is for namespace multiple function declaration overloaded by the argument name.
    /// This module for testing purposes only.
    /// </summary>
    [Obsolete("Remove this class later")]
    public static class Thermo
    {
        private static QsValue MsgBox(string text)
        {
            return Windows.MessageBox(QsParameter.MakeParameter(null, text));
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyTP(QsParameter fluid, QsParameter t, QsParameter p)
        {
            return MsgBox(fluid.RawValue + "(T = " + t.Value.ToString() + " | P = " + p.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy", DefaultScopeFunction = true)]
        public static QsValue EnthalpyTD(QsParameter fluid, QsParameter t, QsParameter d)
        {
            return MsgBox(fluid.RawValue + "(T = " + t.Value.ToString() + " | D = " + d.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyTE(QsParameter fluid, QsParameter t, QsParameter e)
        {
            return MsgBox(fluid.RawValue + "(T = " + t.Value.ToString() + " | E = " + e.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyTH(QsParameter fluid, QsParameter t, QsParameter h)
        {
            return MsgBox(fluid.RawValue + "(T = " + t.Value.ToString() + " | H = " + h.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyPD(QsParameter fluid, QsParameter p, QsParameter d)
        {
            return MsgBox(fluid.RawValue + "(P = " + p.Value.ToString() + " | D = " + d.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyPE(QsParameter fluid, QsParameter p, QsParameter e)
        {
            return MsgBox(fluid.RawValue + "(P = " + p.Value.ToString() + " | E = " + e.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyPH(QsParameter fluid, QsParameter p, QsParameter h)
        {
            return MsgBox(fluid.RawValue + "(P = " + p.Value.ToString() + " | H = " + h.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyDE(QsParameter fluid, QsParameter d, QsParameter e)
        {
            return MsgBox(fluid.RawValue + "(D = " + d.Value.ToString() + " | E = " + e.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyDH(QsParameter fluid, QsParameter d, QsParameter h)
        {
            return MsgBox(fluid.RawValue + "(D = " + d.Value.ToString() + " | H = " + h.Value.ToString() + ")");
        }

        [QsFunction("Enthalpy")]
        public static QsValue EnthalpyEH(QsParameter fluid, QsParameter e, QsParameter h)
        {
            return MsgBox(fluid.RawValue + "(E = " + e.Value.ToString() + " | H = " + h.Value.ToString() + ")");
        }

    }
}
