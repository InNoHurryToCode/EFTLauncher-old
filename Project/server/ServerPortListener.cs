using System.Threading;
using System.Net;
using EFTServer.server.tools;

namespace EFTServer.server
{
    class ServerPortListener
    {
        private volatile string address;    // server address
        private Thread thread;              // request listener thread
        private volatile bool handle;       // thread status

        public ServerPortListener(string domain, int port)
        {
            // get address
            address = string.Format("http://{0}:{1}/", domain, port);
            Logger.Log("INFO: Address:");
            Logger.Log(address);
        }

        public void Initialize()
        {
            if (thread != null)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Initializing port listener");

            // create http listener thread
            handle = true;
            thread = new Thread(ListenerThread);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Terminate()
        {
            if (thread == null || !handle || Thread.CurrentThread == thread)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Joining http listener thread");

            // terminate listener thread
            handle = false;

            if (!thread.Join(1000))
            {
                Logger.Log("ALERT: Thread failed to join, aborting thread");
                thread.Abort();
            }

            thread = null;
        }

        private void ListenerThread()
        {
            // initialize listener
            Logger.Log("INFO: Initializing http listener thread");
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add(address);
            httpListener.Start();

            // listener thread loop
            Logger.Log("INFO: Entering http listener thread loop");
            while (handle)
            {
                HttpListenerContext context = httpListener.GetContext();
            }

            // terminate listener
            httpListener.Stop();
            Logger.Log("INFO: Terminated http listener thread");
        }
    }
}
