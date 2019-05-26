using System;

namespace EFTServer.server.data
{
    public struct LoginData
    {
        public string email;
        public string password;
        public bool toggle;
        public double timestamp;

        public override string ToString()
        {
            return "email: " + email + Environment.NewLine
                 + "password: " + password + Environment.NewLine
                 + "toggle: " + toggle + Environment.NewLine
                 + "timestamp: " + timestamp;
        }
    }
}
