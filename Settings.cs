using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;

namespace Aion_Launcher
{
    public partial class Settings : Form
    {
        string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString();
        Properties.Settings ps = Properties.Settings.Default;

        public class PubVar
        {
            public static bool toggle = false;
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //Properties.Settings ps = Properties.Settings.Default;
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
        }

        //private void CloseButton_Click(object sender, EventArgs e)
        //{
        //    this.Visible=false;
        //}

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            //Properties.Settings ps = Properties.Settings.Default;
            ps.Capacity = comboBox1.SelectedIndex;
            ps.Tray = trayCheckBox.Checked;
            ps.Priority = fastStart.Checked;
            ps.GamePath = textBox1.Text;
            ps.Extra = textBox3.Text;
            ps.AutoUPD = checkBox1.Checked;
            ps.SCCB = ServerComboBox.Text;
            ps.Monitoring = comboBox2.SelectedIndex;
            ps.RestartAlert = RestartCheckBox.Checked;
            ps.Save();

            PubVar.toggle = true;

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
            DialogResult result = MessageBox.Show("Сбросить настройки программы?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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


    }
}
