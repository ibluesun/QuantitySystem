﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;
using Qs.RuntimeTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Qs.Modules
{
    /// <summary>
    /// Qs Module as namespace.
    /// </summary>
    public static class Windows
    {
        public static QsValue MessageBox(QsParameter text)
        {
            DialogResult d =
                System.Windows.Forms.MessageBox.Show(
                    ForegroundWindow.Instance, text.RawValue, 
                    "Quantity System"
                );

            return ((int)d).ToScalarValue();
        }

        public static QsValue MessageBox(QsParameter text, QsParameter caption)
        {
            DialogResult d =
                System.Windows.Forms.MessageBox.Show(
                    ForegroundWindow.Instance, text.RawValue, 
                    caption.RawValue
                );

            return ((int)d).ToScalarValue();
        }
    }



    /// <summary>
    /// to get the console window
    /// </summary>
    class ForegroundWindow : IWin32Window
    {
        private static ForegroundWindow _window = new ForegroundWindow();
        private ForegroundWindow() { }

        public static IWin32Window Instance
        {
            get { return _window; }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        IntPtr IWin32Window.Handle
        {
            get
            {
                return GetForegroundWindow();
                
            }
        }
    }
}
