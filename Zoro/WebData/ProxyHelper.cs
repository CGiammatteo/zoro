using System;
using System.Net;

namespace Zoro.WebData
{
    internal class ProxyHelper
    {
        public static bool CheckProxy(WebProxy proxy)
        {
            WebClient wc = new WebClient();

            try
            {
                wc.Proxy = proxy;
                wc.Credentials = proxy.Credentials;
                wc.DownloadString("https://google.com/");
                wc.Dispose();
                return true;
            }
            catch(WebException ex)
            {
                wc.Dispose();
                return false;
            }
        }

        public static void RotateProxy()
        {
            int currentIdx = Settings.SessionProxies.IndexOf(Settings.LoadedProxy);

            try
            {
                Settings.LoadedProxy = Settings.SessionProxies[currentIdx + 1];
            }
            catch (Exception ex)
            {
                Settings.LoadedProxy = Settings.SessionProxies[0];
            }
        }
    }
}
