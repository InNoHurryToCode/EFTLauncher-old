using System;

namespace EFTLauncher.ClientData
{
    /// <summary>
    /// Contains the login data for the Escape From Tarkov client
    /// </summary>
    public struct LoginData
    {
        public string email;    // login email
        public string password; // login password
        public bool toggle;     // login successful
        public long timestamp;  // login timestamp

        public override string ToString()
        {
            return "email: " + email + Environment.NewLine
                 + "password: " + password + Environment.NewLine
                 + "toggle: " + toggle + Environment.NewLine
                 + "timestamp: " + timestamp;
        }
    }
}
