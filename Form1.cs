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

namespace Aion_Launcher
{
    public partial class Form1 : Form
    {
        public bool l = false;
        int i;
        Properties.Settings ps = Properties.Settings.Default;
        string[] arg;
        public Form1(string[] args)
        {
            if (ps.License == 0)
            {
                InitializeComponent();
                License l = new License();
                l.ShowDialog();
                arg = args;
                LoginTextBox.ForeColor = Color.Gray;
                PasswordTextBox.ForeColor = Color.Gray;
            }

            else if (ps.License == 1)
            {
                InitializeComponent();
                arg = args;
                LoginTextBox.ForeColor = Color.Gray;
                PasswordTextBox.ForeColor = Color.Gray;
            }
            statusStrip1.Padding = new Padding(statusStrip1.Padding.Left,
            statusStrip1.Padding.Top, statusStrip1.Padding.Left, statusStrip1.Padding.Bottom);
        }

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

        private void PasswordTextBox_Enter(object sender, EventArgs e)
        {
            Font font = new Font(PasswordTextBox.Font, FontStyle.Regular);
            PasswordTextBox.Font = font;
            PasswordTextBox.ForeColor = Color.Black;

            if (PasswordTextBox.Text == "Пароль")
            {
                PasswordTextBox.Clear();
                PasswordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void LoginTextBox_Enter(object sender, EventArgs e)
        {
            Font font = new Font(LoginTextBox.Font, FontStyle.Regular);
            LoginTextBox.Font = font;
            LoginTextBox.ForeColor = Color.Black;

            if (LoginTextBox.Text == "Логин")
                LoginTextBox.Clear();
        }
      
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (PasswordTextBox.Text != "Пароль" && LoginTextBox.Text != "Логин" && PasswordTextBox.Text != "" && LoginTextBox.Text != "")
                {
                    ps.Pass = PasswordTextBox.Text;
                    ps.Log = LoginTextBox.Text;
                    ps.Checked = checkBox1.Checked;
                    ps.Save();
                }
            }
            if (checkBox1.Checked == false)
            {
                ps.Log = "Логин";
                ps.Pass = "Пароль";
                ps.Checked = checkBox1.Checked;
                ps.Save();
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
            if (CheckForInternetConnection() == true)
            {
                Thread s = new Thread(ServerStatus);
                s.Start();

                if (ps.AutoUPD == true)
                {
                    Thread a = new Thread(AutoUPD);
                    a.Start();
                }

                if (ps.RestartAlert == true)
                {
                    Thread r = new Thread(RestartAlert);
                    r.Start();
                }

                webBrowser1.Navigate(new Uri("http://web-launcher.ncsoft.com/aion/en/installed_hq.php#"));
            }


            if (ps.Ping == true)
            {
                backgroundWorker1.RunWorkerAsync();
                timer1.Enabled = true;
                pingStatusLabel.Visible = true;
            }

            if (ps.Ping == false)
            {
                pingStatusLabel.Visible = false;
            }

            ServerStatusCheck();

            LoginTextBox.Text = ps.Log;
            PasswordTextBox.Text = ps.Pass;
            checkBox1.Checked = ps.Checked;

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

            if (LoginTextBox.Text == "Логин" | PasswordTextBox.Text == "Пароль")
            {
                LoginTextBox.ForeColor = Color.Gray;
                PasswordTextBox.ForeColor = Color.Gray;
                PasswordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                Font font = new Font(LoginTextBox.Font, FontStyle.Regular);
                LoginTextBox.Font = font;
                PasswordTextBox.Font = font;
                LoginTextBox.ForeColor = Color.Black;
                PasswordTextBox.ForeColor = Color.Black;
            }

            if (arg.Length == 0) /* Аргумент = 0 */
            {
                if (ps.Priority == true)
                {
                    this.WindowState = FormWindowState.Minimized;
                    toolStripStatusLabel1.Visible = false;
                    toolStripDropDownButton1.Visible = true;
                    i = 3;
                    toolStripDropDownButton1.Text = "Запуск игры через: " + i.ToString();
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
        }

        public void ServerStatusCheck()
        {
        	toolStripStatusLabel3.Text = ps.SCCB;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureBox1.Focus(); 
        }


        #region Поток проверки рестарта
        public void RestartAlert()
        {
            try {
            HttpWebRequest req; 
            HttpWebResponse resp;
            StreamReader sr;
            string C;
            string RS;

                req = (HttpWebRequest)WebRequest.Create("http://24timezones.com/usa_time/tx_galveston/texas_city.htm");
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                C = sr.ReadToEnd();
                sr.Close();

                RS = "Wednesday";

                if (C.IndexOf(RS) > -1)
                {
                    MethodInvoker bl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                    statusStrip1.BeginInvoke(bl);
                    MethodInvoker restnow = () => toolStripStatusLabel1.Text = "Сегодня рестарт!";
                    statusStrip1.Invoke(restnow);
                    Bitmap restimg = Properties.Resources.restart;
                    MethodInvoker hj = () => toolStripStatusLabel1.Image = restimg;
                    pictureBox2.BeginInvoke(hj);   
                }

                else 
                {
                    MethodInvoker bl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                    statusStrip1.BeginInvoke(bl);
                    MethodInvoker gocrab = () => toolStripStatusLabel1.Text = "Го крабить!";
                    statusStrip1.Invoke(gocrab);

                    Bitmap crabimg = Properties.Resources.crab;
                    MethodInvoker gc = () => toolStripStatusLabel1.Image = crabimg;
                    pictureBox2.BeginInvoke(gc);
                }

                } catch { }
        }
        #endregion

        #region Поток проверки обновлений

        public void UpdateCheck()
        {
            MethodInvoker link = () => toolStripStatusLabel1.IsLink = false;
            statusStrip1.BeginInvoke(link);
            MethodInvoker cl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
            statusStrip1.BeginInvoke(cl);
            MethodInvoker tx = () => toolStripStatusLabel1.Text = "Проверка обновления...";
            statusStrip1.BeginInvoke(tx);
            Bitmap looa = Properties.Resources.loader;
            MethodInvoker diin = () => toolStripStatusLabel1.Image = looa;
            pictureBox2.BeginInvoke(diin);

            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://sigmanor.tk/soft/Aion-Game-Launcher/version");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                String version = Application.ProductVersion;

                int con = Convert.ToInt32(content.Replace(".", ""));
                int ver = Convert.ToInt32(version.Replace(".", ""));

                if (con != ver)
                {
                    Bitmap up = Properties.Resources.upd;
                    MethodInvoker upimg = () => toolStripStatusLabel1.Image = up;
                    pictureBox2.BeginInvoke(upimg);

                    MethodInvoker ne = () => toolStripStatusLabel1.Text = "Доступна новая версия!";
                    statusStrip1.BeginInvoke(ne);
                    MethodInvoker fo = () => toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Text, 8, FontStyle.Regular | FontStyle.Underline);
                    statusStrip1.BeginInvoke(fo);
                    MethodInvoker ho = () => toolStripStatusLabel1.ForeColor = SystemColors.HotTrack;
                    statusStrip1.BeginInvoke(ho);
                    MethodInvoker ha = () => toolStripStatusLabel1.IsLink = true;
                    statusStrip1.BeginInvoke(ha);
                }
                else 
                {
                    Bitmap noup = Properties.Resources.noupd;
                    MethodInvoker tlimg = () => toolStripStatusLabel1.Image = noup;
                    pictureBox2.BeginInvoke(tlimg);
                    MethodInvoker co = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.SeaGreen;
                    statusStrip1.BeginInvoke(co);
                    MethodInvoker ls = () => toolStripStatusLabel1.Text = "Установлена последняя версия!";
                    statusStrip1.BeginInvoke(ls);
                    System.Threading.Thread.Sleep(3000);

                    if (ps.RestartAlert == true) /* Restart Check */
                    {
                        RestartAlert();
                    }

                    else
                    {
                        MethodInvoker bl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                        statusStrip1.BeginInvoke(bl);
                        MethodInvoker gocrab = () => toolStripStatusLabel1.Text = "Го крабить!";
                        statusStrip1.Invoke(gocrab);
                        Bitmap crabimg = Properties.Resources.crab;
                        MethodInvoker gc = () => toolStripStatusLabel1.Image = crabimg;
                        pictureBox2.BeginInvoke(gc);
                    }
                  
                }

            }

            catch (WebException)
            {
                Bitmap ufail = Properties.Resources.updfail;
                MethodInvoker uimg = () => toolStripStatusLabel1.Image = ufail;
                pictureBox2.BeginInvoke(uimg);
                MethodInvoker ik = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                statusStrip1.BeginInvoke(ik);
                MethodInvoker ls = () => toolStripStatusLabel1.Text = "Сервер обновления не доступен!";
                statusStrip1.BeginInvoke(ls);

                System.Threading.Thread.Sleep(3000);

                if (ps.RestartAlert == true) /* Restart Check */
                {
                    Thread r = new Thread(RestartAlert);
                    r.Start();
                }

                if (ps.RestartAlert == false)
                {
                    try
                    {
                        MethodInvoker bl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                        statusStrip1.BeginInvoke(bl);
                        MethodInvoker gocrab = () => toolStripStatusLabel1.Text = "Го крабить!";
                        statusStrip1.Invoke(gocrab);

                        Bitmap crabimg = Properties.Resources.crab;
                        MethodInvoker gc = () => toolStripStatusLabel1.Image = crabimg;
                        pictureBox2.BeginInvoke(gc);
                    }
                    catch
                    {                      
                        return;
                    }
                }

            }
        }
            #endregion

        #region Поток проверки пинга

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                pingStatusLabel.Text = "Пинг: " + new Ping().Send("64.25.35.103").RoundtripTime.ToString() + " мсек.";
            }
            catch (PingException)
            {
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return;
            }
        }

        #endregion

        #region Поток автоматической проверки обновлений
        public void AutoUPD()
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://sigmanor.tk/soft/Aion-Game-Launcher/version");
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                String version = Application.ProductVersion;

                int con = Convert.ToInt32(content.Replace(".", ""));
                int ver = Convert.ToInt32(version.Replace(".", ""));

                if (con != ver)
                {
                    Bitmap crabimg = Properties.Resources.upd;
                    MethodInvoker gc = () => toolStripStatusLabel1.Image = crabimg;
                    pictureBox2.BeginInvoke(gc);

                    MethodInvoker w = () => toolStripStatusLabel1.Text = "Доступна новая версия!";
                    statusStrip1.BeginInvoke(w);

                    MethodInvoker r = () => toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Text, 8, FontStyle.Regular | FontStyle.Underline);
                    statusStrip1.BeginInvoke(r);

                    MethodInvoker t = () => toolStripStatusLabel1.ForeColor = SystemColors.HotTrack;
                    statusStrip1.BeginInvoke(t);

                    MethodInvoker ha = () => toolStripStatusLabel1.IsLink = true;
                    statusStrip1.BeginInvoke(ha);
                }
            }
            catch
            {
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

                //if (ps.Monitoring == 2)
                //{
                //    site = "http://rainy.ws/server-status/";
                //}


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
                    //C = C.Replace("\n", string.Empty);
                    //C = C.Replace(" ", string.Empty);
                    IS = "online.png\" /> Israphel";
                    KR = "online.png\" /> Kahrun";
                    SL = "online.png\" /> Siel";
                    TM = "online.png\" /> Tiamat";
                    LG = "online.png\" /> Login(NA)";
                }

                Bitmap on = Properties.Resources.bullet_green;
                Bitmap off = Properties.Resources.bullet_red;

                if (ps.SCCB == "Israphel")
                {
                    if (C.IndexOf(IS) > -1)
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
                        pictureBox2.BeginInvoke(gc);
                    }
                    else
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
                        pictureBox2.BeginInvoke(gc);
                    }
                }


                if (ps.SCCB == "Kahrun")
                {
                    if (C.IndexOf(KR) > -1)
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
                        pictureBox2.BeginInvoke(gc);
                    }
                    else
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
                        pictureBox2.BeginInvoke(gc);
                    }
                }

                if (ps.SCCB == "Siel")
                {
                    if (C.IndexOf(SL) > -1)
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
                        pictureBox2.BeginInvoke(gc);
                    }
                    else
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
                        pictureBox2.BeginInvoke(gc);
                    }
                }

                if (ps.SCCB == "Tiamat")
                {
                    if (C.IndexOf(TM) > -1)
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
                        pictureBox2.BeginInvoke(gc);
                    }
                    else
                    {
                        MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
                        pictureBox2.BeginInvoke(gc);
                    }
                }

                /* NC Login */
                if (C.IndexOf(LG) > -1)
                {
                    MethodInvoker gc = () => toolStripStatusLabel2.Image = on;
                    pictureBox2.BeginInvoke(gc);
                }
                else
                {
                    MethodInvoker gc = () => toolStripStatusLabel2.Image = off;
                    pictureBox2.BeginInvoke(gc);
                }
            }
            catch
            {
                //MethodInvoker err1 = () => toolStripStatusLabel3.Image = Properties.Resources.bullet_purple;
                //pictureBox2.BeginInvoke(err1);

                //MethodInvoker err2 = () => toolStripStatusLabel2.Image = Properties.Resources.bullet_purple;
                //pictureBox2.BeginInvoke(err2);
                //MessageBox.Show("Web Exception");
            }
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
            toolStripDropDownButton1.Text = "Запуск игры через: " + (--i).ToString();
            if (i < 0)
            { 
                PriorityTimer.Stop();
                toolStripDropDownButton1.Visible = false;
                GoNagibatButton_Click(this, new EventArgs());
            }
        }

        #region commented out
        //#region Поток автоматической проверки обновлений
        //public void AutoUPD()
        //    {      
        //            try
        //            {
        //                WebClient client = new WebClient();
        //                Stream stream = client.OpenRead("http://sigmanor.tk/soft/Aion-Game-Launcher/version");
        //                StreamReader reader = new StreamReader(stream);
        //                String content = reader.ReadToEnd();
        //                String version = Application.ProductVersion;

        //                int con = Convert.ToInt32(content.Replace(".", ""));
        //                int ver = Convert.ToInt32(version.Replace(".", ""));

        //                if (con != ver)
        //                {
        //                    if (ps.AutoUPD == true)
        //                    {
        //                        Bitmap crabimg = Properties.Resources.upd;
        //                        MethodInvoker gc = () => toolStripStatusLabel1.Image = crabimg;
        //                        pictureBox2.BeginInvoke(gc);

        //                        MethodInvoker w = () => toolStripStatusLabel1.Text = "Доступна новая версия!";
        //                        label6.BeginInvoke(w);

        //                        MethodInvoker r = () => toolStripStatusLabel1.Font = new Font(label6.Text, 8, FontStyle.Regular | FontStyle.Underline);
        //                        label6.BeginInvoke(r);

        //                        MethodInvoker t = () => toolStripStatusLabel1.ForeColor = SystemColors.HotTrack;
        //                        label6.BeginInvoke(t);

        //                        MethodInvoker ha = () => toolStripStatusLabel1.IsLink = true;
        //                        label6.BeginInvoke(ha);
        //                    }
        //                }
        //            }
        //            catch (WebException)
        //            {
        //                //Bitmap ufail = Properties.Resources.updfail;
        //                //MethodInvoker uimg = () => toolStripStatusLabel1.Image = ufail;
        //                //pictureBox2.BeginInvoke(uimg);

        //                //MethodInvoker cl = () => toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
        //                //label6.BeginInvoke(cl);
        //                //MethodInvoker ls = () => toolStripStatusLabel1.Text = "Ошибка соединения!";
        //                //label6.BeginInvoke(ls);
        //            }
        //    }
        //    #endregion

        //#region Поток проверки статуса серверов
        //    public void ServerStatus()
        //    {
        //        HttpWebRequest req;
        //        HttpWebResponse resp;
        //        StreamReader sr;
        //        string C;
        //        string IS = "", KR = "", SL = "", TM = "", LG = "";
        //        string site = "";

        //        try
        //        {
        //            if (ps.Monitoring == 0) { site = "http://aionstatus.net/"; }

        //            if (ps.Monitoring == 1) { site = "http://aion.im/status/status.php"; }

        //            if (ps.Monitoring == 2) { site = "http://aion.mouseclic.com/tool/status/"; }              

        //            //if (ps.Monitoring == 2)
        //            //{
        //            //    site = "http://rainy.ws/server-status/";
        //            //}


        //            req = (HttpWebRequest)WebRequest.Create(site);
        //            resp = (HttpWebResponse)req.GetResponse();
        //            sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
        //            C = sr.ReadToEnd();
        //            sr.Close();

        //            if (ps.Monitoring == 0)
        //            {
        //                IS = "Online</font></td><td><a href=\"fav.php?favorite=Israphel";
        //                KR = "Online</font></td><td><a href=\"fav.php?favorite=Kahrun";
        //                SL = "Online</font></td><td><a href=\"fav.php?favorite=Siel";
        //                TM = "Online</font></td><td><a href=\"fav.php?favorite=Tiamat";
        //                LG = "Online</font></td><td><a href=\"fav.php?favorite=NA_Login";
        //            }

        //            if (ps.Monitoring == 1)
        //            {
        //                IS = "class=\"lang-na\"></span>Israphel<span class=\"status-1\">";
        //                KR = "class=\"lang-na\"></span>Kahrun<span class=\"status-1\">";
        //                SL = "class=\"lang-na\"></span>Siel<span class=\"status-1\">";
        //                TM = "class=\"lang-na\"></span>Tiamat<span class=\"status-1\">";
        //                LG = "class=\"lang-na\"></span>NCSoft Login<span class=\"status-1\">";
        //            }

        //            if (ps.Monitoring == 2)
        //            {
        //                //C = C.Replace("\n", string.Empty);
        //                //C = C.Replace(" ", string.Empty);
        //                IS = "online.png\" /> Israphel";
        //                KR = "online.png\" /> Kahrun";
        //                SL = "online.png\" /> Siel";
        //                TM = "online.png\" /> Tiamat";
        //                LG = "online.png\" /> Login(NA)";
        //            }

        //            Bitmap on = Properties.Resources.bullet_green;
        //            Bitmap off = Properties.Resources.bullet_red;

        //            if (ps.SCCB == "Israphel")
        //            {
        //                if (C.IndexOf(IS) > -1)
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //                else
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //            }


        //            if (ps.SCCB == "Kahrun")
        //            {
        //                if (C.IndexOf(KR) > -1)
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //                else
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //            }

        //            if (ps.SCCB == "Siel")
        //            {
        //                if (C.IndexOf(SL) > -1)
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //                else
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //            }

        //            if (ps.SCCB == "Tiamat")
        //            {
        //                if (C.IndexOf(TM) > -1)
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = on;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //                else
        //                {
        //                    MethodInvoker gc = () => toolStripStatusLabel3.Image = off;
        //                    pictureBox2.BeginInvoke(gc);
        //                }
        //            }
					
        //            /* NC Login */
        //             if (C.IndexOf(LG) > -1)
        //            {
        //                MethodInvoker gc = () => toolStripStatusLabel2.Image = on;
        //                pictureBox2.BeginInvoke(gc);
        //            }
        //            else
        //            { 
        //                MethodInvoker gc = () => toolStripStatusLabel2.Image = off;
        //                pictureBox2.BeginInvoke(gc);
        //            }
        //        }
        //        catch /*(WebException)*/
        //        {
        //            /*MethodInvoker err1 = () => toolStripStatusLabel3.Image = Properties.Resources.bullet_purple;
        //            pictureBox2.BeginInvoke(err1);

        //            MethodInvoker err2 = () => toolStripStatusLabel2.Image = Properties.Resources.bullet_purple;
        //            pictureBox2.BeginInvoke(err2);*/
        //            //MessageBox.Show("Exception");
        //        }
        //    }
        //     #endregion
        #endregion

        private void Form1_Activated(object sender, EventArgs e)
            {
                if (Aion_Launcher.Settings.PubVar.toggle == true)
                {                
                    Aion_Launcher.Settings.PubVar.toggle = false;
                    toolStripStatusLabel1.Visible = true;
                    ServerStatusCheck();
                    Thread s = new Thread(ServerStatus);
                    s.Start();
                    if (ps.Priority == false)
                    {
                        PriorityTimer.Stop();
                        toolStripDropDownButton1.Visible = false;
                    }

                    if (ps.Ping == true)
                    {
                        backgroundWorker1.RunWorkerAsync();
                        timer1.Enabled = true;
                        pingStatusLabel.Visible = true;
                    }

                    if (ps.Ping == false)
                    {
                        timer1.Enabled = false;
                        pingStatusLabel.Visible = false;
                    }  
                }
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

                if (toolStripStatusLabel1.Text == "Доступна новая версия!")
                {
                    DialogResult result = MessageBox.Show("Сейчас будет скачано и установлено обновление", "Обновление", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {

                        if (RemoteFileExists("http://sigmanor.tk/soft/Aion-Game-Launcher/AionGameLauncher.exe"))
                        {
                            Bitmap la = Properties.Resources.load_anim;
                            toolStripStatusLabel1.Image = la;
                            toolStripStatusLabel1.Text = "Обновление...";
                            toolStripStatusLabel1.IsLink = false;
                            toolStripStatusLabel1.Font = new Font(toolStripStatusLabel1.Text, 8, FontStyle.Regular);
                            toolStripStatusLabel1.ForeColor = Color.Black;

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

                            webClient.DownloadFileAsync(new Uri("http://sigmanor.tk/soft/Aion-Game-Launcher/AionGameLauncher.exe"), "aiongamelauncher.update");
                        }

                        else
                        {
                            MessageBox.Show("Обновление не удалось\nПопробуйте еще раз позже", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            private void GoNagibatButton_Click(object sender, EventArgs e)
            {
                string sd = "/" + System.Environment.SystemDirectory.Substring(0, 1) + " ";
                string r = ps.GamePath;
                r = r.Replace(":\\", ":\\\"");
                string bin = "";

                if (PasswordTextBox.Text == "Пароль" | LoginTextBox.Text == "Логин")
                {
                    if (ps.Capacity == 0)
                    {
                        bin = "bin32";
                    }

                    if (ps.Capacity == 1)
                    {
                        bin = "bin64";
                    }
                    PasswordTextBox.Text = "";
                    LoginTextBox.Text = "";
                }

                else if (PasswordTextBox.Text != "Пароль" | LoginTextBox.Text != "Логин")
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
                    Arguments = sd + " start " + r + "\\" + bin + "\\AION.bin\" -ip:64.25.35.103 -port:2106 -cc:1 -noauthgg -charnamemenu -lbox -f2p -loginex -pwd16 -nosatab -lang:enu -account:" + LoginTextBox.Text + " -password:" + PasswordTextBox.Text + " " + ps.Extra
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
                    MessageBox.Show("Лаунчер не установлен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            		       
        
        void WebBrowser1NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	 e.Cancel = true;
        	 Process.Start(webBrowser1.StatusText);             
        }
        
        void WebBrowser1DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (CheckForInternetConnection() == true)
            //{
            //    try
            //    {
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
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }



    }
}

