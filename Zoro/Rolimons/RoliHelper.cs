using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Zoro.Rolimons
{
    internal class RoliHelper
    {
        public static bool GrabRolimonsData()
        {
            if (File.Exists(AppContext.BaseDirectory + @"\data\RoliData.json"))
            {
                File.Delete(AppContext.BaseDirectory + @"\data\RoliData.json");
            }

            if (!Directory.Exists(AppContext.BaseDirectory + @"\data"))
            {
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\data");
            }

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://rolimons.com/itemapi/itemdetails");
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"; //you need to use a useragent for rolimons
                httpWebRequest.Proxy = WebProxy.GetDefaultProxy();
                httpWebRequest.Credentials = WebProxy.GetDefaultProxy().Credentials;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic json = JsonConvert.DeserializeObject(result);

                    using (StreamWriter file = File.CreateText(AppContext.BaseDirectory + @"\data\RoliData.json"))
                    {
                        using (JsonTextWriter jsonWrite = new JsonTextWriter(file))
                        {
                            json.WriteTo(jsonWrite);
                        }
                    }
                }

                httpResponse.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static int GrabItemRap(long itemId)
        {
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            int rap = Convert.ToInt32(roliFile.items[itemId.ToString()][2]);

            return rap;
        }

        public static string GrabItemName(long itemId)
        {
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            string name = Convert.ToString(roliFile.items[itemId.ToString()][0]);

            return name;
        }

        public static int GrabItemValue(long itemId)
        {
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            int value = Convert.ToInt32(roliFile.items[itemId.ToString()][4]);
            int rap = Convert.ToInt32(roliFile.items[itemId.ToString()][2]);

            if (value != rap)
            {
                return value;
            }
            else
            {
                //not valued, send 0
                return 0;
            }
        }

        public static bool GrabProjected(long itemId)
        {
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            // -1 = not projected, 1 = projected

            string projected = Convert.ToString(roliFile.items[itemId.ToString()][7]);

            if (projected == "-1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int GrabItemTrend(long itemId)
        {
            /*
             trend type:
            -1 = not assigned
             0 = lowering
             1 = unstable
             2 = stable
             3 = ?? (might be raising)
             4 = fluxuating
             */
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            int trend = roliFile.items[itemId.ToString()][6];

            return trend;
        }

        public static int GrabItemDemand(long itemId)
        {
            /*
             demand type:
            -1 = not assigned
             0 = terrible
             1 = low
             2 = normal
             3 = high
             4 = amazing
             */
            dynamic roliFile = JsonConvert.DeserializeObject(File.ReadAllText(AppContext.BaseDirectory + @"\data\RoliData.json"));

            int demand = roliFile.items[itemId.ToString()][5];

            return demand;
        }
    }
}
