using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qs.Runtime;
using Qs.Types;
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

            IWin32Window fw = null;
            try { fw = ForegroundWindow.Instance; }
            catch { }

            DialogResult d =
                System.Windows.Forms.MessageBox.Show(
                fw,
                text.UnknownValueText,
                    "Quantity System"
                );

            return ((int)d).ToScalarValue();
        }

        public static QsValue MessageBox(QsParameter text, [QsParamInfo(QsParamType.Raw)]QsParameter caption)
        {
            IWin32Window fw = null;
            try { fw = ForegroundWindow.Instance; }
            catch { }

            DialogResult d =
                System.Windows.Forms.MessageBox.Show(
                fw,
                text.UnknownValueText,
                caption.UnknownValueText
                );

            return ((int)d).ToScalarValue();
        }

        public static QsValue Beep()
        {
            System.Media.SystemSounds.Beep.Play();
            return null;
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
