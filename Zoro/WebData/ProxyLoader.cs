using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Zoro.WebData
{
    internal class ProxyLoader
    {
        public static void LoadProxiesFromFile()
        {
            if (Settings.ProxyEnabled)
            {
                if(!File.Exists(AppContext.BaseDirectory + @"Proxies.txt"))
                {
                    File.CreateText(AppContext.BaseDirectory + @"Proxies.txt");
                    Misc.Output.Error("Proxies file created, get proxies from https://www.webshare.io/?referral_code=u40wlib00q5c, then try again. CLosing in 10 seconds...");
                    Thread.Sleep(10000);
                    Environment.Exit(0);
                }

                int total = 0;
                Misc.Output.Basic("Checking proxies (This process can take a few minutes depending on how many proxies you are using)...");
                foreach(string line in File.ReadLines(AppContext.BaseDirectory + @"Proxies.txt"))
                {
                    string[] words = line.Split(':');
                    WebProxy newProx = new WebProxy(words[0], Convert.ToInt32(words[1]));
                    newProx.BypassProxyOnLocal = false;
                    newProx.UseDefaultCredentials = false;
                    newProx.Credentials = new NetworkCredential(words[2], words[3]);

                    if (ProxyHelper.CheckProxy(newProx))
                    {
                        //Misc.Output.Basic($"{Convert.ToString(newProx.Address)}");
                        Settings.SessionProxies.Add(newProx);
                        total++;
                    }
                }
                Settings.LoadedProxy = Settings.SessionProxies[0];
                Misc.Output.Information($"Total proxies loaded: {total}");
            }
            else
            {
                Settings.LoadedProxy = WebProxy.GetDefaultProxy();
            }
        }
    }
}
