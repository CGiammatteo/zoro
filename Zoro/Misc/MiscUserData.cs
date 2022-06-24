using Newtonsoft.Json;
using System;
using System.Net;

namespace Zoro.Misc
{
    internal class MiscUserData
    {
        public static string GrabUsername(long userid)
        {
            WebClient client = new WebClient();
            client.Proxy = Settings.LoadedProxy;
            client.Credentials = Settings.LoadedProxy.Credentials;

            try
            {
                dynamic json = JsonConvert.DeserializeObject(client.DownloadString($"https://users.roblox.com/v1/users/{userid}"));
                client.Dispose();
                return Convert.ToString(json["name"]);
            }
            catch(WebException ex)
            {
                if ((int)ex.Status == 429 || (int)ex.Status == 401)
                {
                    WebData.ProxyHelper.RotateProxy();
                    Misc.Output.Basic("Rotated proxy.");
                }

                client.Dispose();
                Misc.Output.Error("Unable to grab username!");
                return "";
            }
        }
    }
}
