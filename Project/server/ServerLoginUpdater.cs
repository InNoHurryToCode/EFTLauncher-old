using System;
using System.Timers;
using Microsoft.Win32;
using EFTServer.server.data;
using EFTServer.server.tools;

namespace EFTServer.server
{
    class ServerLoginUpdater
    {
        private Timer createLoginTokenTimer;    // login token renewer

        public void Initialize()
        {
            if (createLoginTokenTimer != null)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Initializing login token updater");

            // create login token
            CreateLoginToken();

            // schedule login token update
            createLoginTokenTimer = new Timer(1000 * 60);
            createLoginTokenTimer.Elapsed += OnUpdate;
            createLoginTokenTimer.AutoReset = true;
            createLoginTokenTimer.Enabled = true;
        }

        public void Terminate()
        {
            if (createLoginTokenTimer == null)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Terminating login token updater");

            // terminate login token update
            createLoginTokenTimer.Enabled = false;
            createLoginTokenTimer = null;
        }

        private void OnUpdate(Object source, ElapsedEventArgs e)
        {
            CreateLoginToken();
        }

        private void CreateLoginToken()
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
    }
}
