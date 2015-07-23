using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class Settings : Form
    {
        string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString();
        Properties.Settings ps = Properties.Settings.Default;

        public class PubVar
        {
            public static bool toggle = false;
            public static string langChange;
        }

        public Settings()
        {
            InitializeComponent();
        }

        public class MyGroupBox : GroupBox
        {
            private Color _borderColor = Color.Black;

            public Color BorderColor
            {
                get
                {
                    return this._borderColor;
                }
                set
                {
                    this._borderColor = value;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                //get the text size in groupbox
                Size tSize = TextRenderer.MeasureText(this.Text, this.Font);

                Rectangle borderRect = e.ClipRectangle;
                borderRect.Y = (borderRect.Y + (tSize.Height / 2));
                borderRect.Height = (borderRect.Height - (tSize.Height / 2));
                ControlPaint.DrawBorder(e.Graphics, borderRect, this._borderColor, ButtonBorderStyle.Solid);

                Rectangle textRect = e.ClipRectangle;
                textRect.X = (textRect.X + 6);
                textRect.Width = tSize.Width;
                textRect.Height = tSize.Height;
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = ps.Capacity;
            trayCheckBox.Checked = ps.Tray;
            fastStart.Checked = ps.Priority;
            textBox3.Text = ps.Extra;
            checkBox1.Checked = ps.AutoUPD;
            textBox2.Text = path;
            textBox1.Text = ps.GamePath;
            ServerComboBox.Text = ps.SCCB;
            comboBox2.SelectedIndex = ps.Monitoring;
            RestartCheckBox.Checked = ps.RestartAlert;
            pingCheckBox.Checked = ps.Ping;

            languageComboBox.DataSource = new CultureInfo[]{
            CultureInfo.GetCultureInfo("ru-RU"),
            CultureInfo.GetCultureInfo("en-US")
                  };

            languageComboBox.DisplayMember = "NativeName";
            languageComboBox.ValueMember = "Name";

            //string language = "";
            //if (ps.Language == "ru-RU")
            //{
            //    language = "Русский";
            //}

            //if (ps.Language == "en-US")
            //{
            //    language = "English";
            //}

            if (!String.IsNullOrEmpty(ps.Language))
            {
                languageComboBox.SelectedValue = ps.Language;
                PubVar.langChange = ps.Language;
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            //string language = "";
            //if (languageComboBox.Text == "Русский")
            //{
            //    language = "ru-RU";
            //}

            //if (languageComboBox.Text == "English")
            //{
            //    language = "en-US";
            //}

            ps.Capacity = comboBox1.SelectedIndex;
            ps.Tray = trayCheckBox.Checked;
            ps.Priority = fastStart.Checked;
            ps.GamePath = textBox1.Text;
            ps.Extra = textBox3.Text;
            ps.AutoUPD = checkBox1.Checked;
            ps.SCCB = ServerComboBox.Text;
            ps.Monitoring = comboBox2.SelectedIndex;
            ps.RestartAlert = RestartCheckBox.Checked;
            ps.Ping = pingCheckBox.Checked;
            ps.Language = languageComboBox.SelectedValue.ToString();
            ps.Save();

            PubVar.toggle = true;

            if (languageComboBox.Text == ("русский (Россия)") & !Directory.Exists("ru-RU"))
            {
                Directory.CreateDirectory("ru-RU");
                File.WriteAllBytes(@".\ru-RU\Aion Game Launcher.resources.dll", Properties.Resources.ru);
                Application.Restart();
            }

            //if (PubVar.langChange != ps.Language)
            //{
            //    DialogResult result = MessageBox.Show(translate.needToRestartText, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //    if (result == DialogResult.Yes)
            //    {
            //        Application.Restart();
            //    }
            //}

            this.Visible = false;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            label2.Focus();
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void ResetSettingsButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(translate.resetSettingsText, "", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (result == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                Settings_Load(this, new EventArgs());  
            }                 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label3.Focus();
            string folder = path.Remove(path.LastIndexOf(@"\user.config"));
            Process.Start(folder);
        }

        private void fastStart_CheckedChanged(object sender, EventArgs e)
        {
            if (fastStart.Checked == true)
            {
                trayCheckBox.Enabled = true;
            }

            if (fastStart.Checked == false)
            {
                trayCheckBox.Enabled = false;
                trayCheckBox.Checked = false;

            }
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void ServerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();            
        }

    }
}
