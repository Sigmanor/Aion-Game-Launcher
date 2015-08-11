using Ionic.Zip;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Updater
{

    public partial class UpdateForm : Form
    {
        string[] arg;

        public UpdateForm(string[] args)
        {
            arg = args;
            InitializeComponent();
        }


        #region ExtractZip
        public void ExtractFileToDirectory(string zipFileName, string outputDirectory)
        {
            ZipFile zip = ZipFile.Read(zipFileName);
            Directory.CreateDirectory(outputDirectory);
            zip.ExtractAll(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
        }
        #endregion


        void downloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var pr = new Process();

            if (e.Error != null)
            {
                pr.StartInfo.FileName = "Aion Game Launcher.exe";
                pr.StartInfo.Arguments = "/e";
                pr.Start();
            }

            if (e.Error == null)
            {
                ExtractFileToDirectory("Aion-Game-Launcher-Update.zip", @".\");

                pr.StartInfo.FileName = "Aion Game Launcher.exe";
                pr.StartInfo.Arguments = "/u";
                pr.Start();

            }

            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (arg.Length == 0)
            {
                this.Close();
            }

            else if (arg[0] == "/u")
            {
                WebClient webClient = new WebClient();

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(downloader_DownloadFileCompleted);

                webClient.DownloadFileAsync(new Uri("http://sigmanor.tk/soft/Aion-Game-Launcher/Update.zip"), "Aion-Game-Launcher-Update.zip");
            }
        }



    }
}
