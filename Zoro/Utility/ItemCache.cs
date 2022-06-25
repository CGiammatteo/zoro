using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoro.Utility
{
    internal class ItemCache
    {
        public static void CacheItem(LimitedData.Item cachedItem)
        {
            /*
             how cache works:
            - while searching for players, we cache every single item found. Inside the item, we store all the normal item data,
            as well as when we cache it. We update each item every 3 days, so thats a thing. we search through a json file
            for all the data, very similar to how we access the rolimons data. 
             */

            if (!Directory.Exists(AppContext.BaseDirectory + @"\data"))
            {
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\data");
            }

            if (File.Exists(AppContext.BaseDirectory + @"\data\cache.json"))
            {
                File.Delete(AppContext.BaseDirectory + @"\data\cache.json");
            }

            try
            {

            }
            catch(Exception ex)
            {

            }
        }
    }
}
