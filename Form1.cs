using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

namespace Aion_Launcher
{
    public partial class Form1 : Form
    {
        int i;
        Properties.Settings ps = Properties.Settings.Default;
        string[] arg;

        AutoResetEvent resetEvent = new AutoResetEvent(false);

        public int comboindex = 0;

        IniFile ini = new IniFile();

        public Form1(string[] args)
        {
            if (!string.IsNullOrEmpty(ps.Language))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ps.Language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(ps.Language);
            }

            InitializeComponent();

            arg = args;
            emailComboBox.ForeColor = Color.Gray;
            passwordTextBox.ForeColor = Color.Gray;

            if (ps.LangCheck == false)
            {
                LanguageForm l = new LanguageForm();
                l.ShowDialog();
            }

            if (ps.License == 0)
            {
                License l = new License();
                l.ShowDialog();
            }


            statusStrip1.Padding = new Padding(statusStrip1.Padding.Left,
            statusStrip1.Padding.Top, statusStrip1.Padding.Left, statusStrip1.Padding.Bottom);
        }


        #region RemoteFileExists
        private bool RemoteFileExists(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }
        #endregion


        private void PasswordTextBox_Enter(object sender, EventArgs e)
        {
            Font font = new Font(passwordTextBox.Font, FontStyle.Regular);
            passwordTextBox.Font = font;
            passwordTextBox.ForeColor = Color.Black;

            if (passwordTextBox.Text == translate.password)
            {
                passwordTextBox.Clear();
                passwordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void LoginTextBox_Enter(object sender, EventArgs e)
        {
            Font font = new Font(loginTextBox.Font, FontStyle.Regular);
            loginTextBox.Font = font;
            loginTextBox.ForeColor = Color.Black;

            if (loginTextBox.Text == translate.email)
                loginTextBox.Clear();
        }


        #region multiaccounts
        public void multiAccounts()
        {
            if (!File.Exists(ini.Path))
            {
                ps.account = -1;
                ps.Save();

                ini.Write("login0", "1", "Login");
                ini.Write("password0", "", "Password");

                ini.Write("login1", "2", "Login");
                ini.Write("password1", "", "Password");

                ini.Write("login2", "3", "Login");
                ini.Write("password2", "", "Password");

                ini.Write("login3", "4", "Login");
                ini.Write("password3", "", "Password");

                ini.Write("login4", "5", "Login");
                ini.Write("password4", "", "Password");

                ini.Write("login5", "6", "Login");
                ini.Write("password5", "", "Password");

                ini.Write("login6", "7", "Login");
                ini.Write("password6", "", "Password");

                ini.Write("login7", "8", "Login");
                ini.Write("password7", "", "Password");

                ini.Write("login8", "9", "Login");
                ini.Write("password8", "", "Password");

                ini.Write("login9", "10", "Login");
                ini.Write("password9", "", "Password");
            }

            emailComboBox.Items.Add(ini.Read("login0", "Login"));
            emailComboBox.Items.Add(ini.Read("login1", "Login"));
            emailComboBox.Items.Add(ini.Read("login2", "Login"));
            emailComboBox.Items.Add(ini.Read("login3", "Login"));
            emailComboBox.Items.Add(ini.Read("login4", "Login"));
            emailComboBox.Items.Add(ini.Read("login5", "Login"));
            emailComboBox.Items.Add(ini.Read("login6", "Login"));
            emailComboBox.Items.Add(ini.Read("login7", "Login"));
            emailComboBox.Items.Add(ini.Read("login8", "Login"));
            emailComboBox.Items.Add(ini.Read("login9", "Login"));
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (rememberCheckBox.Checked == true)
            {
                if (passwordTextBox.Text != translate.password && emailComboBox.Text != translate.email /*&& passwordTextBox.Text != "" && emailComboBox.Text != ""*/)
                {
                    ini.Write("login" + comboindex, emailComboBox.Text, "Login");

                    ini.Write("password" + comboindex, passwordTextBox.Text, "Password");

                    emailComboBox.Items.Clear();

                    multiAccounts();

                    ps.account = comboindex;
                    ps.Checked = rememberCheckBox.Checked;
                    ps.Save();

                    Thread m = new Thread(multiAccountsThread);
                    m.Start();
                }
            }
            if (rememberCheckBox.Checked == false)
            {
                //ps.Log = "";
                //ps.Pass = "";
                //ps.Checked = rememberCheckBox.Checked;
                //ps.Save();
            }

        }

        #region Проверка соединения
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            multiAccounts();

            CultureInfo cultureInfo = new CultureInfo(ps.Language);
            ChangeLanguage.Instance.localizeForm(this, cultureInfo);

            Thread s = new Thread(ServerStatus);
            s.Start();

            Thread ar = new Thread(autoUpd_restart);
            ar.Start();

            if (CheckForInternetConnection() == true)
            {
                webBrowser1.Navigate(new Uri("http://web-launcher.ncsoft.com/aion/en/installed_hq.php#"));
            }

            if (ps.Ping == true)
            {
                SendPing();
                pingTimer.Enabled = true;
                pingStatusLabel.Visible = true;
            }

            if (ps.Ping == false)
            {
                pingStatusLabel.Visible = false;
            }

            ServerStatusCheck();

            if (ps.account == -1)
            {
                eyeButton.Enabled = false;
                emailComboBox.Tag = translate.email;
                emailComboBox.Text = emailComboBox.Tag.ToString();

                passwordTextBox.Tag = translate.password;
                passwordTextBox.Text = passwordTextBox.Tag.ToString();
            }

            if (ps.Log != "" & ps.Pass != "")
            {
                ps.account = 0;

                ini.Write("login" + ps.account, ps.Log, "Login");
                ini.Write("password" + ps.account, ps.Pass, "Password");

                emailComboBox.Items.Clear();

                multiAccounts();

                ps.Log = "";
                ps.Pass = "";

                ps.Save();
            }

            if (ps.account != -1)
            {
                emailComboBox.SelectedIndex = ps.account;
                passwordTextBox.Text = ini.Read("password" + ps.account, "Password");

                Size size = new Size(22, 19);
                eyeButton.Size = size;
            }

            /* Получить путь к игре */
            if (ps.GamePath == "")
            {
                RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\NCWest\AION");
                if (registryKey1 != null)
                {
                    string path1 = (string)registryKey1.GetValue("BaseDir");
                    ps.GamePath = path1;
                    ps.Save();
                    registryKey1.Close();

                }

                RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\NCWest\AION");
                if (registryKey2 != null)
                {
                    string path2 = (string)registryKey2.GetValue("BaseDir");
                    ps.GamePath = path2;
                    ps.Save();
                    registryKey2.Close();
                }
            }
            /* Получить путь к игре */

            if (emailComboBox.Text == translate.email | passwordTextBox.Text == translate.password | emailComboBox.Text == "" | passwordTextBox.Text == "")
            {
                emailComboBox.Tag = translate.email;
                emailComboBox.Text = emailComboBox.Tag.ToString();

                passwordTextBox.Tag = translate.password;
                passwordTextBox.Text = passwordTextBox.Tag.ToString();


                emailComboBox.ForeColor = Color.Gray;
                passwordTextBox.ForeColor = Color.Gray;
                passwordTextBox.UseSystemPasswordChar = false;
            }

            if (emailComboBox.Text != translate.email | passwordTextBox.Text != translate.password)
            {
                Font font = new Font(emailComboBox.Font, FontStyle.Regular);
                emailComboBox.Font = font;
                passwordTextBox.Font = font;
                emailComboBox.ForeColor = Color.Black;
                passwordTextBox.ForeColor = Color.Black;
            }

            if (arg.Length == 0) /* Аргумент = 0 */
            {
                if (ps.Priority == true)
                {
                    this.WindowState = FormWindowState.Minimized;
                    statusLabel.Visible = false;
                    toolStripDropDownButton1.Visible = true;
                    i = 3;
                    toolStripDropDownButton1.Text = translate.runAfter + i.ToString();
                    PriorityTimer.Interval = 1000;
                    PriorityTimer.Enabled = true;
                    PriorityTimer.Start();
                }
            }

            else if (arg[0] == "upd") /* Аргумент = upd */
            {
                string f = Path.GetDirectoryName(Application.ExecutablePath) + @"\Updater.exe";
                if (File.Exists(f))
                {
                    File.Delete(f);
                }

                Version_Info v = new Version_Info();
                v.ShowDialog();
            }
            emailComboBox.SelectionLength = 0;
        }

        public void ServerStatusCheck()
        {
            toolStripStatusLabel3.Text = ps.SCCB;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }


        /*ПОТОКИ*/

        #region Поток проверки обновлений

        public void UpdateCheck()
        {
            MethodInvoker link = () => statusLabel.IsLink = false;
            statusStrip1.BeginInvoke(link);
            MethodInvoker cl = () => statusLabel.ForeColor = System.Drawing.Color.Red;
            statusStrip1.BeginInvoke(cl);
            MethodInvoker tx = () => statusLabel.Text = translate.updCheck;
            statusStrip1.BeginInvoke(tx);
            Bitmap looa = Properties.Resources.loader;
            MethodInvoker diin = () => statusLabel.Image = looa;
            statusStrip1.BeginInvoke(diin);

            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/Sigmanor/sigmanor.github.io/master/soft/Aion-Game-Launcher/version");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                String version = Application.ProductVersion;

                int con = Convert.ToInt32(content.Replace(".", ""));
                int ver = Convert.ToInt32(version.Replace(".", ""));

                if (con != ver)
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker upimg = () => statusLabel.Image = Properties.Resources.upd;
                        statusStrip1.BeginInvoke(upimg);

                        MethodInvoker ne = () => statusLabel.Text = translate.newVersion;
                        statusStrip1.BeginInvoke(ne);

                        MethodInvoker fo = () => statusLabel.Font = new Font(statusLabel.Text, 8, FontStyle.Regular | FontStyle.Underline);
                        statusStrip1.BeginInvoke(fo);

                        MethodInvoker ho = () => statusLabel.ForeColor = SystemColors.HotTrack;
                        statusStrip1.BeginInvoke(ho);

                        MethodInvoker ha = () => statusLabel.IsLink = true;
                        statusStrip1.BeginInvoke(ha);
                    }
                }
                else
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker tlimg = () => statusLabel.Image = Properties.Resources.noupd;
                        statusStrip1.BeginInvoke(tlimg);

                        MethodInvoker co = () => statusLabel.ForeColor = System.Drawing.Color.SeaGreen;
                        statusStrip1.BeginInvoke(co);

                        MethodInvoker ls = () => statusLabel.Text = translate.noUpd;
                        statusStrip1.BeginInvoke(ls);

                        System.Threading.Thread.Sleep(3000);

                        if (ps.RestartAlert == true) /* Restart Check */
                        {
                            Thread ar = new Thread(autoUpd_restart);
                            ar.Start();
                        }

                        else
                        {
                            MethodInvoker bl = () => statusLabel.ForeColor = System.Drawing.Color.Black;
                            statusStrip1.BeginInvoke(bl);

                            MethodInvoker gocrab = () => statusLabel.Text = translate.ready;
                            statusStrip1.Invoke(gocrab);

                            MethodInvoker gc = () => statusLabel.Image = Properties.Resources.controller;
                            statusStrip1.BeginInvoke(gc);
                        }
                    }
                }

            }

            catch (WebException)
            {
                if (InvokeRequired)
                {
                    Bitmap ufail = Properties.Resources.updfail;
                    MethodInvoker uimg = () => statusLabel.Image = ufail;
                    statusStrip1.BeginInvoke(uimg);
                    MethodInvoker ik = () => statusLabel.ForeColor = System.Drawing.Color.Red;
                    statusStrip1.BeginInvoke(ik);
                    MethodInvoker ls = () => statusLabel.Text = translate.updServerError;
                    statusStrip1.BeginInvoke(ls);

                    System.Threading.Thread.Sleep(3000);

                    if (ps.RestartAlert == true) /* Restart Check */
                    {
                        Thread ar = new Thread(autoUpd_restart);
                        ar.Start();
                    }

                    if (ps.RestartAlert == false)
                    {
                        MethodInvoker bl = () => statusLabel.ForeColor = System.Drawing.Color.Black;
                        statusStrip1.BeginInvoke(bl);
                        MethodInvoker gocrab = () => statusLabel.Text = translate.ready;
                        statusStrip1.Invoke(gocrab);

                        Bitmap crabimg = Properties.Resources.controller;
                        MethodInvoker gc = () => statusLabel.Image = crabimg;
                        statusStrip1.BeginInvoke(gc);
                    }
                }
            }

        }
        #endregion

        #region Поток проверки статуса серверов
        public void ServerStatus()
        {
            HttpWebRequest req;
            HttpWebResponse resp;
            StreamReader sr;
            string C;
            string IS = "", KR = "", SL = "", TM = "", LG = "";
            string site = "";

            try
            {
                if (ps.Monitoring == 0)
                {
                    site = "http://aionstatus.net/";
                }

                if (ps.Monitoring == 1)
                {
                    site = "http://aion.im/status/status.php";
                }

                if (ps.Monitoring == 2)
                {
                    site = "http://aion.mouseclic.com/tool/status/";
                }

                req = (HttpWebRequest)WebRequest.Create(site);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                C = sr.ReadToEnd();
                sr.Close();

                if (ps.Monitoring == 0)
                {
                    IS = "Online</font></td><td><a href=\"fav.php?favorite=Israphel";
                    KR = "Online</font></td><td><a href=\"fav.php?favorite=Kahrun";
                    SL = "Online</font></td><td><a href=\"fav.php?favorite=Siel";
                    TM = "Online</font></td><td><a href=\"fav.php?favorite=Tiamat";
                    LG = "Online</font></td><td><a href=\"fav.php?favorite=NA_Login";
                }

                if (ps.Monitoring == 1)
                {
                    IS = "class=\"lang-na\"></span>Israphel<span class=\"status-1\">";
                    KR = "class=\"lang-na\"></span>Kahrun<span class=\"status-1\">";
                    SL = "class=\"lang-na\"></span>Siel<span class=\"status-1\">";
                    TM = "class=\"lang-na\"></span>Tiamat<span class=\"status-1\">";
                    LG = "class=\"lang-na\"></span>NCSoft Login<span class=\"status-1\">";
                }

                if (ps.Monitoring == 2)
                {
                    IS = "online.png\" /> Israphel";
                    KR = "online.png\" /> Kahrun";
                    SL = "online.png\" /> Siel";
                    TM = "online.png\" /> Tiamat";
                    LG = "online.png\" /> Login(NA)";
                }

                Bitmap on = Properties.Resources.bullet_green;
                Bitmap off = Properties.Resources.bullet_red;
                if (InvokeRequired)
                {
                    if (ps.SCCB == "Israphel")
                    {
                        if (C.IndexOf(IS) > -1)
                        {
                            MethodInvoker online = () => toolStripStatusLabel3.Image = on;
                            statusStrip1.BeginInvoke(online);
                        }
                        else
                        {
                            MethodInvoker offline = () => toolStripStatusLabel3.Image = off;
                            statusStrip1.BeginInvoke(offline);
                        }
                    }


                    if (ps.SCCB == "Kahrun")
                    {
                        if (C.IndexOf(KR) > -1)
                        {
                            MethodInvoker online = () => toolStripStatusLabel3.Image = on;
                            statusStrip1.BeginInvoke(online);
                        }
                        else
                        {
                            MethodInvoker offline = () => toolStripStatusLabel3.Image = off;
                            statusStrip1.BeginInvoke(offline);
                        }
                    }

                    if (ps.SCCB == "Siel")
                    {
                        if (C.IndexOf(SL) > -1)
                        {
                            MethodInvoker online = () => toolStripStatusLabel3.Image = on;
                            statusStrip1.BeginInvoke(online);
                        }
                        else
                        {
                            MethodInvoker offline = () => toolStripStatusLabel3.Image = off;
                            statusStrip1.BeginInvoke(offline);
                        }
                    }

                    if (ps.SCCB == "Tiamat")
                    {
                        if (C.IndexOf(TM) > -1)
                        {
                            MethodInvoker online = () => toolStripStatusLabel3.Image = on;
                            statusStrip1.BeginInvoke(online);
                        }
                        else
                        {
                            MethodInvoker offline = () => toolStripStatusLabel3.Image = off;
                            statusStrip1.BeginInvoke(offline);
                        }
                    }

                    /* NC Login */
                    if (C.IndexOf(LG) > -1)
                    {
                        MethodInvoker online = () => toolStripStatusLabel2.Image = on;
                        statusStrip1.BeginInvoke(online);
                    }
                    else
                    {
                        MethodInvoker offline = () => toolStripStatusLabel2.Image = off;
                        statusStrip1.BeginInvoke(offline);
                    }
                }
            }
            catch (WebException)
            {
                if (InvokeRequired)
                {
                    MethodInvoker err1 = () => toolStripStatusLabel3.Image = Properties.Resources.bullet_black;
                    statusStrip1.BeginInvoke(err1);

                    MethodInvoker err2 = () => toolStripStatusLabel2.Image = Properties.Resources.bullet_black;
                    statusStrip1.BeginInvoke(err2);
                }
            }
        }
        #endregion

        #region Поток авто.обновления/рестарт

        public void autoUpd_restart()
        {
            try
            {

                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/Sigmanor/sigmanor.github.io/master/soft/Aion-Game-Launcher/version");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                String version = Application.ProductVersion;

                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;

                int con = Convert.ToInt32(content.Replace(".", ""));
                int ver = Convert.ToInt32(version.Replace(".", ""));

                string C;
                string RS = "Wednesday";

                req = (HttpWebRequest)WebRequest.Create("http://24timezones.com/usa_time/tx_galveston/texas_city.htm");
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                C = sr.ReadToEnd();
                sr.Close();


                if (ps.RestartAlert == true & C.IndexOf(RS) > -1)
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker islink = () => statusLabel.IsLink = false;
                        statusStrip1.BeginInvoke(islink);

                        MethodInvoker font = () => statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                        statusStrip1.Invoke(font);

                        MethodInvoker color = () => statusLabel.ForeColor = System.Drawing.Color.Black;
                        statusStrip1.BeginInvoke(color);

                        MethodInvoker text = () => statusLabel.Text = translate.serverRestart;
                        statusStrip1.Invoke(text);

                        MethodInvoker image = () => statusLabel.Image = Properties.Resources.construction;
                        statusStrip1.BeginInvoke(image);
                    }
                }
                else
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker islink = () => statusLabel.IsLink = false;
                        statusStrip1.BeginInvoke(islink);

                        MethodInvoker font = () => statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                        statusStrip1.Invoke(font);

                        MethodInvoker color = () => statusLabel.ForeColor = System.Drawing.Color.Black;
                        statusStrip1.BeginInvoke(color);

                        MethodInvoker text = () => statusLabel.Text = translate.ready;
                        statusStrip1.Invoke(text);

                        MethodInvoker image = () => statusLabel.Image = Properties.Resources.controller;
                        statusStrip1.BeginInvoke(image);
                    }
                }


                if (ps.AutoUPD == true & con != ver)
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker image = () => statusLabel.Image = Properties.Resources.upd;
                        statusStrip1.BeginInvoke(image);

                        MethodInvoker text = () => statusLabel.Text = translate.newVersion;
                        statusStrip1.BeginInvoke(text);

                        MethodInvoker font = () => statusLabel.Font = new Font(statusLabel.Text, 8, FontStyle.Regular | FontStyle.Underline);
                        statusStrip1.BeginInvoke(font);

                        MethodInvoker color = () => statusLabel.ForeColor = SystemColors.HotTrack;
                        statusStrip1.BeginInvoke(color);

                        MethodInvoker islink = () => statusLabel.IsLink = true;
                        statusStrip1.BeginInvoke(islink);
                    }
                }


            }

            catch (WebException)
            {
                if (InvokeRequired)
                {
                    MethodInvoker color = () => statusLabel.ForeColor = System.Drawing.Color.Black;
                    statusStrip1.BeginInvoke(color);

                    MethodInvoker text = () => statusLabel.Text = translate.ready;
                    statusStrip1.Invoke(text);

                    MethodInvoker image = () => statusLabel.Image = Properties.Resources.controller;
                    statusStrip1.BeginInvoke(image);
                }
            }

        }

        #endregion

        #region Асинхронная проверка пинга

        private void SendPing()
        {
            System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
            pingSender.PingCompleted += new PingCompletedEventHandler(pingSender_Complete);
            byte[] packetData = Encoding.ASCII.GetBytes("................................");
            PingOptions packetOptions = new PingOptions(50, true);
            pingSender.SendAsync("64.25.35.103", 5000, packetData, packetOptions, resetEvent);
        }

        private void pingSender_Complete(object sender, PingCompletedEventArgs e)
        {
            PingReply pingResponse = e.Reply;
            ShowPingResults(pingResponse);
        }

        public void ShowPingResults(PingReply pingResponse)
        {
            pingStatusLabel.Text = translate.pingString + pingResponse.RoundtripTime.ToString() + translate.msString;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //if (CheckForInternetConnection() == true)
            //{
            //    try
            //    {
            //        SendPing();
            //        //pingStatusLabel.Text = pingStringFix + new Ping().Send("64.25.35.103").RoundtripTime.ToString() + msStringFix;
            //    }
            //    catch (PingException)
            //    {
            //        return;
            //    }
            //    catch (System.ComponentModel.Win32Exception)
            //    {
            //        return;
            //    }
            //}
        }

        #endregion

        #region Мультиаккаунты

        private void multiAccountsThread()
        {
            MethodInvoker islink = () => statusLabel.IsLink = false;
            statusStrip1.BeginInvoke(islink);
            MethodInvoker font = () => statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            statusStrip1.Invoke(font);
            MethodInvoker color = () => statusLabel.ForeColor = Color.Black;
            statusStrip1.BeginInvoke(color);
            MethodInvoker text = () => statusLabel.Text = translate.savingText;
            statusStrip1.Invoke(text);
            MethodInvoker image = () => statusLabel.Image = Properties.Resources.save;
            statusStrip1.BeginInvoke(image);

            Thread.Sleep(2000);

            Thread ar = new Thread(autoUpd_restart);
            ar.Start();

            MethodInvoker cb = () => rememberCheckBox.Checked = false;
            statusStrip1.BeginInvoke(cb);
        }

        #endregion


        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings Settings = new Settings();
            Settings.ShowDialog();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
            this.Opacity = 100;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PriorityTimer_Tick(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = translate.runAfter + (--i).ToString();
            if (i < 0)
            {
                PriorityTimer.Stop();
                playButton_Click(this, new EventArgs());
                statusLabel.Visible = true;
                toolStripDropDownButton1.Visible = false;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (Aion_Launcher.Settings.PubVar.toggle == true)
            {
                if (Aion_Launcher.Settings.PubVar.langChange != ps.Language)
                {
                    CultureInfo cultureInfo = new CultureInfo(ps.Language);
                    ChangeLanguage.Instance.localizeForm(this, cultureInfo);
                    try
                    {
                        SendPing();
                    }
                    catch
                    {
                    }
                    //if (ps.Log == "" & ps.Pass == "")
                    if (ps.account == -1)
                    {
                        emailComboBox.Tag = translate.email;
                        emailComboBox.Text = emailComboBox.Tag.ToString();

                        passwordTextBox.Tag = translate.password;
                        passwordTextBox.Text = passwordTextBox.Tag.ToString();
                    }
                    this.Refresh();
                }

                if (CheckForInternetConnection() == true)
                {
                    Thread ar = new Thread(autoUpd_restart);
                    ar.Start();
                }

                ServerStatusCheck();
                Thread s = new Thread(ServerStatus);
                s.Start();

                if (ps.Priority == false)
                {
                    PriorityTimer.Stop();
                    statusLabel.Visible = true;
                    toolStripDropDownButton1.Visible = false;
                }

                if (ps.Ping == true)
                {
                    try
                    {
                        SendPing();
                        pingTimer.Enabled = true;
                        pingStatusLabel.Visible = true;
                    }
                    catch
                    {
                    }
                }

                if (ps.Ping == false)
                {
                    pingTimer.Enabled = false;
                    pingStatusLabel.Visible = false;
                }

            }

            Aion_Launcher.Settings.PubVar.toggle = false;
        }

        private void AutoUPDtimer_Tick(object sender, EventArgs e)
        {
            Thread s = new Thread(ServerStatus);
            s.Start();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (PriorityTimer.Enabled == true)
            {
                PriorityTimer.Stop();

                Bitmap t = Properties.Resources.timerplay;
                toolStripDropDownButton1.Image = t;
            }

            else if (PriorityTimer.Enabled == false)
            {
                PriorityTimer.Start();

                Bitmap t = Properties.Resources.timerstop;
                toolStripDropDownButton1.Image = t;
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();

            if (statusLabel.Text == translate.newVersion)
            {
                DialogResult result = MessageBox.Show(translate.updMsgBoxText, translate.updMsgBoxTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result == DialogResult.OK)
                {

                    if (RemoteFileExists("https://github.com/Sigmanor/sigmanor.github.io/tree/master/soft/Aion-Game-Launcher/AionGameLauncher.exe"))
                    {
                        Bitmap la = Properties.Resources.load_anim;
                        statusLabel.Image = la;
                        statusLabel.Text = translate.processUpdate;
                        statusLabel.IsLink = false;
                        statusLabel.Font = new Font(statusLabel.Text, 8, FontStyle.Regular);
                        statusLabel.ForeColor = Color.Black;

                        webClient.DownloadFileCompleted += (s, bg) =>
                        {
                            string N = "agl_update.bat";
                            using (StreamWriter sw = new StreamWriter(N))
                            {
                                sw.WriteLine(":first");
                                sw.WriteLine("del \"Aion Game Launcher.exe\"");
                                sw.WriteLine("if exist \"Aion Game Launcher.exe\" goto :first");
                                sw.WriteLine("rename \"aiongamelauncher.update\" \"Aion Game Launcher.exe\"");
                                sw.WriteLine("start \"\" \"Aion Game Launcher.exe\" upd");
                                sw.WriteLine("del /q /f \"%~f0\" >nul 2>&1 & exit /b 0");
                                sw.Close();
                            }

                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            startInfo.FileName = N;

                            Process.Start(startInfo);

                            Application.Exit();
                        };

                        webClient.DownloadFileAsync(new Uri("https://github.com/Sigmanor/sigmanor.github.io/tree/master/soft/Aion-Game-Launcher/AionGameLauncher.exe"), "aiongamelauncher.update");
                    }

                    else
                    {
                        MessageBox.Show(translate.updFailedText, translate.updFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void gButton2_Click(object sender, EventArgs e)
        {
            Process.Start("http://vk.com/aion_community");
        }

        private void gButton1_Click(object sender, EventArgs e)
        {
            Process.Start("http://na.aiononline.com/");
        }

        private void gButton3_Click(object sender, EventArgs e)
        {
            Process.Start("http://aion.im");

        }

        private void gButton4_Click(object sender, EventArgs e)
        {
            Process.Start("https://docs.google.com/spreadsheet/ccc?key=0AtDAFcPW1M8fdGc2UWJUVHJpelNhZlVncXdhNnlnQnc&usp=drive_web#gid=115");
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            string sd = "/" + System.Environment.SystemDirectory.Substring(0, 1) + " ";
            string r = ps.GamePath;
            r = r.Replace(":\\", ":\\\"");
            string bin = "";

            if (passwordTextBox.Text == translate.password | emailComboBox.Text == translate.email)
            {
                if (ps.Capacity == 0)
                {
                    bin = "bin32";
                }

                if (ps.Capacity == 1)
                {
                    bin = "bin64";
                }
                passwordTextBox.Text = "";
                emailComboBox.Text = "";
            }

            else if (passwordTextBox.Text != translate.password | emailComboBox.Text != translate.email)
            {
                if (ps.Capacity == 0)
                {
                    bin = "bin32";
                }

                if (ps.Capacity == 1)
                {
                    bin = "bin64";
                }
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = sd + " start " + r + "\\" + bin + "\\AION.bin\" -ip:64.25.35.103 -port:2106 -cc:1 -noauthgg -charnamemenu -lbox -f2p -loginex -pwd16 -nosatab -lang:enu -account:" + emailComboBox.Text + " -password:" + passwordTextBox.Text + " " + ps.Extra
            };
            Process.Start(startInfo);

            if (ps.Tray == false)
            {
                Application.Exit();
            }

            if (ps.Tray == true)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(30000);
            }
        }

        private void обновленияToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Thread u = new Thread(UpdateCheck);
            u.Start();
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            About A = new About();
            A.ShowDialog();
        }

        private void документацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://sigmanor.tk/aion-game-launcher/manual");
        }

        private void официальныйЛаунчерToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string p = "";

            if (File.Exists(@"C:\Program Files (x86)\NCWest\NCLauncher\NCLauncher.exe"))
            {
                p = " (x86)";
            }

            if (File.Exists(@"C:\Program Files\NCWest\NCLauncher\NCLauncher.exe"))
            {
                p = "";
            }

            if ((File.Exists(@"C:\Program Files\NCWest\NCLauncher\NCLauncher.exe")) || (File.Exists(@"C:\Program Files (x86)\NCWest\NCLauncher\NCLauncher.exe")))
            {
                Process pr = new Process();
                pr.StartInfo.FileName = @"C:\Program Files" + p + @"\NCWest\NCLauncher\NCLauncher.exe";
                pr.StartInfo.Arguments = @"/LauncherID:""NCWest"" /CompanyID:""12"" /GameID:""AION"" /LUpdateAddr:""updater.nclauncher.ncsoft.com""";
                pr.Start();
            }

            if ((!File.Exists(@"C:\Program Files\NCWest\NCLauncher\NCLauncher.exe")) && (!File.Exists(@"C:\Program Files (x86)\NCWest\NCLauncher\NCLauncher.exe")))
            {
                MessageBox.Show("The launcher is not installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void WebBrowser1NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Process.Start(webBrowser1.StatusText);
        }

        void WebBrowser1DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.DocumentText.Contains("Переход на веб-страницу отменен"))
            {
                pictureBox1.Visible = true;
            }

            if (webBrowser1.DocumentText.Contains("Account"))
            {

                if (webBrowser1.Url.ToString().IndexOf("http://web-launcher.ncsoft.com/aion/en/installed_hq.php#") == 0)
                {
                    webBrowser1.Visible = true;
                    pictureBox1.Visible = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendPing();
        }

        private void eyeButton_Paint(object sender, PaintEventArgs e)
        {
            Graphics objGraphics = null;
            objGraphics = this.CreateGraphics();
            objGraphics.Clear(SystemColors.Control);
            objGraphics.DrawRectangle(Pens.DarkGray,
                 eyeButton.Left - 0, eyeButton.Top - 1,
                  eyeButton.Width + 0, eyeButton.Height + 1);
            objGraphics.Dispose();
        }

        private void eyeButton_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void eyeButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (passwordTextBox.UseSystemPasswordChar == false)
            {
                passwordTextBox.UseSystemPasswordChar = true;
                eyeButton.BackColor = Color.White;
            }

            else
            {
                passwordTextBox.UseSystemPasswordChar = false;
                eyeButton.BackColor = Color.LightGray;
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            if (passwordTextBox.Text != translate.password)
            {
                eyeButton.Enabled = true;
                passwordTextBox.UseSystemPasswordChar = true;
                eyeButton.BackColor = Color.White;


            }



            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                eyeButton.BackColor = Color.White;
                eyeButton.Enabled = false;
                passwordTextBox.UseSystemPasswordChar = true;

                Graphics objGraphics = null;
                objGraphics = this.CreateGraphics();
                objGraphics.Clear(SystemColors.Control);
                objGraphics.DrawRectangle(Pens.DarkGray,
                     eyeButton.Left - 0, eyeButton.Top - 1,
                      eyeButton.Width + 0, eyeButton.Height + 1);
                objGraphics.Dispose();
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboindex = emailComboBox.SelectedIndex;
            passwordTextBox.Text = ini.Read("password" + comboindex, "Password");
            passwordTextBox.ForeColor = Color.Black;
            rememberCheckBox.Checked = false;

            if (passwordTextBox.Text != translate.password && emailComboBox.Text != translate.email && passwordTextBox.Text != "" && emailComboBox.Text != "")
            {
                ps.account = comboindex;
                ps.Save();
            }

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void emailComboBox_Enter(object sender, EventArgs e)
        {
            Font font = new Font(emailComboBox.Font, FontStyle.Regular);
            emailComboBox.Font = font;
            emailComboBox.ForeColor = Color.Black;

            if (emailComboBox.Text == translate.email)
            {
                emailComboBox.Text = "";
            }
        }

        private void emailComboBox_DropDown(object sender, EventArgs e)
        {
            if (emailComboBox.Text == translate.email)
            {
                emailComboBox.Text = "";
            }

        }

        private void emailComboBox_MouseEnter(object sender, EventArgs e)
        {
            if (emailComboBox.Text.Length > 24)
            {
                toolTip1.Show(emailComboBox.Text, emailComboBox);
            }
        }

        private void emailComboBox_Leave(object sender, EventArgs e)
        {
            toolTip1.Hide(emailComboBox);
        }
    }
}

