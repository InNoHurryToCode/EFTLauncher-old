using System;
using System.Collections.Generic;
using System.Text;

namespace EFTServer
{
    /// <summary>
    /// Emulated Escape From Tarkov server
    /// </summary>
    class Server
    {      
        public Server()
        {
            // setup logger
            Logger.SetFilePath(Environment.CurrentDirectory + "/data/logs/");
            Logger.SetFileName("server");

            // start the server
            Start();
        }

        public void Start()
        {
            // create log
            Logger.Log("Escape From Tarkov server");
            Logger.Log("https://github.com/InNoHurryToCode/EFTServer");
            Logger.Log("Version: 0.0.1");
            Logger.Log("Created by Merijn Hendriks");
            Logger.Log("--------------------------------------------------");
        }
    }
}
