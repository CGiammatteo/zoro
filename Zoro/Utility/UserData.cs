using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Zoro.Utility
{
    internal class UserData
    {
        public static List<long> GrabPlayerItems(long userid)
        {
            List<long> items = new List<long>();

            WebClient wc = new WebClient();
            wc.Proxy = Settings.LoadedProxy;
            wc.Credentials = Settings.LoadedProxy.Credentials;
            try
            {
                dynamic json = JsonConvert.DeserializeObject(wc.DownloadString($"https://inventory.roblox.com/v1/users/{userid}/assets/collectibles?sortOrder=Asc&limit=100"));

                JArray array = (JArray)json["data"];

                for (int i = 0; i < array.Count; i++)
                {
                    items.Add(Convert.ToInt64(array[i]["assetId"].ToString()));
                }

                wc.Dispose();
                return items;
            }
            catch (WebException ex)
            {
                if ((int)ex.Status == 429 || (int)ex.Status == 401)
                {
                    WebData.ProxyHelper.RotateProxy();
                    Misc.Output.Basic("Rotated proxy.");
                }

                Misc.Output.Error($"Unable to grab {userid}'s items!");
                wc.Dispose();
                return items;
            }
        }

        public static bool IsPremium(long userid)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://premiumfeatures.roblox.com/v1/users/{userid}/validate-membership");
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, string.Format(".ROBLOSECURITY={0}", Settings.Cookie));
            httpWebRequest.Headers.Add("X-CSRF-TOKEN", SessionDetails.GrabRegData()); 
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Proxy = Settings.LoadedProxy;
            httpWebRequest.Credentials = Settings.LoadedProxy.Credentials;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                try
                {
                    return Convert.ToBoolean(result);
                }
                catch(WebException ex)
                {
                    if ((int)ex.Status == 429 || (int)ex.Status == 401)
                    {
                        WebData.ProxyHelper.RotateProxy();
                        Misc.Output.Basic("Rotated proxy.");
                    }
                    return false;
                }
            }
        }

        public static List<long> ViableUser(List<long> ids)
        {
            List<long> users = new List<long>();
            foreach (long id in ids)
            {
                Misc.Output.Basic(Convert.ToString(id));
                //https://api.roblox.com/users/78035609/onlinestatus/
                try
                {
                    WebClient wc = new WebClient();
                    wc.Proxy = Settings.LoadedProxy;
                    wc.Credentials = Settings.LoadedProxy.Credentials;

                    dynamic json = JsonConvert.DeserializeObject(wc.DownloadString($"https://api.roblox.com/users/{id}/onlinestatus/"));

                    DateTime lastOnline = Convert.ToDateTime(json["LastOnline"]);
                    TimeSpan span = lastOnline.Subtract(DateTime.Now);
                    int days = (int)span.TotalDays;

                    if (lastOnline.Year == DateTime.Now.Year && days <= 7)
                    {
                        if (IsPremium(id))
                            users.Add(id);
                    }
                }
                catch(WebException ex)
                {
                    if ((int)ex.Status == 429 || (int)ex.Status == 401)
                    {
                        WebData.ProxyHelper.RotateProxy();
                        Misc.Output.Basic("Rotated proxy.");
                    }
                }
            }
            return users;
        }
    }
}
