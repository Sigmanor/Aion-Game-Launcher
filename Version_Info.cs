using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class Version_Info : Form
    {

        public Version_Info()
        {
            InitializeComponent();
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Process.Start(webBrowser1.StatusText); 
        }

        private void Version_Info_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = Properties.Resources.history;
        }


    }
}
