using System;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class License : Form
    {
        Properties.Settings prop = Properties.Settings.Default;

        public License()
        {
            InitializeComponent();
            textBox1.GotFocus += delegate { textBox1.Select(2150, 2150); };
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
         {
             if (keyData == Keys.Tab)
                 return true;
          
             return base.ProcessCmdKey (ref msg, keyData);
         }

        private void License_Load(object sender, EventArgs e)
        {
            Properties.Settings prop = Properties.Settings.Default;
            if (prop.License == 1)
            {
                textBox1.Height = 348;
                seperator1.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            prop.License = 1;
            prop.Save();
            this.Hide();
        }

        private void License_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (prop.License == 0)
            {
                e.Cancel = true;
            }
            if (prop.License == 1)
            {
                e.Cancel = false;
            }
        }

        private void License_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }


    }
}
