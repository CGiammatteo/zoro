using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Zoro.Trading
{
    internal class Sending
    {
        public static void SendTrade(Trade tradeDetails)
        {
            List<long> offerIds = LimitedData.ItemHelper.IdToAssetId(tradeDetails.Offer, Settings.UserId);
            List<long> recieveIds = LimitedData.ItemHelper.IdToAssetId(tradeDetails.Recieve, tradeDetails.TraderId);

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://trades.roblox.com/v1/trades/send");
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
                httpWebRequest.Headers.Add("X-CSRF-TOKEN", Utility.SessionDetails.GrabRegData());
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Proxy = Settings.LoadedProxy;
                httpWebRequest.Credentials = Settings.LoadedProxy.Credentials;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string data = "{\"offers\": [{\"userId\": " + tradeDetails.TraderId + ",\"userAssetIds\": [" + string.Join(",", recieveIds) + "],\"robux\": 0},{\"userId\": " + Settings.UserId + ",\"userAssetIds\": [" + string.Join(",", offerIds) + "],\"robux\": 0},],}";
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        dynamic json = JsonConvert.DeserializeObject(result);
                        tradeDetails.FinalTradeId = Convert.ToInt64(json["id"]);
                        Discord.Webhooks.SendOutbound(tradeDetails);
                        Settings.SentTrades.Add(tradeDetails);
                    }
                }
            }
            catch(Exception ex)
            {
                Misc.Output.Error($"Unable to trade with {tradeDetails.TraderId}");
            }
        }
    }
}
