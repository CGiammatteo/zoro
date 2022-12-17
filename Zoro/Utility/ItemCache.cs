using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zoro.Utility
{
    internal class ItemCache
    {
        public static LimitedData.Item GrabCachedItem(long id)
        {
            JObject baseObj = new JObject();
            if (!Directory.Exists(AppContext.BaseDirectory + @"\data"))
            {
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\data");
            }

            if (!File.Exists(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                using (StreamWriter writer = File.CreateText(AppContext.BaseDirectory + @"\data\cache.json"))
                {
                    JObject o1 = new JObject();
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;

                    serializer.Serialize(writer, o1);
                }
            }

            using (StreamReader reader = File.OpenText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    baseObj = (JObject)JToken.ReadFrom(jreader);

                    if (baseObj[Convert.ToString(id)] != null)
                    {
                        LimitedData.Item grabbedItem = new LimitedData.Item();

                        grabbedItem.ItemName = Convert.ToString(baseObj[Convert.ToString(id)]["ItemName"]);
                        grabbedItem.ItemId = id;
                        grabbedItem.Score = Convert.ToInt32(baseObj[Convert.ToString(id)]["Score"]);
                        grabbedItem.RoundedRap = Convert.ToInt32(baseObj[Convert.ToString(id)]["RoundedRap"]);
                        grabbedItem.Value = Convert.ToInt32(baseObj[Convert.ToString(id)]["Value"]);
                        grabbedItem.IsProjected = Convert.ToBoolean(baseObj[Convert.ToString(id)]["IsProjected"]);
                        grabbedItem.AverageSales = Convert.ToInt32(baseObj[Convert.ToString(id)]["AverageSales"]);
                        grabbedItem.LastUpdated = Convert.ToDateTime(baseObj[Convert.ToString(id)]["LastUpdated"]);
                        return grabbedItem;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static void CacheItem(LimitedData.Item item)
        {
            JObject baseObj = new JObject();
            using (StreamReader reader = File.OpenText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    baseObj = (JObject)JToken.ReadFrom(jreader);
                }
            }

            JObject itemObj = new JObject();

            itemObj["ItemName"] = item.ItemName;
            itemObj["Score"] = item.Score;
            itemObj["RoundedRap"] = item.RoundedRap;
            itemObj["Value"] = item.Value;
            itemObj["IsProjected"] = item.IsProjected;
            itemObj["AverageSales"] = item.AverageSales;
            itemObj["LastUpdated"] = item.LastUpdated;

            baseObj.Add(new JProperty(Convert.ToString(item.ItemId), itemObj));

            using (StreamWriter file = File.CreateText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, baseObj);
            }
        }

        public static void UpdateCachedItem(LimitedData.Item item)
        {
            JObject baseObj = new JObject();
            using (StreamReader reader = File.OpenText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    baseObj = (JObject)JToken.ReadFrom(jreader);
                }
            }

            using (StreamWriter file = File.CreateText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                baseObj[Convert.ToString(item.ItemId)]["ItemName"] = item.ItemName;
                baseObj[Convert.ToString(item.ItemId)]["Score"] = item.Score;
                baseObj[Convert.ToString(item.ItemId)]["RoundedRap"] = item.RoundedRap;
                baseObj[Convert.ToString(item.ItemId)]["Value"] = item.Value;
                baseObj[Convert.ToString(item.ItemId)]["IsProjected"] = item.IsProjected;
                baseObj[Convert.ToString(item.ItemId)]["AverageSales"] = item.AverageSales;
                baseObj[Convert.ToString(item.ItemId)]["LastUpdated"] = item.LastUpdated;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, baseObj);
            }
        }

        public static void RefreshSessionCache()
        {
            JObject baseObj = new JObject();
            using (StreamReader reader = File.OpenText(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    baseObj = (JObject)JToken.ReadFrom(jreader);
                }
            }

            int counter = 0;
            foreach (var item in baseObj)
            {
                if (Convert.ToString(item) != null)
                {
                    LimitedData.Item cachedItem = new LimitedData.Item();
                    JValue temp = (JValue)item.Key;
                    string itemId = Convert.ToString(temp);

                    cachedItem.ItemId = Convert.ToInt64(itemId);
                    cachedItem.ItemName = Convert.ToString(baseObj[itemId]["ItemName"]);
                    cachedItem.Score = Convert.ToDouble(baseObj[itemId]["Score"]);
                    cachedItem.RoundedRap = Convert.ToInt32(baseObj[itemId]["RoundedRap"]);
                    cachedItem.Value = Convert.ToInt32(baseObj[itemId]["Value"]);
                    cachedItem.LastUpdated = Convert.ToDateTime(baseObj[itemId]["LastUpdated"]);
                    cachedItem.IsProjected = Convert.ToBoolean(baseObj[itemId]["IsProjected"]);
                    cachedItem.AverageSales = Convert.ToInt32(baseObj[itemId]["AverageSales"]);

                    Settings.CachedItems.Add(cachedItem);
                    counter++;
                }
            }
            Misc.Output.Success($"Refreshed session cache (Items in cache: {counter})!");
        }
    }
}
