using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Zoro.Utility
{
    internal class Users
    {
        public static long FindPlayer()
        {
            long playerId = 0;
            Random random = new Random();

            //web setup
            switch (1) //random.Next(1,3) //change this for an update, not for initial release since there is no need
            {
                case 1:
                    //finding player via item owner
                    return Method1();
                    break;
                case 2:
                    //finding player via item item owner history
                    return Method2();
                    break;
                case 3:
                    //finding player via group
                    break;
                default:
                    break;
            }

            return playerId;
        }

        private static long Method1()
        {
            long playerId = 0;
            Random random = new Random();
            int randomItem = random.Next(0, Settings.CachedItems.Count);
            string nextPageCursor = "";

            while (nextPageCursor != null)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://inventory.roblox.com/v2/assets/{Settings.CachedItems[randomItem].ItemId}/owners?sortOrder=Asc&limit=100&cursor={nextPageCursor}");
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

                        if (json["nextPageCursor"] != null)
                        {
                            nextPageCursor = json["nextPageCursor"];
                        }
                        else
                        {
                            nextPageCursor = null;
                        }

                        for (int i = 0; i < 100; i++)
                        {
                            if (json["data"][i] != null)
                            {
                                string time = json["data"][i]["updated"]; 
                                DateTime dt = Convert.ToDateTime(time);

                                if (json["data"][i]["owner"] != null && Convert.ToString(json["data"][i]["owner"]["id"]) != "1" && dt.Year == DateTime.Now.Year)
                                {
                                    playerId = Convert.ToInt64(json["data"][i]["owner"]["id"]);
                                    if (UserData.ViableUser(playerId)) //also check if user is currently in queue
                                    {
                                        return playerId;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if ((int)ex.Status == 429 || (int)ex.Status == 401 || (int)ex.Status == 400)
                    {
                        WebData.ProxyHelper.RotateProxy();
                        Misc.Output.Basic("Rotated proxy.");
                        Method1();
                    }
                }
            }
            return playerId;
        }

        private static long Method2() //not working, needs to be fixed
        {
            long playerId = 0;
            Random random = new Random();
            //finding player via item owner
            int randomItem = random.Next(0, Settings.CachedItems.Count);
            string nextPageCursor = "";

            while (nextPageCursor != null)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://rblx.trade/api/v2/asset/{randomItem}/ownership-history?limit=100&cursor={nextPageCursor}");
                    httpWebRequest.Method = "GET";
                    //httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
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

                        for (int i = 0; i < array.Count; i++)
                        {
                            playerId = Convert.ToInt64(array[i]["userId"]);
                            if (UserData.ViableUser(playerId)) //also check if user is currently in queue
                            {
                                return playerId;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if ((int)ex.Status == 429 || (int)ex.Status == 401 || (int)ex.Status == 400)
                    {
                        WebData.ProxyHelper.RotateProxy();
                        Misc.Output.Basic("Rotated proxy.");
                        Method2();
                    }
                }
            }
            return playerId;
        }
    }
}
