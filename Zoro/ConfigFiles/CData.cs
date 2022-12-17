using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Zoro.ConfigFiles
{
    internal class CData
    {
        public static void LoadConfigData()
        {
            string path = AppContext.BaseDirectory + "Config.json";

            if (!File.Exists(path))
            {
                Misc.Output.Error("Config file not found! Creating one now...");
                try
                {
                    JObject o1 = new JObject();
                    using (StreamWriter file = File.CreateText(path))
                    {
                        string[] temp = new string[0];
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;

                        o1["Key"] = Settings.Key;
                        o1["Cookie"] = Settings.Cookie;
                        o1["UserId"] = Settings.UserId;

                        o1["OutboundWebhook"] = Settings.OutboundHook;
                        o1["InboundWebhook"] = Settings.InboundHook;
                        o1["CompletedWebhook"] = Settings.CompletedHook;

                        o1["SamplePeriod"] = Settings.SamplePeriod;
                        o1["MinimumDailySales"] = Settings.MinumumDailySales;
                        o1["ItemRefreshRate"] = Settings.ItemRefreshRate;
                        o1["InventoryRefreshRate"] = Settings.InventoryRefreshRate;
                        o1["InboundCheckTime"] = Settings.InboundCheckTime;
                        o1["CompletedTradesTimeCheck"] = Settings.CheckCompletedTradesRate;
                        o1["CheckInboundTradesRate"] = Settings.CheckInboundTradesRate;

                        o1["MinimumProfit"] = Settings.MinimumProfit;
                        o1["MaximumProfit"] = Settings.MaximumProfit;

                        o1["BlacklistedItems"] = JsonConvert.SerializeObject(temp);
                        o1["NotForTrade"] = JsonConvert.SerializeObject(temp);
                        o1["InboundChecker"] = Settings.InboundChecker;
                        o1["AntiEggs"] = Settings.AntiEggs;

                        serializer.Serialize(file, o1);
                    }
                    Misc.Output.Success("Created config successfully! Closing in 5 seconds...");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Misc.Output.Error("Unable to create a config, closing in 5 seconds...");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
            }

            try
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    using (JsonReader jreader = new JsonTextReader(reader))
                    {
                        JObject o1 = (JObject)JToken.ReadFrom(jreader);

                        Settings.Key = Convert.ToString(o1["Key"]);
                        Settings.Cookie = Convert.ToString(o1["Cookie"]);
                        Settings.UserId = Convert.ToInt64(o1["UserId"]);

                        Settings.OutboundHook = Convert.ToString(o1["OutboundWebhook"]);
                        Settings.InboundHook = Convert.ToString(o1["InboundWebhook"]);
                        Settings.CompletedHook = Convert.ToString(o1["CompletedWebhook"]);

                        Settings.SamplePeriod = Convert.ToInt32(o1["SamplePeriod"]);
                        Settings.MinumumDailySales = Convert.ToInt32(o1["MinumunDailySales"]);
                        Settings.ItemRefreshRate = Convert.ToInt32(o1["ItemRefreshRate"]);
                        Settings.InventoryRefreshRate = Convert.ToInt32(o1["InventoryRefreshRate"]);
                        Settings.InboundCheckTime = Convert.ToInt32(o1["InboundCheckTime"]);
                        Settings.CheckCompletedTradesRate = Convert.ToInt32(o1["CompletedTradesTimeCheck"]);
                        Settings.CheckInboundTradesRate = Convert.ToInt32(o1["CheckInboundTradesRate"]);
                        Settings.MinimumProfit = Convert.ToDouble(o1["MinimumProfit"]);
                        Settings.MaximumProfit = Convert.ToDouble(o1["MaximumProfit"]);

                        foreach(var id in (JArray)JsonConvert.DeserializeObject(Convert.ToString(o1["BlacklistedItems"])))
                        {
                            Settings.BlacklistedItems.Add(Convert.ToInt64(id));
                        }

                        foreach (var id in (JArray)JsonConvert.DeserializeObject(Convert.ToString(o1["NotForTrade"])))
                        {
                            Settings.NotForTrade.Add(Convert.ToInt64(id));
                        }

                        Settings.InboundChecker = Convert.ToBoolean(o1["InboundChecker"]);
                        Settings.AntiEggs = Convert.ToBoolean(o1["AntiEggs"]);

                        Misc.Output.Success("Data grabbed from config file successfully!");
                    }
                }
            }
            catch(Exception ex)
            {
                Misc.Output.Error("Unable to read from your config file! Closing in 5 seconds...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }
    }
}