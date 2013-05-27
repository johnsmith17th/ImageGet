using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ImageGet
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                MessageBox.Show("No file to download.");
            }

            // get save directory
            string path = GetDirectory();

            if (!string.IsNullOrEmpty(path))
            {
                if (args.Length > 1)
                {
                    MainWin2 win2 = new MainWin2();
                    win2.InitializeParameters(args, path);
                    Application.Run(win2);
                }
                else
                {
                    MainWin win = new MainWin();
                    win.InitializeParameters(args[0], path);
                    Application.Run(win);
                }
            }
        }

        /// <summary>
        /// Select the directory on local drive to save files
        /// </summary>
        internal static string GetDirectory()
        {
            using (FolderBrowserDialog d = new FolderBrowserDialog())
            {
                d.ShowNewFolderButton = true;
                if (DialogResult.OK == d.ShowDialog())
                {
                    return d.SelectedPath;
                }
            }
            return null;
        }

        /// <summary>
        /// open the directory with explorer
        /// </summary>
        internal static void OpenFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        /// <summary>
        /// Download a file on the Internet, with a web client
        /// </summary>
        internal static bool Download(WebClient client, string address, string fileName)
        {
            try
            {
                client.DownloadFile(address, fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Download a file on the Internet
        /// </summary>
        internal static bool Download(string address, string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(address, fileName);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the path to save at on local drivers, use the name of the downloading file
        /// </summary>
        internal static string GetLocalPath(string address, string destination)
        {
            int lastSlash = address.LastIndexOf('/');
            return Path.Combine(destination, address.Substring(lastSlash + 1));
        }
    }
}
