using EFTLauncher.Utility;

namespace EFTLauncher.ClientLogic
{
    /// <summary>
    /// Emulated Escape From Tarkov client
    /// </summary>
    class Client
    {
        Login login;

        public void Start(string email, string password)
        {
            Logger.Log("INFO: Client starting");
            login = new Login(email, password);
            login.Initialize();
        }

        public void Stop()
        {
            login.Terminate();
            Logger.Log("INFO: Client terminated");
        }
    }
}
