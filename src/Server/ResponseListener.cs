using System;
using System.IO;
using System.Net;
using System.Text;
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
        private volatile string domain;     // server address
        private Thread thread;              // request listener thread
        private volatile bool threadHandle; // thread status

        Regex assort = new Regex(@"/client/trading/api/getTraderAssort/([a-z0-9])+"); 
        Regex prices = new Regex(@"/client/trading/api/getUserAssortPrice/([a-z0-9])+");
        Regex getTrader = new Regex(@"/client/trading/api/getTrader/");
        Regex traderImg = new Regex(@"/files/([a-z0-9/\.jpng])+");
        Regex content = new Regex(@"/uploads/([a-z0-9/\.jpng_])+");
        Regex pushNotifier = new Regex(@"/push/notifier/get/");

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
                HttpListenerContext context = httpListener.GetContext();

                // receive request
                HttpListenerRequest request = context.Request;
                string requestURl;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        requestURl = readStream.ReadToEnd();
                    }
                }
                Logger.Log("INFO: Recieved request from " + request.Url);

                // get response to request
                string responseData = GetResponseText(requestURl);

                // initiaize response response
                HttpListenerResponse response = context.Response;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseData);
                response.ContentLength64 = buffer.Length;

                // send response
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            // terminate listener
            httpListener.Stop();
            Logger.Log("INFO: Terminated http listener thread");
        }

        private string GetResponseText(string requestURL)
        {
            string output = "";

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

            switch (requestURL)
            {
                case "/":
                    output = "Escape From Tarkov server for version 0.11.7.3233 by Merijn Hendriks";
                    break;

                case "/client/friend/list":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/friends.json");
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
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nullarray.json");
                    break;

                case "/client/friend/request/list/outbox":
                case "/client/friend/request/list/inbox":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nullarray.json");
                    break;

                case "/client/languages":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/languages.json");
                    break;

                case "/client/menu/locale/en":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/english/list.json");
                    break;

                case "/client/menu/locale/ru":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/list.json");
                    break;

                case "/client/game/version/validate":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nulldata.json");
                    break;

                case "/client/game/login":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/login.json");
                    break;

                case "/client/items":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/items.json");
                    break;

                case "/client/globals":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/globals.json");
                    break;

                case "/client/game/profile/list":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/list.json");
                    break;

                case "/client/game/profile/select":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/select.json");
                    break;

                case "/client/profile/status":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/profile/status.json");
                    break;

                case "/client/game/keepalive":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/empty/nulldata.json");
                    break;

                case "/client/weather":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/weathers.json");
                    break;

                case "/client/locale/en":
                case "/client/locale/En":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/locale.json");
                    break;

                case "/client/locale/ru":
                case "/client/locale/Ru":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/localization/russian/locale.json");
                    break;

                case "/client/locations":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/locations.json");
                    break;

                case "/client/handbook/templates":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/templates.json");
                    break;

                case "/client/quest/list":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/quests.json");
                    break;

                case "/client/game/bot/generate":
                    // TODO: randomized bots
                    //output = JSON.stringify( { "err": 0,"errmsg": null,"data": generateBots(JSON.parse(body)) } );
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/bots.json");
                    break;

                case "/client/trading/api/getTradersList":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/traders/tradelist.json");
                    break;

                case "/client/server/list":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/server/server.json");
                    break;

                case "/client/ragfair/search":
                    output = JsonHelper.ReadJson(Environment.CurrentDirectory + @"/data/ragfair/search.json");
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
                    output = "UNHANDLED REQUEST " + requestURL;
                    break;
            }

            return output;
        }
    }
}
