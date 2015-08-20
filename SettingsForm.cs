using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class SettingsForm : Form
    {
        Properties.Settings ps = Properties.Settings.Default;

        PortableSettingsProvider psp = new PortableSettingsProvider();

        public class PubVar
        {
            public static bool toggle = false;
            public static string langChange;
        }

        public SettingsForm()
        {
            InitializeComponent();

            comboBox1.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            languageComboBox.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            ServerComboBox.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            comboBox2.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            comboBox3.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
        }

        void comboBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = ps.Capacity;
            comboBox3.SelectedIndex = ps.AfterStart;
            fastStart.Checked = ps.Priority;
            textBox3.Text = ps.Extra;
            checkBox1.Checked = ps.AutoUPD;
            textBox2.Text = Environment.CurrentDirectory + @"\" + psp.GetAppSettingsFilename();
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

            if (!string.IsNullOrEmpty(ps.Language))
            {
                languageComboBox.SelectedValue = ps.Language;
                PubVar.langChange = ps.Language;
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            ps.Capacity = comboBox1.SelectedIndex;
            ps.AfterStart = comboBox3.SelectedIndex;
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
                File.Delete(psp.GetAppSettingsFilename());
                Application.Restart();
            }                 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                label3.Focus();
                string folder = Environment.CurrentDirectory + @"\"+ psp.GetAppSettingsFilename();
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", folder));
            }
            catch { }       
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Focus();
        }

    }
}
