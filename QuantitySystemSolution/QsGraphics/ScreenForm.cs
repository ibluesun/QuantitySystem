using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QsGraphics
{
    public partial class ScreenForm : Form
    {
        public ScreenForm()
        {
            InitializeComponent();
        }


        private Graphics _FormGraphics;

        private void ScreenForm_Load(object sender, EventArgs e)
        {
            _FormGraphics = CreateGraphics();
        }

        public Graphics FormGraphics
        {
            get
            {
                return _FormGraphics;
            }
        }
    }
}
