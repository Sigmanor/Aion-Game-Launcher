using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
        	Version vrs = new Version(Application.ProductVersion);
            VersionLabel.Text = string.Format("v{0}.{1}", vrs.Major, vrs.Minor/*, vrs.Build*/);
        }

        private void VersionLabel_Click(object sender, EventArgs e)
        {
            Version_Info v = new Version_Info();
            v.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.visualstudio.com/products/visual-studio-community-vs");
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.fatcow.com/free-icons");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("http://sigmanor.tk/aion-game-launcher/");         
        }

        private void label6_Click(object sender, EventArgs e)
        {
            License l = new License();
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
    }
}
