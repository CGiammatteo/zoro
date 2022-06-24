using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            catch (Exception ex)
            {
                Misc.Output.Error($"Unable to grab {userid}'s items!");
                wc.Dispose();
                return items;
            }
        }
    }
}
