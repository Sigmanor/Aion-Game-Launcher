using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
        	Version vrs = new Version(Application.ProductVersion);
            VersionLabel.Text = string.Format("{0}.{1}", vrs.Major, vrs.Minor/*, vrs.Build*/);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.fatcow.com/free-icons");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("http://sigmanor.pp.ua/aion-game-launcher/");         
        }

        private void label6_Click(object sender, EventArgs e)
        {
            LicenseForm l = new LicenseForm();
            l.ShowDialog();
        }
	     
        void PictureBox1Click(object sender, EventArgs e)
        {
			if (timer1.Enabled) {
			  timer1.Stop();
			  pictureBox1.Image = Aion_Launcher.Properties.Resources.ai_eye2;
			} else {
			  timer1.Start();
			}	
        }
        
        void Timer1Tick(object sender, EventArgs e)
        {
        	Image img = pictureBox1.Image;
			img.RotateFlip(RotateFlipType.Rotate90FlipNone);
			pictureBox1.Image = img;
        }

        private void About_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            VersionForm v = new VersionForm();
            v.ShowDialog();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Process.Start("http://wyday.com/vistamenu/");
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Sigmanor/Aion-Game-Launcher");
        }

        private void label15_Click(object sender, EventArgs e)
        {    
            Process.Start("http://www.codeproject.com/KB/vb/CustomSettingsProvider.aspx");
        }
    }
}
