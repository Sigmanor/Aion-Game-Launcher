namespace Aion_Launcher
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.SettingsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officialLauncherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.docsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.PriorityTimer = new System.Windows.Forms.Timer(this.components);
            this.AutoUPDtimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pingStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pingTimer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.playButton = new System.Windows.Forms.Button();
            this.eyeButton = new System.Windows.Forms.PictureBox();
            this.webSiteButton = new Aion_Launcher.GButton();
            this.vkButton = new Aion_Launcher.GButton();
            this.scheduleButton = new Aion_Launcher.GButton();
            this.usefulButton = new Aion_Launcher.GButton();
            this.SettingsMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eyeButton)).BeginInit();
            this.SuspendLayout();
            // 
            // loginTextBox
            // 
            this.loginTextBox.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.loginTextBox, "loginTextBox");
            this.loginTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Tag = "Email";
            this.loginTextBox.Enter += new System.EventHandler(this.LoginTextBox_Enter);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.passwordTextBox, "passwordTextBox");
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Tag = "Password";
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.passwordTextBox_TextChanged);
            this.passwordTextBox.Enter += new System.EventHandler(this.PasswordTextBox_Enter);
            this.passwordTextBox.Leave += new System.EventHandler(this.passwordTextBox_Leave);
            // 
            // rememberCheckBox
            // 
            resources.ApplyResources(this.rememberCheckBox, "rememberCheckBox");
            this.rememberCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.rememberCheckBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.UseVisualStyleBackColor = false;
            this.rememberCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SettingsMenu
            // 
            resources.ApplyResources(this.SettingsMenu, "SettingsMenu");
            this.SettingsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.moreToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.SettingsMenu.Name = "contextMenuStrip1";
            this.SettingsMenu.OwnerItem = this.menuDropDownButton;
            // 
            // settingsToolStripMenuItem
            // 
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // moreToolStripMenuItem
            // 
            this.moreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.officialLauncherToolStripMenuItem});
            resources.ApplyResources(this.moreToolStripMenuItem, "moreToolStripMenuItem");
            this.moreToolStripMenuItem.Name = "moreToolStripMenuItem";
            // 
            // officialLauncherToolStripMenuItem
            // 
            resources.ApplyResources(this.officialLauncherToolStripMenuItem, "officialLauncherToolStripMenuItem");
            this.officialLauncherToolStripMenuItem.Name = "officialLauncherToolStripMenuItem";
            this.officialLauncherToolStripMenuItem.Click += new System.EventHandler(this.официальныйЛаунчерToolStripMenuItem_Click_1);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.docsToolStripMenuItem,
            this.updatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // docsToolStripMenuItem
            // 
            resources.ApplyResources(this.docsToolStripMenuItem, "docsToolStripMenuItem");
            this.docsToolStripMenuItem.Name = "docsToolStripMenuItem";
            this.docsToolStripMenuItem.Click += new System.EventHandler(this.документацияToolStripMenuItem_Click);
            // 
            // updatesToolStripMenuItem
            // 
            resources.ApplyResources(this.updatesToolStripMenuItem, "updatesToolStripMenuItem");
            this.updatesToolStripMenuItem.Name = "updatesToolStripMenuItem";
            this.updatesToolStripMenuItem.Click += new System.EventHandler(this.обновленияToolStripMenuItem_Click_1);
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click_1);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // menuDropDownButton
            // 
            resources.ApplyResources(this.menuDropDownButton, "menuDropDownButton");
            this.menuDropDownButton.DropDown = this.SettingsMenu;
            this.menuDropDownButton.Margin = new System.Windows.Forms.Padding(0, 2, 5, 0);
            this.menuDropDownButton.Name = "menuDropDownButton";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // PriorityTimer
            // 
            this.PriorityTimer.Tick += new System.EventHandler(this.PriorityTimer_Tick);
            // 
            // AutoUPDtimer
            // 
            this.AutoUPDtimer.Enabled = true;
            this.AutoUPDtimer.Interval = 60000;
            this.AutoUPDtimer.Tick += new System.EventHandler(this.AutoUPDtimer_Tick);
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel8,
            this.pingStatusLabel,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.menuDropDownButton});
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            // 
            // statusLabel
            // 
            this.statusLabel.ActiveLinkColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusLabel.Image = global::Aion_Launcher.Properties.Resources.controller;
            this.statusLabel.LinkColor = System.Drawing.Color.Blue;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // toolStripDropDownButton1
            // 
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.Image = global::Aion_Launcher.Properties.Resources.timerstop;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // toolStripStatusLabel8
            // 
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Spring = true;
            // 
            // pingStatusLabel
            // 
            this.pingStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            resources.ApplyResources(this.pingStatusLabel, "pingStatusLabel");
            this.pingStatusLabel.Name = "pingStatusLabel";
            this.pingStatusLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Image = global::Aion_Launcher.Properties.Resources.loadgif;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser1DocumentCompleted);
            this.webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(this.WebBrowser1NewWindow);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::Aion_Launcher.Properties.Resources.ai_main2;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::Aion_Launcher.Properties.Resources.panels;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.webSiteButton);
            this.panel2.Controls.Add(this.vkButton);
            this.panel2.Controls.Add(this.scheduleButton);
            this.panel2.Controls.Add(this.usefulButton);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // pingTimer
            // 
            this.pingTimer.Interval = 3000;
            this.pingTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // playButton
            // 
            this.playButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(7)))));
            this.playButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.playButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(175)))), ((int)(((byte)(64)))));
            this.playButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(161)))), ((int)(((byte)(29)))));
            resources.ApplyResources(this.playButton, "playButton");
            this.playButton.ForeColor = System.Drawing.Color.White;
            this.playButton.Name = "playButton";
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // eyeButton
            // 
            this.eyeButton.BackColor = System.Drawing.SystemColors.Window;
            this.eyeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.eyeButton.Image = global::Aion_Launcher.Properties.Resources.eo;
            resources.ApplyResources(this.eyeButton, "eyeButton");
            this.eyeButton.Name = "eyeButton";
            this.eyeButton.TabStop = false;
            this.eyeButton.Paint += new System.Windows.Forms.PaintEventHandler(this.eyeButton_Paint);
            this.eyeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.eyeButton_MouseDown);
            this.eyeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.eyeButton_MouseUp);
            // 
            // webSiteButton
            // 
            this.webSiteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.webSiteButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.webSiteButton, "webSiteButton");
            this.webSiteButton.ForeColor = System.Drawing.Color.White;
            this.webSiteButton.Image = null;
            this.webSiteButton.Name = "webSiteButton";
            this.webSiteButton.Click += new System.EventHandler(this.gButton2_Click);
            // 
            // vkButton
            // 
            this.vkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.vkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.vkButton, "vkButton");
            this.vkButton.ForeColor = System.Drawing.Color.White;
            this.vkButton.Image = null;
            this.vkButton.Name = "vkButton";
            this.vkButton.Click += new System.EventHandler(this.gButton2_Click);
            // 
            // scheduleButton
            // 
            this.scheduleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.scheduleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.scheduleButton, "scheduleButton");
            this.scheduleButton.ForeColor = System.Drawing.Color.White;
            this.scheduleButton.Image = null;
            this.scheduleButton.Name = "scheduleButton";
            this.scheduleButton.Click += new System.EventHandler(this.gButton4_Click);
            // 
            // usefulButton
            // 
            this.usefulButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(41)))));
            this.usefulButton.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.usefulButton, "usefulButton");
            this.usefulButton.ForeColor = System.Drawing.Color.White;
            this.usefulButton.Image = null;
            this.usefulButton.Name = "usefulButton";
            this.usefulButton.Click += new System.EventHandler(this.gButton3_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.eyeButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.loginTextBox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SettingsMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eyeButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.ContextMenuStrip SettingsMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer PriorityTimer;
        private System.Windows.Forms.Timer AutoUPDtimer;
        private System.Windows.Forms.Panel panel2;
        private GButton scheduleButton;
        private GButton vkButton;
        private GButton usefulButton;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton menuDropDownButton;
        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem docsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem updatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officialLauncherToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripStatusLabel pingStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.Timer pingTimer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private GButton webSiteButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.PictureBox eyeButton;
    }
}

