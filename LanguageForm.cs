using System;
using System.Globalization;
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

            if (!string.IsNullOrEmpty(ps.Language) && ci.Name != "ru-RU")
            {
                langComboBox.SelectedValue = ps.Language;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ps.LangCheck = true;
            ps.Language = langComboBox.SelectedValue.ToString();
            ps.Save();
            CultureInfo cultureInfo = new CultureInfo(ps.Language);
            ChangeLanguage.Instance.localizeForm(this, cultureInfo);
            this.Close();
        }

    }
}
