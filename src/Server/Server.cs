using EFTLauncher.Utility;

namespace EFTLauncher.ServerLogic
{
    /// <summary>
    /// Emulated Escape From Tarkov server
    /// </summary>
    public class Server
    {
        ServerResponseListener serverResponseListener;

        public void Start(string domain)
        {
            Logger.Log("INFO: Server starting");
            serverResponseListener = new ServerResponseListener(domain);
            serverResponseListener.Initialize();
        }

        public void Stop()
        {
            // terminate processes
            serverResponseListener.Terminate();
            Logger.Log("INFO: Server terminated");
        }
    }
}
