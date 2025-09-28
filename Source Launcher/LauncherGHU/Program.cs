using LauncherGHU.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace LauncherGHU
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            string currentDirectory = Directory.GetCurrentDirectory();
            if (Settings.Default.URLResource == null || Settings.Default.URLResource == "")
            {
                Settings.Default.URLResource = currentDirectory + "\\Resource";
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);
            ControlMgr.EmptyFolder(directoryInfo);
            Application.Run(new formLogin());

        }
    }
}
