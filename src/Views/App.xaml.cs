using System;
using System.Windows;
using EFTLauncher.Utility;

namespace EFTLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {
            // intialize logger
            Logger.SetFilePath(Environment.CurrentDirectory + @"/logs/");
            Logger.SetFileName(DateTime.Now.ToString("yyyyMMddTHHmmss").Replace(":", "."));

            // create log
            Logger.Log("--------------------------------------------------");
            Logger.Log("Escape From Tarkov server");
            Logger.Log("https://github.com/InNoHurryToCode/EFTServer");
            Logger.Log("Version: 0.0.1");
            Logger.Log("Created by Merijn Hendriks");
            Logger.Log("--------------------------------------------------");
            Logger.Log("INFO: Log created at: " + DateTime.Now.ToString());
        }
    }
}
