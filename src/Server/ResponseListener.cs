using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using EFTLauncher.Utility;

namespace EFTLauncher.ServerLogic
{
    /// <summary>
    /// TODO: handle IP address
    /// </summary>
    class ServerResponseListener
    {
        private Thread thread;                  // request listener thread
        private volatile bool threadHandle;     // thread status
        private volatile string domain;         // server address
        private volatile string finalOutput;    // command to send

        // regex for output
        private Regex assort = new Regex(@"/client/trading/api/getTraderAssort/([a-z0-9])+");
        private Regex prices = new Regex(@"/client/trading/api/getUserAssortPrice/([a-z0-9])+");
        private Regex getTrader = new Regex(@"/client/trading/api/getTrader/");
        private Regex traderImg = new Regex(@"/files/([a-z0-9/\.jpng])+");
        private Regex content = new Regex(@"/uploads/([a-z0-9/\.jpng_])+");
        private Regex pushNotifier = new Regex(@"/push/notifier/get/");

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
                // handle request
                HttpListenerContext context = httpListener.GetContext();
                string requestedData = GetRequest(context);
                SendResponse(context, requestedData);
            }

            // terminate listener
            httpListener.Stop();
            Logger.Log("INFO: Terminated http listener thread");
        }

        private string GetRequest(HttpListenerContext context)
        {
            // receive request
            Logger.Log("INFO: Recieved request from " + context.Request.RemoteEndPoint.Address + " for " + context.Request.Url);

            // get data
            byte[] buffer = ZLib.ToByteArray(context.Request.InputStream);

            // decompress the data
            string body = ZLib.Decompress(buffer);
            Logger.Log("INFO: Decompressed body: " + body);

            return body;
        }

        private void SendResponse(HttpListenerContext context, string body)
        {
            // get response type
            if (context.Request.HttpMethod == "POST")
            {
                Logger.Log("INFO: Request is POST");
            }
            else
            {
                Logger.Log("INFO: Request is GET");
            }

            // get response to send
            byte[] buffer = ZLib.Compress(GetResponseBody(body));

            // send response
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        private string GetResponseBody(string body)
        {
            //string info = "{}"; // movement info

            #region psuedo code
            /*if (url.match(assort))
            {
                FinalOutput = ReadJson("assort/" + url.substring(36).replace(/[^a - zA - Z0 - 9_] / g, '') + ".json");
                return;
            }
            
            if (url.match(prices))
            {
                FinalOutput = ReadJson("prices/" + url.substring(46).replace(/[^a - zA - Z0 - 9_] / g, '') + ".json"); // thats some budget ass shit
                return;
            }
            
            if (url.match(getTrader))
            {
                console.log(url.substring(30));
                FinalOutput = '{"err":0, "errmsg":null, "data":{"_id":"' + url.substring(30) + '", "working":true, "name":"ez", "surname":"ez", "nickname":"ez", "location":"", "avatar":"/files/trader/avatar/59b91ca086f77469a81232e4.jpg", "balance_rub":80000000, "balance_dol":80000000, "balance_eur":80000000, "display":true, "discount":1337, "discount_end":0, "buyer_up":false, "currency":"RUB", "supply_next_time":1551040000, "repair":{"availability":true, "quality":"1.2", "excluded_id_list":[], "excluded_category":[], "currency":"5449016a4bdc2d6f028b456f", "currency_coefficient":1, "price_rate":0}, "insurance":{"availability":true, "min_payment":0, "min_return_hour":24, "max_return_hour":36, "max_storage_time":72, "excluded_category":[]}, "gridHeight":1000, "loyalty":{"currentLevel":1337, "currentStanding":1337, "currentSalesSum":1337, "loyaltyLevels":{"0":{"minLevel":1, "minSalesSum":0, "minStanding":0}, "1":{"minLevel":1, "minSalesSum":1, "minStanding":1}, "2":{"minLevel":1, "minSalesSum":1, "minStanding":1}, "3":{"minLevel":1, "minSalesSum":1, "minStanding":1}}}, "sell_category":[]}}';
                return;
            }
            
            if (url.match(traderImg) || url.match(content))
            {
                FinalOutput = "DEAD";
                return;
            }

            if (url.match(pushNotifier))
            {
                FinalOutput = '{"err":0, "errmsg":null, "data":[]}';
                return;
            }*/
            #endregion

            switch (body)
            {
                case "/":
                    finalOutput = "Escape From Tarkov server for version 0.11.7.3233 by Merijn Hendriks";
                    break;

                case "/client/friend/list":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/friends.json");
                    break;

                case "/client/game/profile/items/moving":
                    /*for (var a = 0; a < info.data.length; a++)
                    {
                        handleMoving(info.data[a]);
                    }

                    if (FinalOutput == "OK")
                    {
                        FinalOutput = JSON.stringify(ItemOutput);
                    }*/
                    break;

                case "/client/mail/dialog/list":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nullarray.json");
                    break;

                case "/client/friend/request/list/outbox":
                case "/client/friend/request/list/inbox":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nullarray.json");
                    break;

                case "/client/languages":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/languages.json");
                    break;

                case "/client/menu/locale/en":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/english/list.json");
                    break;

                case "/client/menu/locale/ru":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/list.json");
                    break;

                case "/client/game/version/validate":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nulldata.json");
                    break;

                case "/client/game/login":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/login.json");
                    break;

                case "/client/items":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/items.json");
                    break;

                case "/client/globals":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/globals.json");
                    break;

                case "/client/game/profile/list":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/list.json");
                    break;

                case "/client/game/profile/select":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/select.json");
                    break;

                case "/client/profile/status":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/status.json");
                    break;

                case "/client/game/keepalive":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nulldata.json");
                    break;

                case "/client/weather":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/weathers.json");
                    break;

                case "/client/locale/en":
                case "/client/locale/En":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/locale.json");
                    break;

                case "/client/locale/ru":
                case "/client/locale/Ru":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/locale.json");
                    break;

                case "/client/locations":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/locations.json");
                    break;

                case "/client/handbook/templates":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/templates.json");
                    break;

                case "/client/quest/list":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/quests.json");
                    break;

                case "/client/game/bot/generate":
                    // TODO: randomized bots
                    //output = JSON.stringify( { "err": 0,"errmsg": null,"data": generateBots(JSON.parse(body)) } );
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/bots.json");
                    break;

                case "/client/trading/api/getTradersList":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/traders/tradelist.json");
                    break;

                case "/client/server/list":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/server/server.json");
                    break;

                case "/client/ragfair/search":
                    finalOutput = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/ragfair/search.json");
                    break;

                case "/dump":
                    // TODO: randomized bots
                    /*var presetExtended = JSON.parse(ReadJson("bots/presetExtended.json"));
                    presetExtended.data.forEach(pres)
                    { 
                        if (pres._items[0]._tpl == "5aafa857e5b5b00018480968")
                        {
                            console.log(pres._id);
                        }
                    }*/
                    break;

                default:
                    finalOutput = "UNHANDLED REQUEST " + body;
                    break;
            }

            return finalOutput;
        }
    }
}
