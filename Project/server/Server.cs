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
        ClientLogin clientLogin;                        // login updater
        ServerResponseListener serverResponseListener;  // port listener

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

        public void Start(string email, string password, string domain, int port)
        {
            // initialize processes
            clientLogin = new ClientLogin(email, password);
            serverResponseListener = new ServerResponseListener(domain, port);

            // start server
            clientLogin.Initialize();
            serverResponseListener.Initialize();
        }

        public void Stop()
        {
            // terminate processes
            clientLogin.Terminate();
            serverResponseListener.Terminate();

            // log status
            Logger.Log("INFO: Server terminated");
        }
    }
}
