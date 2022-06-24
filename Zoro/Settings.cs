using System.Collections.Generic;

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
        //add a proxy thing here

        //SESSION ONLY DATA/SETTINGS\\
        public static List<LimitedData.Item> CachedItems = new List<LimitedData.Item>(); //session items are also stored in a cache json file
        public static List<LimitedData.Item> UserItems = new List<LimitedData.Item>();

        //PROXIES\\
        public static List<string> SessionProxies = new List<string>();
        public static string LoadedProxy = "";
        
    }
}
