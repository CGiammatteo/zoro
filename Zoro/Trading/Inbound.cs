using System.Timers;

namespace Zoro.Trading
{
    internal class Inbound
    {
        public static void SetupInbound()
        {
            Timer timer = new Timer(Settings.InboundCheckTime * 60000); //10 minutes
            timer.Elapsed += new ElapsedEventHandler(OnTime);
            timer.Start();
        }

        private static void OnTime(object source, ElapsedEventArgs e)
        {
            //check inbound and eval trades here
        }
    }
}
