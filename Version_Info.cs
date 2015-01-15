using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
