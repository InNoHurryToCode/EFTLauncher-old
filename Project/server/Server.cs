using System;
using EFTServer.server;
using EFTServer.server.tools;

namespace EFTServer
{
    /// <summary>
    /// Emulated Escape From Tarkov server
    /// </summary>
    public class Server
    {
        private string domain = "localhost";    // server domain
        private int port = 8080;                // server port
        ServerLoginUpdater serverLoginUpdater;  // login updater
        ServerPortListener serverPortListener;  // port listener

        public Server()
        {
            // intialize logger
            Logger.SetFilePath(Environment.CurrentDirectory + @"/logs/");
            Logger.SetFileName("server");

            // create log
            Logger.Log("--------------------------------------------------");
            Logger.Log("Escape From Tarkov server");
            Logger.Log("https://github.com/InNoHurryToCode/EFTServer");
            Logger.Log("Version: 0.0.1");
            Logger.Log("Created by Merijn Hendriks");
            Logger.Log("--------------------------------------------------");
            Logger.Log("INFO: Log created at: " + DateTime.Now.ToString());
            Logger.Log("INFO: Server started");
        }

        public void Start()
        {
            // initialize processes
            serverLoginUpdater = new ServerLoginUpdater();
            serverPortListener = new ServerPortListener(domain, port);

            // start server
            serverLoginUpdater.Initialize();
            serverPortListener.Initialize();
        }

        public void Stop()
        {
            // terminate processes
            serverLoginUpdater.Terminate();
            serverPortListener.Terminate();

            // log status
            Logger.Log("INFO: Server terminated");
        }
    }
}
