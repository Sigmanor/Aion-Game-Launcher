using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Aion_Launcher
{
    public partial class MainForm : Form
    {
        int timerCount;

        Properties.Settings ps = Properties.Settings.Default;

        string[] arg;

        public int comboindex = 0;

        public int updStatus;

        IniFile ini = new IniFile();

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(ps.Language))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(ps.Language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(ps.Language);
            }

            arg = args;
            emailComboBox.ForeColor = Color.Gray;
            passwordTextBox.ForeColor = Color.Gray;

            if (ps.LangCheck == false)
            {
                new LanguageForm().ShowDialog();
            }

            if (ps.License == 0)
            {
                new LicenseForm().ShowDialog();
            }
            statusStrip1.Padding = new Padding(statusStrip1.Padding.Left,
            statusStrip1.Padding.Top, statusStrip1.Padding.Left, statusStrip1.Padding.Bottom);
        }

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


        #region multiAccounts
        public void multiAccounts()
        {
            if (!File.Exists(ini.Path))
            {
                ps.account = -1;
                ps.accountCount = 0;
                ps.Save();
            }

            int g = -1;

            while (g != ps.accountCount)
            {
                g++;
                emailComboBox.Items.Add(ini.Read("login" + g, "Login"));
            }
        }
        #endregion


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (rememberCheckBox.Checked == true)
            {
                if (passwordTextBox.Text != translate.password && emailComboBox.Text != translate.email /*&& passwordTextBox.Text != "" && emailComboBox.Text != ""*/)
                {
                    if (!ini.KeyExists("login" + comboindex.ToString(), "Login") && !string.IsNullOrEmpty(emailComboBox.Text))
                    {
                        ps.accountCount = emailComboBox.Items.Count;
                        ps.Save();
                    }
                    ini.Write("login" + comboindex, emailComboBox.Text, "Login");
                    ini.Write("password" + comboindex, passwordTextBox.Text, "Password");

                    emailComboBox.Items.Clear();

                    multiAccounts();

                    ps.account = comboindex;
                    rememberCheckBox.Enabled = false;
                    ps.Save();

                    Thread m = new Thread(multiAccountsThread);
                    m.Start();
                }
            }
        }

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
                SendPing("64.25.35.103");
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
                RegistryKey registryKey64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\NCWest\AION");
                if (registryKey64 != null)
                {
                    string path64 = (string)registryKey64.GetValue("BaseDir");
                    ps.GamePath = path64;
                    ps.Save();
                    registryKey64.Close();
                }

                RegistryKey registryKey32 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\NCWest\AION");
                if (registryKey32 != null)
                {
                    string path32 = (string)registryKey32.GetValue("BaseDir");
                    ps.GamePath = path32;
                    ps.Save();
                    registryKey32.Close();
                }
            }/* Получить путь к игре */


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

            /* args */        

            if (arg.Length == 0) /* arg == 0 */
            {
                if (ps.Priority == true)
                {
                    this.WindowState = FormWindowState.Minimized;
                    statusLabel.Visible = false;
                    toolStripDropDownButton1.Visible = true;
                    timerCount = 3;
                    toolStripDropDownButton1.Text = translate.runAfter + timerCount.ToString();
                    PriorityTimer.Interval = 1000;
                    PriorityTimer.Enabled = true;
                    PriorityTimer.Start();
                }
            }

            else if (arg[0] == "/u") /* arg == update */
            {
                DeleteUpdateFiles();
                new VersionForm().ShowDialog();
            }

            else if (arg[0] == "/e") /* arg == error */
            {
                DeleteUpdateFiles();
                MessageBox.Show(translate.updFailedText, translate.updFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteUpdateFiles()
        {
            string UpdaterExe = Path.GetDirectoryName(Application.ExecutablePath) + @".\Updater.exe";
            string UpdateZip = Path.GetDirectoryName(Application.ExecutablePath) + @".\Aion-Game-Launcher-Update.zip";

            if (File.Exists(UpdaterExe))
            {
                File.Delete(UpdaterExe);
            }

            if (File.Exists(UpdateZip))
            {
                File.Delete(UpdateZip);
            }

            /*******************************
             FOR UPDATE FROM 2.5 TO 2.6 VER
            *******************************/
            DirectoryInfo parent = Directory.GetParent
            (Path.GetDirectoryName(Application.ExecutablePath));
            string UpdTo26 = parent.FullName + @"\Updater.exe";
            if (File.Exists(UpdTo26)) { File.Delete(UpdTo26); }
            /*******************************
             FOR UPDATE FROM 2.5 TO 2.6 VER
            *******************************/
        }

        public void ServerStatusCheck()
        {
            toolStripStatusLabel3.Text = ps.SCCB;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            statusStrip1.Focus();
        }


        /*THREADS*/

        #region Поток проверки обновлений

        public void UpdateCheck()
        {
            statusStrip1.Invoke((Action)delegate
            {
                statusLabel.IsLink = false;
                statusLabel.ForeColor = Color.DarkViolet;
                statusLabel.Text = translate.updCheck;
                statusLabel.Image = Properties.Resources.loader;
            });
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/Sigmanor/sigmanor.github.io/master/soft/Aion-Game-Launcher/version");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                string version = Application.ProductVersion;

                int ver = Convert.ToInt32(version.Replace(".", "")), con = Convert.ToInt32(content.Replace(".", ""));

                if (con != ver)
                {
                    statusStrip1.Invoke((Action)delegate
                    {
                        statusLabel.Image = Properties.Resources.upd;
                        statusLabel.Text = translate.newVersion;
                        statusLabel.Font = new Font(statusLabel.Text, 8, FontStyle.Regular | FontStyle.Underline);
                        statusLabel.ForeColor = SystemColors.HotTrack;
                        statusLabel.IsLink = true;
                    });
                }

                else
                {
                    statusStrip1.Invoke((Action)delegate
                    {
                        statusLabel.Image = Properties.Resources.noupd;
                        statusLabel.ForeColor = Color.SeaGreen;
                        statusLabel.Text = translate.noUpd;
                    });

                    Thread.Sleep(3000);

                    if (ps.RestartAlert == true) /* Restart Check */
                    {
                        Thread ar = new Thread(autoUpd_restart);
                        ar.Start();

                    }
                    else
                    {
                        statusStrip1.Invoke((Action)delegate
                        {
                            statusLabel.ForeColor = Color.Black;
                            statusLabel.Text = translate.ready;
                            statusLabel.Image = Properties.Resources.controller;
                        });
                    }
                }
            }
            catch (WebException)
            {
                statusStrip1.Invoke((Action)delegate
                {
                    statusLabel.Image = Properties.Resources.updfail;
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.Text = translate.updServerError;
                });

                Thread.Sleep(3000);

                if (ps.RestartAlert == true) /* Restart Check */
                {
                    Thread ar = new Thread(autoUpd_restart);
                    ar.Start();
                }

                if (ps.RestartAlert == false)
                {
                    statusStrip1.Invoke((Action)delegate
                    {
                        statusLabel.ForeColor = Color.Black;
                        statusLabel.Text = translate.ready;
                        statusLabel.Image = Properties.Resources.controller;
                    });
                }

                statusStrip1.Invoke((Action)delegate
                {
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.Text = translate.ready;
                    statusLabel.Image = Properties.Resources.controller;
                });

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
            string server = "";

            try
            {
                switch (ps.Monitoring)
                {
                    case 0:
                        site = "http://aionstatus.net/";
                        IS = "Online</font></td><td><a href=\"fav.php?favorite=Israphel";
                        KR = "Online</font></td><td><a href=\"fav.php?favorite=Kahrun";
                        SL = "Online</font></td><td><a href=\"fav.php?favorite=Siel";
                        TM = "Online</font></td><td><a href=\"fav.php?favorite=Tiamat";
                        LG = "Online</font></td><td><a href=\"fav.php?favorite=NA_Login";
                        break;

                    case 1:
                        site = "http://aion.im/status/status.php";
                        IS = "class=\"lang-na\"></span>Israphel<span class=\"status-1\">";
                        KR = "class=\"lang-na\"></span>Kahrun<span class=\"status-1\">";
                        SL = "class=\"lang-na\"></span>Siel<span class=\"status-1\">";
                        TM = "class=\"lang-na\"></span>Tiamat<span class=\"status-1\">";
                        LG = "class=\"lang-na\"></span>NCSoft Login<span class=\"status-1\">";
                        break;

                    case 2:
                        site = "http://aion.mouseclic.com/tool/status/";
                        IS = "online.png\" /> Israphel";
                        KR = "online.png\" /> Kahrun";
                        SL = "online.png\" /> Siel";
                        TM = "online.png\" /> Tiamat";
                        LG = "online.png\" /> Login(NA)";
                        break;
                }


                switch (ps.SCCB)
                {
                    case "Israphel":
                        server = IS;
                        break;
                    case "Kahrun":
                        server = KR;
                        break;
                    case "Siel":
                        server = SL;
                        break;
                    case "Tiamat":
                        server = TM;
                        break;
                }

                req = (HttpWebRequest)WebRequest.Create(site);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                C = sr.ReadToEnd();
                sr.Close();

                Bitmap on = Properties.Resources.bullet_green;
                Bitmap off = Properties.Resources.bullet_red;

                /* Server */
                if (C.IndexOf(server) > -1)
                {
                    MethodInvoker online = () => toolStripStatusLabel3.Image = on;
                    statusStrip1.BeginInvoke(online);
                }
                else
                {
                    MethodInvoker offline = () => toolStripStatusLabel3.Image = off;
                    statusStrip1.BeginInvoke(offline);
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
            catch (WebException)
            {
                MessageBox.Show("WebException");
                statusStrip1.Invoke((Action)delegate
                {
                    toolStripStatusLabel2.Image = Properties.Resources.bullet_black;
                    toolStripStatusLabel3.Image = Properties.Resources.bullet_black;
                });
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
                HttpWebRequest req;
                HttpWebResponse resp;
                StreamReader sr;

                string content = reader.ReadToEnd();
                string version = Application.ProductVersion;
                string RS = "Wednesday";
                string C;

                int ver = Convert.ToInt32(version.Replace(".", "")), con = Convert.ToInt32(content.Replace(".", ""));

                req = (HttpWebRequest)WebRequest.Create("http://24timezones.com/usa_time/tx_galveston/texas_city.htm");
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                C = sr.ReadToEnd();
                sr.Close();


                if (ps.RestartAlert == true & C.IndexOf(RS) > -1)
                {
                    statusStrip1.Invoke((Action)delegate
                     {
                         statusLabel.IsLink = false;
                         statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                         statusLabel.ForeColor = Color.Black;
                         statusLabel.Text = translate.serverRestart;
                         statusLabel.Image = Properties.Resources.construction;
                     });
                }
                else
                {
                    statusStrip1.Invoke((Action)delegate
                    {
                        statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                        statusLabel.IsLink = false;
                        statusLabel.ForeColor = Color.Black;
                        statusLabel.Text = translate.ready;
                        statusLabel.Image = Properties.Resources.controller;
                    });
                }

                if (ps.AutoUPD == true & con != ver)
                {
                    statusStrip1.Invoke((Action)delegate
                    {
                        statusLabel.Image = Properties.Resources.upd;
                        statusLabel.Text = translate.newVersion;
                        statusLabel.Font = new Font(statusLabel.Text, 8, FontStyle.Regular | FontStyle.Underline);
                        statusLabel.ForeColor = SystemColors.HotTrack;
                        statusLabel.IsLink = true;
                    });
                }
            }

            catch (WebException)
            {
                statusStrip1.Invoke((Action)delegate
                {
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.Text = translate.ready;
                    statusLabel.Image = Properties.Resources.controller;
                });
            }
        }

        #endregion

        #region Мультиаккаунты

        private void multiAccountsThread()
        {
            try
            {
                statusStrip1.Invoke((Action)delegate
                {
                    statusLabel.IsLink = false;
                    statusLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                    statusLabel.ForeColor = Color.Black;
                    statusLabel.Text = translate.savingText;
                    statusLabel.Image = Properties.Resources.save;
                });

                Thread.Sleep(1500);

                Thread ar = new Thread(autoUpd_restart);
                ar.Start();

                statusStrip1.Invoke((Action)delegate
                {
                    rememberCheckBox.Checked = false;
                    rememberCheckBox.Enabled = true;
                });
            }
            catch
            {

            }
        }

        #endregion

        /*THREADS*/

        #region Асинхронная проверка пинга

        private void SendPing(string IP)
        {
            //AutoResetEvent resetEvent = new AutoResetEvent(false);
            try
            {
                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                pingSender.PingCompleted += new PingCompletedEventHandler(pingSender_Complete);
                byte[] packetData = Encoding.ASCII.GetBytes("................................");
                PingOptions packetOptions = new PingOptions(50, true);
                pingSender.SendAsync(IP, 5000, packetData, packetOptions, new AutoResetEvent(false));
            }
            catch { }

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

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            notifyIcon1.Visible = false;
            this.Opacity = 100;
        }

        private void PriorityTimer_Tick(object sender, EventArgs e)
        {
            toolStripDropDownButton1.Text = translate.runAfter + (--timerCount).ToString();
            if (timerCount < 0)
            {
                PriorityTimer.Stop();
                playButton_Click(this, new EventArgs());
                statusLabel.Visible = true;
                toolStripDropDownButton1.Visible = false;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (SettingsForm.PubVar.toggle == true)
            {
                if (SettingsForm.PubVar.langChange != ps.Language)
                {
                    CultureInfo cultureInfo = new CultureInfo(ps.Language);

                    if (statusLabel.Text == translate.newVersion)
                    {
                        ChangeLanguage.Instance.localizeForm(this, cultureInfo);
                        statusLabel.Text = translate.newVersion;
                    }

                    if (statusLabel.Text == translate.serverRestart)
                    {
                        ChangeLanguage.Instance.localizeForm(this, cultureInfo);
                        statusLabel.Text = translate.serverRestart;
                    }

                    if (statusLabel.Text == translate.ready)
                    {
                        ChangeLanguage.Instance.localizeForm(this, cultureInfo);
                    }

                    if (emailComboBox.Text == emailComboBox.Tag.ToString() && passwordTextBox.Text == passwordTextBox.Tag.ToString())
                    {
                        emailComboBox.Tag = translate.email;
                        emailComboBox.Text = emailComboBox.Tag.ToString();

                        passwordTextBox.Tag = translate.password;
                        passwordTextBox.Text = passwordTextBox.Tag.ToString();
                    }
                    SendPing("64.25.35.103");
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
                    SendPing("64.25.35.103");
                    pingTimer.Enabled = true;
                    pingStatusLabel.Visible = true;
                }

                if (ps.Ping == false)
                {
                    pingTimer.Enabled = false;
                    pingStatusLabel.Visible = false;
                }

            }
            SettingsForm.PubVar.toggle = false;
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
                toolStripDropDownButton1.Image = Properties.Resources.timerplay;
            }

            else if (PriorityTimer.Enabled == false)
            {
                PriorityTimer.Start();
                toolStripDropDownButton1.Image = Properties.Resources.timerstop;
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(translate.updMsgBoxText, translate.updMsgBoxTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                File.WriteAllBytes("Updater.exe", Properties.Resources.Updater);
                var pr = new Process();
                pr.StartInfo.FileName = "Updater.exe";
                pr.StartInfo.Arguments = "/u";
                pr.Start();
                Application.Exit();
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

        public void gButton3_Click(object sender, EventArgs e)
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

        void WebBrowser1NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Process.Start(webBrowser1.StatusText);
        }

        void WebBrowser1DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
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
            SendPing("64.25.35.103");
        }

        private void eyeButtonBorder(Pen Color)
        {
            Graphics objGraphics = null;
            objGraphics = this.CreateGraphics();
            objGraphics.Clear(SystemColors.Control);
            objGraphics.DrawRectangle(Color,
            eyeButton.Left - 0, eyeButton.Top - 1,
            eyeButton.Width + 0, eyeButton.Height + 1);
            objGraphics.Dispose();
        }

        private void eyeButton_Paint(object sender, PaintEventArgs e)
        {
            eyeButtonBorder(Pens.DarkGray);
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
            }

            if (passwordTextBox.Text == translate.password)
            {
                eyeButton.Enabled = false;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                eyeButton.BackColor = Color.White;
                eyeButton.Enabled = false;
                passwordTextBox.UseSystemPasswordChar = true;
                eyeButtonBorder(Pens.DarkGray);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboindex = emailComboBox.SelectedIndex;
            passwordTextBox.Text = ini.Read("password" + comboindex, "Password");
            passwordTextBox.ForeColor = Color.Black;
            ps.account = comboindex;
            ps.Save();

            if (eyeButton.BackColor == Color.White)
            {
                passwordTextBox.UseSystemPasswordChar = true;
            }
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

        private void Form1_Click(object sender, EventArgs e)
        {
            statusStrip1.Focus();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contextMenu1_Collapse(object sender, EventArgs e)
        {
            statusStrip1.ContextMenu = null;
        }

        private void toolStripStatusLabel1_Click_1(object sender, EventArgs e)
        {
            statusStrip1.ContextMenu = contextMenu1;
            contextMenu1.Show(statusStrip1, new Point(510, 25));
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            string p = "";

            if (File.Exists(@"C:\Program Files (x86)\NCWest\NCLauncher\NCLauncher.exe"))
            {
                p = " (x86)";
            }

            if ((File.Exists(@"C:\Program Files" + p + @"\NCWest\NCLauncher\NCLauncher.exe")))
            {
                Process pr = new Process();
                pr.StartInfo.FileName = @"C:\Program Files" + p + @"\NCWest\NCLauncher\NCLauncher.exe";
                pr.StartInfo.Arguments = @"/LauncherID:""NCWest"" /CompanyID:""12"" /GameID:""AION"" /LUpdateAddr:""updater.nclauncher.ncsoft.com""";
                pr.Start();
            }

            if ((!File.Exists(@"C:\Program Files" + p + @"\NCWest\NCLauncher\NCLauncher.exe")))
            {
                MessageBox.Show(translate.noLauncherError, translate.updFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void updatesMenuItem_Click(object sender, EventArgs e)
        {
            Thread u = new Thread(UpdateCheck);
            u.Start();
        }

        private void documentationMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://sigmanor.pp.ua/aion-game-launcher/manual");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                documentationMenuItem_Click(this, new EventArgs());
            }
        }

        private void toolStripStatusLabel1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.BackColor = SystemColors.ButtonFace;
        }

        private void toolStripStatusLabel1_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.BackColor = Color.FromArgb(213, 213, 213/*145, 201, 247*/);
        }
    }
}

