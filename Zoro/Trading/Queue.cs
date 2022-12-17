using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zoro.Trading
{
    internal class Queue
    {
        public static async void HandleQueue()
        {
            int TradesSent = 0;

            while (true)
            {
                if(TradesSent >= 100)
                {
                    await Task.Delay(60 * 60000); //1 hour
                    TradesSent = 0;
                }
                try
                {
                    if(Settings.TradeQueue.Count > 0 && TradesSent < 100)
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
