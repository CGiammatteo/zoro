using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zoro.Trading
{
    internal class Outbound
    {
        public static void MainOutboundLoop()
        {
            Misc.Output.Information("Outboung trading started.");
            //https://inventory.roblox.com/v2/assets/{id}/owners?limit=100
            //get 100 people who have had the limited for like 3 months, then check if they're viable. if they are, compose trade

            while (true)
            {
                for(int i = 0; i < Settings.CachedItems.Count; i++)
                {
                    WebClient wc = new WebClient();
                    string nextPageCursor = "";
                    wc.Proxy = Settings.LoadedProxy;
                    wc.Credentials = Settings.LoadedProxy.Credentials;
                    List<long> Players = new List<long>();

                    try
                    {
                        while(nextPageCursor != null)
                        {
                            dynamic json = JsonConvert.DeserializeObject(wc.DownloadString($"https://inventory.roblox.com/v2/assets/{Settings.CachedItems[i].ItemId}/owners?sortOrder=Asc&limit=100&cursor={nextPageCursor}"));

                            if (json["nextPageCursor"] != null)
                            {
                                nextPageCursor = json["nextPageCursor"];
                            }
                            else
                            {
                                nextPageCursor = null;
                            }

                            for (int x = 0; x < 100; x++)
                            {
                                DateTime dt = Convert.ToDateTime(json["data"][x]["updated"]);
                                if (DateTime.Now.AddYears(-1).Year <= dt.Year)
                                {
                                    if (json["data"][x]["owner"] != null)
                                    {
                                        Misc.Output.Basic($"Grabbed user {json["data"][x]["owner"]["id"]}");
                                        Players.Add(json["data"][x]["owner"]["id"]);
                                    }
                                }
                            }
                        }

                        List<long> goodUsers = Utility.UserData.ViableUser(Players);

                        foreach(long good in goodUsers)
                        {
                            Misc.Output.Success($"Viable trader found: {good}");
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
            }
        }
    }
}
