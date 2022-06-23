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
            Console.ReadKey();
        }
    }
}
