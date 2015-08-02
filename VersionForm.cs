using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class VersionForm : Form
    {
        Properties.Settings ps = Properties.Settings.Default;
        public VersionForm()
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
            if (ps.Language == "ru-RU")
            {

                webBrowser1.DocumentText = Properties.Resources.history;
            }
            else
            {
                webBrowser1.DocumentText = Properties.Resources.historyen;
            }
        }


    }
}
