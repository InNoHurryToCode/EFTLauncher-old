using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using EFTLauncher.Utility;

namespace EFTLauncher.ExtractorLogic
{
    class ServerResponseListener
    {
        private Thread thread;                  // request listener thread
        private volatile bool threadHandle;     // thread status
        private volatile string domain;         // server address

        public ServerResponseListener(string domain)
        {
            // get address
            this.domain = domain;
            Logger.Log("INFO: Server domain: " + this.domain);
        }

        public void Initialize()
        {
            if (thread != null)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Initializing http listener");

            // create http listener thread
            threadHandle = true;
            thread = new Thread(ListenerThread);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Terminate()
        {
            if (thread == null || !threadHandle || Thread.CurrentThread == thread)
            {
                return;
            }

            // log status
            Logger.Log("INFO: Joining http listener thread");

            // terminate listener thread
            threadHandle = false;

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

            // check domain
            string charToCheck = domain.Substring(domain.Length - 1);
            if (charToCheck != @"/")
            {
                domain += @"/";
            }

            // listen to domain
            httpListener.Prefixes.Add(domain);
            httpListener.Start();

            // listener thread loop
            Logger.Log("INFO: Entering http listener thread loop");
            while (threadHandle)
            {
                // dump request
                DumpRequest(httpListener.GetContext());
            }

            // terminate listener
            httpListener.Stop();
            Logger.Log("INFO: Terminated http listener thread");
        }

        private void DumpRequest(HttpListenerContext context)
        {
            // receive request
            Logger.Log("INFO: Recieved request from " + context.Request.RemoteEndPoint.Address + " for " + context.Request.Url);

            // get the data
            byte[] buffer = ZLib.ToByteArray(context.Request.InputStream);

            // decompress the data
            string body = ZLib.Decompress(buffer);
            Logger.Log("INFO: Decompressed body: " + body);

            // save the request
            JsonHelper.SaveJson<string>("extracted" + context.Request.Url + ".json", body);
        }
    }
}
