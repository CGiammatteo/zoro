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

            try
            {
                dynamic json = JsonConvert.DeserializeObject(client.DownloadString($"https://users.roblox.com/v1/users/{userid}"));

                return Convert.ToString(json["name"]);
            }
            catch(Exception ex)
            {
                Misc.Output.Error("Unable to grab username!");
                return "";
            }
        }
    }
}
