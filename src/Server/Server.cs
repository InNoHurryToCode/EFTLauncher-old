using System;
using EFTLauncher.ClientLogic;
using EFTLauncher.ServerLogic;
using EFTLauncher.Utility;

namespace EFTLauncher.ServerLogic
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
            // log state
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
