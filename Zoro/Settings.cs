using System.Collections.Generic;
using System.Net;

namespace Zoro
{
    internal class Settings
    {
        //STRINGS\\
        public static string Key = "";
        public static string Cookie = "";

        public static string OutboundHook = "";
        public static string InboundHook = "";
        public static string CompletedHook = "";

        public static string Version = "1.0";


        //LONGS\\
        public static long UserId = 0;

        //INTS\\
        public static int SamplePeriod = 30;
        public static int MinumumDailySales = 100;
        public static int ItemRefreshRate = 3; //days
        public static int InboundCheckTime = 60; //minutes
        public static int InventoryRefreshRate = 60; //minutes
        public static int CheckCompletedTradesRate = 10; //minutes
        public static int CheckInboundTradesRate = 30; //minutes

        public static double MinimumProfit = 6;
        public static double MaximumProfit = 15;

        //BOOLS\\
        public static bool ProxyEnabled = true;
        public static bool InboundChecker = true;
        public static bool AntiEggs = true;

        //SESSION ONLY DATA/SETTINGS\\
        public static List<LimitedData.Item> CachedItems = new List<LimitedData.Item>(); //session items are also stored in a cache json file for later session usage
        public static List<long> PlayerCache = new List<long>();
        public static List<Trading.Trade> SentTrades = new List<Trading.Trade>();

        //USER LISTS
        public static List<LimitedData.Item> UserItems = new List<LimitedData.Item>();

        //TRADE QUEUE
        public static List<Trading.Trade> TradeQueue = new List<Trading.Trade>();

        //ITEM STUFF (represented as item id)
        public static List<long> BlacklistedItems = new List<long>();
        public static List<long> NotForTrade = new List<long>();

        //PROXIES\\
        public static List<WebProxy> SessionProxies = new List<WebProxy>();
        public static WebProxy LoadedProxy = null; //use default if proxyenabled is false, use loaded otherwise
        
    }
}
