using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace Zoro.LimitedData
{
    internal class ItemHelper
    {
        public static Item FindItemByName(string name)
        {
            foreach(Item item in Settings.CachedItems)
            {
                if(item.ItemName == name)
                {
                    return item;
                }
            }
            return null;
        }

        public static Item FindItemById(long id)
        {
            foreach (Item item in Settings.CachedItems)
            {
                if (item.ItemId == id)
                {
                    return item;
                }
            }
            return null;
        }

        public static int[][] GrabResaleData(long itemId)
        {
            List<int> prices = new List<int>();
            List<int> sales = new List<int>();

            WebClient wc = new WebClient();
            wc.Proxy = Settings.LoadedProxy;
            wc.Credentials = Settings.LoadedProxy.Credentials;

            try
            {
                dynamic json = JsonConvert.DeserializeObject(wc.DownloadString(string.Format("https://economy.roblox.com/v1/assets/{0}/resale-data", itemId)));

                JArray priceArray = (JArray)json["priceDataPoints"];
                JArray salesArray = (JArray)json["volumeDataPoints"];

                for (int i = 0; i < priceArray.Count; i++)
                {
                    prices.Add(Convert.ToInt32(priceArray[i]["value"].ToString()));
                }

                for (int i = 0; i < salesArray.Count; i++)
                {
                    sales.Add(Convert.ToInt32(salesArray[i]["value"].ToString()));
                }

                int[][] sentArr = {
                    prices.ToArray(),
                    sales.ToArray()
            };

                wc.Dispose();

                return sentArr;
            }
            catch (WebException ex)
            {
                if((int)ex.Status == 429 || (int)ex.Status == 401)
                {
                    WebData.ProxyHelper.RotateProxy();
                    Misc.Output.Basic("Rotated proxy.");
                }
                Misc.Output.Error("Could not grab resale data for " + itemId);
                wc.Dispose();
                return null;
            }
        }

        public static int[] AverageItemData(long itemId)
        {
            //1st item in array is the average price, second in array is average sales

            int[][] data = GrabResaleData(itemId);

            int rapAvg = 0;
            int salesAvg = 0;

            if (data == null)
            {
                int[] endData = { -1 };
                return endData;
            }

            try
            {
                for (int i = 0; i < Settings.SamplePeriod; i++)
                {
                    rapAvg += data[0][i];
                    salesAvg += data[1][i];
                }

                rapAvg = rapAvg / Settings.SamplePeriod;
                salesAvg = salesAvg / Settings.SamplePeriod;

                int[] endData = new int[] { rapAvg, salesAvg };

                return endData;
            }
            catch (Exception ex)
            {
                int[] endData = new int[] { rapAvg, salesAvg };
                return endData;
            }
        }
        
        public static bool CustomProjectedDetection(int roundedRap, int normalRap)
        {
            float percentage = (100 * (Math.Abs(roundedRap - normalRap) / ((roundedRap + normalRap) / 2)));

            if(percentage >= 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Item CreateItemObject(long id)
        {
            Item item = Utility.ItemCache.GrabCachedItem(id);

            if(item == null)
            {
                item = new Item();
                item.ItemName = Rolimons.RoliHelper.GrabItemName(id);
                item.ItemId = id;
                item.Value = Rolimons.RoliHelper.GrabItemValue(id);

                int[] data = AverageItemData(id);

                if (CustomProjectedDetection(data[0], Rolimons.RoliHelper.GrabItemRap(id)) == true)
                {
                    if (data[0] < Rolimons.RoliHelper.GrabItemRap(id))
                    {
                        item.RoundedRap = data[0];
                    }
                    else
                    {
                        item.RoundedRap = Rolimons.RoliHelper.GrabItemRap(id);
                    }
                }
                else
                {
                    item.RoundedRap = data[0];
                }
                item.AverageSales = data[1];
                item.Score = ItemScoring.Score(id, data[0], data[1]);
                item.LastUpdated = DateTime.Now;

                Utility.ItemCache.CacheItem(item);

                return item;
            }
            else
            {
                TimeSpan span = DateTime.Now - item.LastUpdated;
                if((int)span.TotalDays >= Settings.ItemRefreshRate) //could be problomatic, idk at this point (ik i spelled problematic wrong stfu)
                {
                    Misc.Output.Basic($"Updating data for {item.ItemId}");

                    item = new Item();
                    item.ItemName = Rolimons.RoliHelper.GrabItemName(id);
                    item.ItemId = id;
                    item.Value = Rolimons.RoliHelper.GrabItemValue(id);

                    int[] data = AverageItemData(id);

                    if (CustomProjectedDetection(data[0], Rolimons.RoliHelper.GrabItemRap(id)) == true)
                    {
                        if (data[0] < Rolimons.RoliHelper.GrabItemRap(id))
                        {
                            item.RoundedRap = data[0];
                        }
                        else
                        {
                            item.RoundedRap = Rolimons.RoliHelper.GrabItemRap(id);
                        }
                    }
                    else
                    {
                        item.RoundedRap = data[0];
                    }
                    item.AverageSales = data[1];
                    item.Score = ItemScoring.Score(id, data[0], data[1]);
                    item.LastUpdated = DateTime.Now;

                    Utility.ItemCache.UpdateCachedItem(item);
                }

                return item;
            }
        }
    }
}
