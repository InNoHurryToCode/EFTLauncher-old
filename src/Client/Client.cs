using EFTLauncher.Utility;

namespace EFTLauncher.ClientLogic
{
    /// <summary>
    /// Emulated Escape From Tarkov client
    /// </summary>
    class Client
    {
        ClientLogin clientLogin;

        public void Start(string email, string password)
        {
            Logger.Log("INFO: Client starting");
            clientLogin = new ClientLogin(email, password);
            clientLogin.Initialize();
        }

        public void Stop()
        {
            clientLogin.Terminate();
            Logger.Log("INFO: Client terminated");
        }
    }
}
