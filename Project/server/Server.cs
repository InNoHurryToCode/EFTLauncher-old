using System;
using System.Collections.Generic;
using EFTServer.server.data;

namespace EFTServer
{
    /// <summary>
    /// Emulated Escape From Tarkov server
    /// </summary>
    public class Server
    {      
        public Server()
        {
            // intialize logger
            Logger.SetFilePath(Environment.CurrentDirectory + @"/logs/");
            Logger.SetFileName("server");
        }

        public void Start()
        {
            // create log
            Logger.Log("--------------------------------------------------");
            Logger.Log("Escape From Tarkov server");
            Logger.Log("https://github.com/InNoHurryToCode/EFTServer");
            Logger.Log("Version: 0.0.1");
            Logger.Log("Created by Merijn Hendriks");
            Logger.Log("INFO: Server started");
            Logger.Log("--------------------------------------------------");

            // login
            Login();
        }

        public void Stop()
        {
            Logger.Log("INFO: Server terminated");
        }

        public void Login()
        {
            // get login data
            LoginData loginData = JsonHelper.LoadJson<LoginData>(@"data/account/login.json");
            Logger.Log("INFO: Login data:");
            Logger.Log(loginData.ToString());

            // calculate timestamp
            double currentDay = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
            double tempTimestamp = Math.Floor(currentDay / 1000) + 45;

            loginData.timestamp = (long)tempTimestamp ^ 698464131;
            Logger.Log("INFO: Login timestamp:");
            Logger.Log(loginData.timestamp.ToString());
        }
    }
}
