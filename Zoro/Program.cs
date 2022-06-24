using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoro
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Zoro";
            ConfigFiles.CData.LoadConfigData();
            Utility.SessionDetails.CheckCookie();

            if(Rolimons.RoliHelper.GrabRolimonsData())
            {
                Misc.Output.Success("Rolimons data grabbed successfully!");
            }
            else
            {
                Misc.Output.Error("Unable to grab rolimons data!");
            }

            int total = 0;

            foreach (long id in Utility.UserData.GrabPlayerItems(Settings.UserId))
            {
                LimitedData.Item item = LimitedData.ItemHelper.CreateItemObject(id);

                if (LimitedData.ItemHelper.FindItemById(id) == null)
                {
                    Settings.CachedItems.Add(item);
                }
                Settings.UserItems.Add(item);
                Misc.Output.Basic($"Item in inventory: {item.ItemName} ({item.RoundedRap})");

                total += item.RoundedRap;
            }

            Misc.Output.Information($"Inventory: ~{total} total value");

            Console.ReadKey();
        }
    }
}
