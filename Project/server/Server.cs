using System;
using System.Timers;
using Microsoft.Win32;
using EFTServer.server.data;

namespace EFTServer
{
    /// <summary>
    /// Emulated Escape From Tarkov server
    /// </summary>
    public class Server
    {
        private int port = 8080;                // port to listen to
        private Timer createLoginTokenTimer;    // checks when login token needs to be renewed

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
            Logger.Log("--------------------------------------------------");
            Logger.Log("INFO: Server started");

            // setup comminucation
            SetupPort();

            // setup login token
            CreateLoginToken();

            // setup login token timer
            createLoginTokenTimer = new Timer(1000 * 60);
            createLoginTokenTimer.Elapsed += OnUpdateLoginToken;
            createLoginTokenTimer.AutoReset = true;
            createLoginTokenTimer.Enabled = true;
        }

        public void Stop()
        {
            Logger.Log("INFO: Server terminated");
        }

        private void OnUpdateLoginToken(Object source, ElapsedEventArgs e)
        {
            CreateLoginToken();
        }

        public void CreateLoginToken()
        {
            // log status
            Logger.Log("INFO: Updating login token");

            // get login data
            string loginDataPath = @"data/account/login.json";
            LoginData loginData = JsonHelper.LoadJson<LoginData>(loginDataPath);

            // calculate timestamp
            double millisecondsSince1970 = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            double tempTimestamp = Math.Floor(millisecondsSince1970 / 1000) + 45;
            loginData.timestamp = (long)tempTimestamp ^ 698464131;

            // convert login data to encoded base64 json
            char[] json = JsonHelper.EncodeTo64(JsonHelper.NormalizeJson<LoginData>(loginData)).ToCharArray();

            // convert encoded base64 json to bytes
            byte[] bytes = new byte[json.Length];
            for (int i = 0; i < json.Length; ++i)
            {
                bytes[i] += (byte)json[i];
            }

            // write login data to registery
            Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\Battlestate Games\\EscapeFromTarkov", "bC5vLmcuaS5u_h1472614626", bytes, RegistryValueKind.Binary);
        }

        public void SetupPort()
        {
            // log status
            Logger.Log("INFO: Setup port");

            // code here
        }
    }
}
