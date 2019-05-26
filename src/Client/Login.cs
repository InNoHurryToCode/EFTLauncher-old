using System;
using System.Timers;
using Microsoft.Win32;
using EFTLauncher.ClientData;
using EFTLauncher.Utility;

namespace EFTLauncher.ClientLogic
{
    /// <summary>
    /// Login token updater. Normally the BattleState Game Launcher creates this, but we want to run the game without it.
    /// TODO: split the login token updater from the launcher and make a custom one.
    /// </summary>
    class ClientLogin
    {
        private Timer createLoginTokenTimer;    // login token renewer
        LoginData loginData;                    // login information

        public ClientLogin(string email, string password)
        {
            // set login info
            loginData.email = email;
            loginData.password = password;

            // log status
            Logger.Log("INFO: Login email: " + loginData.email);
            Logger.Log("INFO: Login password: " + loginData.password);
        }

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

            // calculate timestamp
            double millisecondsSince1970 = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            loginData.timestamp = (long)(Math.Floor(millisecondsSince1970 / 1000) + 45) ^ 698464131;
            Logger.Log("INFO: Login timestamp: " + loginData.timestamp);

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
