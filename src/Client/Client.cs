using System;
using System.IO;
using Microsoft.Win32;
using EFTLauncher.Utility;

namespace EFTLauncher.ClientLogic
{
    /// <summary>
    /// Emulated Escape From Tarkov client
    /// </summary>
    class Client
    {
        Login login;
        GameMonitor gameMonitor;

        public void Start(string email, string password, string gameDirectory, string domain)
        {
            // log status
            Logger.Log("INFO: Client starting");

            // initialize login
            login = new Login(email, password);
            login.Initialize();

            // launch the game
            gameMonitor = new GameMonitor(gameDirectory);
            gameMonitor.LaunchGame(domain);
        }

        public void Stop()
        {
            login.Terminate();
            Logger.Log("INFO: Client terminated");
        }

        public string DetectGameLocation()
        {
            Logger.Log("INFO: Detecting game installation");

            string foundLocation = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov", "UninstallString", "");
            if (!String.IsNullOrEmpty(foundLocation))
            {
                Logger.Log("INFO: Game installation found");
                return new FileInfo(foundLocation).DirectoryName;
            }
            else
            {
                Logger.Log("INFO: Game installation could not be found");
            }

            return "";
        }
    }
}
