using System;
using System.Threading;

namespace Zoro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Zoro";
            WebData.ProxyLoader.LoadProxiesFromFile();
            ConfigFiles.CData.LoadConfigData();
            Utility.SessionDetails.CheckCookie();
            Utility.ItemCache.RefreshSessionCache();

            if (Utility.UserData.IsPremium(Settings.UserId) != true)
            {
                Misc.Output.Error("You do not have premium on roblox! Closing in 5 seconds...");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            if (Rolimons.RoliHelper.GrabRolimonsData())
            {
                Misc.Output.Success("Rolimons data grabbed successfully!");
            }
            else
            {
                Misc.Output.Error("Unable to grab rolimons data!");
            }

            int total = 0;

            foreach (LimitedData.Item item in Utility.UserData.GrabPlayerItems(Settings.UserId))
            {
                Settings.UserItems.Add(item);
                Misc.Output.Basic($"Item in inventory: {item.ItemName} ({item.RoundedRap})");
                total += item.RoundedRap;
            }

            Misc.Output.Information($"Inventory valued at: ~{total} robux");

            
            Thread outboundTrades = new Thread(Trading.Outbound.MainOutboundLoop);
            Thread queue = new Thread(Trading.Queue.HandleQueue);

            outboundTrades.Start();
            queue.Start();
            Trading.Completed.SetupCompleted();
            Trading.Inbound.SetupInbound();

            Console.Read();
        }
    }
}
