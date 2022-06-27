using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            httpWebRequest.Headers.Add("X-CSRF-TOKEN", SessionDetails.GrabRegData()); httpWebRequest.ContentType = "application/json";
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

        public static bool ViableUser(long userid)
        {
            //grab as many people as possible which have last updated said item like 3 months ago. from there, check if they're
            //wghen they were last online. if they were last online max 3 days ago, check if they're a premium member. if they
            //are, then trade with them

            return false;
        }
    }
}
