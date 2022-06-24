using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    Misc.Output.Error("Proxies file created, get proxies from https://webshare.io, then try again.");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }

                int total = 0;
                Misc.Output.Basic("Checking proxies...");
                foreach(string line in File.ReadLines(AppContext.BaseDirectory + @"Proxies.txt"))
                {
                    string[] words = line.Split(':');
                    WebProxy newProx = new WebProxy(words[0], Convert.ToInt32(words[1]));


                    newProx.Credentials = new NetworkCredential(words[2], words[3]);

                    if (ProxyHelper.CheckProxy(newProx))
                    {
                        Settings.LoadedProxy = newProx;
                        total++;
                    }
                }
                Misc.Output.Information($"Total proxies loaded: {total}");
            }
            else
            {
                Settings.LoadedProxy = WebProxy.GetDefaultProxy();
            }
        }
    }
}
