using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class LanguageForm : Form
    {
        Properties.Settings ps = Properties.Settings.Default;

        public LanguageForm()
        {
            InitializeComponent();
        }

        private void Language_Load(object sender, EventArgs e)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (ci.Name == "ru-RU")
            {
                this.Text = "Язык Лаунчера";
                nextButton.Text = "Далее";
            }

            langComboBox.DataSource = new CultureInfo[]{
            CultureInfo.GetCultureInfo("ru-RU"),
            CultureInfo.GetCultureInfo("en-US")
                  };

            langComboBox.DisplayMember = "NativeName";
            langComboBox.ValueMember = "Name";

            if (!String.IsNullOrEmpty(ps.Language) && ci.Name != "ru-RU")
            {
                langComboBox.SelectedValue = ps.Language;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (langComboBox.Text == ("русский (Россия)") & !Directory.Exists("ru-RU"))
            {
                Directory.CreateDirectory("ru-RU");
                File.WriteAllBytes(@".\ru-RU\Aion Game Launcher.resources.dll", Properties.Resources.ru);
            }

            ps.LangCheck = true;
            ps.Language = langComboBox.SelectedValue.ToString();
            ps.Save();
            CultureInfo cultureInfo = new CultureInfo(ps.Language);
            ChangeLanguage.Instance.localizeForm(this, cultureInfo);
            this.Close();
        }

        private void LanguageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ps.LangCheck == false)
            {
                e.Cancel = true;
            }
            if (ps.LangCheck == true)
            {
                e.Cancel = false;
            }
        }   

    }
}
