using System.Collections.Generic;
using System.Net;

namespace Zoro
{
    internal class Settings
    {
        //STRINGS\\
        public static string Key = "";
        public static string Cookie = "";

        //LONGS\\
        public static long UserId = 0;

        //INTS\\
        public static int SamplePeriod = 30;
        public static int MinumumDailySales = 20;

        //BOOLS\\
        public static bool ProxyEnabled = true;

        //SESSION ONLY DATA/SETTINGS\\
        public static List<LimitedData.Item> CachedItems = new List<LimitedData.Item>(); //session items are also stored in a cache json file
        public static List<LimitedData.Item> UserItems = new List<LimitedData.Item>();

        //PROXIES\\
        public static List<WebProxy> SessionProxies = new List<WebProxy>();
        public static WebProxy LoadedProxy = null; //use default if proxyenabled is false, use loaded otherwise
        
    }
}
