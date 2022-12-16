using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Timers;

namespace Zoro.Trading
{
    internal class Completed
    {
        public static void SetupCompleted()
        {
            Timer timer = new Timer(600000); //10 minutes
            timer.Elapsed += new ElapsedEventHandler(OnTime);
            timer.Start();
        }

        private static void OnTime(object source, ElapsedEventArgs e)
        {
            string nextPageCursor = "";

            while (nextPageCursor != null)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://trades.roblox.com/v1/trades/Completed?sortOrder=Asc&limit=100&cursor={nextPageCursor}");
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
                    //httpWebRequest.Headers.Add("X-CSRF-TOKEN", SessionDetails.GrabRegData());
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                    httpWebRequest.Proxy = Settings.LoadedProxy;
                    httpWebRequest.Credentials = Settings.LoadedProxy.Credentials;

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        dynamic json = JsonConvert.DeserializeObject(result);
                        JArray array = (JArray)json["data"];

                        if (json["nextPageCursor"] != null)
                        {
                            nextPageCursor = json["nextPageCursor"];
                        }
                        else
                        {
                            nextPageCursor = null;
                        }

                        List<int> Indexes = new List<int>();
                        for (int i = 0; i < 100; i++)
                        {
                            foreach(Trade trade in Settings.SentTrades)
                            {
                                if (Convert.ToString(array[i]["id"]) == Convert.ToString(trade.FinalTradeId))
                                {
                                    Indexes.Add(Settings.SentTrades.IndexOf(trade));
                                    Misc.Output.Success($"Trade {trade.FinalTradeId} has been completed!");
                                    Discord.Webhooks.SendCompleted(trade);
                                }
                            }
                        }

                        foreach(int idx in Indexes)
                        {
                            Settings.SentTrades.RemoveAt(idx);
                        }

                    }
                }
                catch(WebException ex)
                {
                    //ignore
                }
            }

        }
    }
}
