using Discord;
using Discord.Webhook;
using System.Collections.Generic;

namespace Zoro.Discord
{
    internal class Webhooks
    {
        public static async void SendOutbound(Trading.Trade trade)
        {
            List<string> OfferNames = new List<string>();
            List<string> RecieveNames = new List<string>();

            double offerRap = 0;
            double recieveRap = 0;

            foreach (var offerItems in trade.Offer)
            {
                OfferNames.Add(offerItems.ItemName);
                offerRap += offerItems.RoundedRap;
            }

            foreach (var recieveItems in trade.Recieve)
            {
                RecieveNames.Add(recieveItems.ItemName);
                recieveRap += recieveItems.RoundedRap;
            }

            DiscordWebhookClient hook = new DiscordWebhookClient(Settings.OutboundHook);
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Trade has been sent successfully!");

            builder.AddField("Offer: ", $"{string.Join("\n", OfferNames)}");
            builder.AddField("Recieve: ", $"{string.Join("\n", RecieveNames)}");
            builder.AddField("Expected profit: ", $"{trade.PercentIncrease}% ({offerRap} -> {recieveRap})");

            builder.WithColor(Color.Blue);
            builder.WithFooter(footer => footer.Text = $"Zoro | v{Settings.Version}");

            Embed[] embedArray = new Embed[] { builder.Build() };

            await hook.SendMessageAsync(null, false, embedArray, "Zoro", null, null, null, null);
        }

        public static async void SendCompleted(Trading.Trade trade)
        {
            List<string> OfferNames = new List<string>();
            List<string> RecieveNames = new List<string>();

            double offerRap = 0;
            double recieveRap = 0;

            foreach (var offerItems in trade.Offer)
            {
                OfferNames.Add(offerItems.ItemName);
                offerRap += offerItems.RoundedRap;
            }

            foreach (var recieveItems in trade.Recieve)
            {
                RecieveNames.Add(recieveItems.ItemName);
                recieveRap += recieveItems.RoundedRap;
            }

            DiscordWebhookClient hook = new DiscordWebhookClient(Settings.OutboundHook);
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Trade accepted!");

            builder.AddField("Offer: ", $"{string.Join("\n", OfferNames)}");
            builder.AddField("Recieve: ", $"{string.Join("\n", RecieveNames)}");
            builder.AddField("Profit: ", $"{trade.PercentIncrease}% ({offerRap} -> {recieveRap})");

            builder.WithColor(Color.Green);
            builder.WithFooter(footer => footer.Text = $"Zoro | v{Settings.Version}");

            Embed[] embedArray = new Embed[] { builder.Build() };

            await hook.SendMessageAsync(null, false, embedArray, "Zoro", null, null, null, null);
        }

        public static async void SendInbound(Trading.Trade trade)
        {
           
        }
    }
}
