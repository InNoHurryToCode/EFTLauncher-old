using EFTLauncher.Utility;

namespace EFTLauncher.ClientLogic
{
    /// <summary>
    /// Emulated Escape From Tarkov client
    /// </summary>
    class Client
    {
        Login login;
        GameMonitor gameMonitor;

        public void Start(string email, string password, string gameDirectory, string address)
        {
            // log state
            Logger.Log("INFO: Client starting");

            // initialize login
            login = new Login(email, password);
            login.Initialize();

            // launch the game
            gameMonitor = new GameMonitor(gameDirectory);
            gameMonitor.LaunchGame(address);
        }

        public void Stop()
        {
            login.Terminate();
            Logger.Log("INFO: Client terminated");
        }
    }
}
