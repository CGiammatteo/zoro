using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zoro.Trading
{
    internal class Queue
    {
        public static void HandleQueue()
        {
            int TradesSent = 0;

            while (true)
            {
                try
                {
                    if(Settings.TradeQueue.Count > 0)
                    {
                        Sending.SendTrade(Settings.TradeQueue[0]);
                        Settings.TradeQueue.RemoveAt(0);
                        Misc.Output.Success($"Sent trade successfully! (Queue length: {Settings.TradeQueue.Count})");
                        TradesSent++;
                    }
                }
                catch(Exception ex)
                {
                    //continue
                }
                Thread.Sleep(1);
            }
        }
    }
}
