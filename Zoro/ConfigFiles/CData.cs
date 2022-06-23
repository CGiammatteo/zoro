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
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;

                        o1["Key"] = Settings.Key;
                        o1["Cookie"] = Settings.Cookie;
                        o1["UserId"] = Settings.UserId;

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