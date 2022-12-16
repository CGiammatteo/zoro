using System;
using System.Collections.Generic;
using System.Threading;

namespace Zoro.Trading
{
    internal class Outbound
    {
        public static void MainOutboundLoop()
        {
            Misc.Output.Information("Outbound trading started.");

            while (true)
            {
                //choose a random player to trade with
                long foundUser = Utility.Users.FindPlayer();
                Misc.Output.Basic($"Found user: {foundUser}");

                List<LimitedData.Item> TraderInventory = new List<LimitedData.Item>();

                foreach (LimitedData.Item item in Utility.UserData.GrabPlayerItems(foundUser))
                {
                    TraderInventory.Add(item);
                }

                Trade foundTrade = FindBestTrade(Settings.UserItems, TraderInventory, foundUser);

                if(foundTrade != null)
                {
                    List<string> OfferNames = new List<string>();
                    List<string> RecieveNames = new List<string>();

                    double offerRap = 0;
                    double recieveRap = 0;

                    foreach (var offerItems in foundTrade.Offer)
                    {
                        OfferNames.Add(offerItems.ItemName);
                        offerRap += offerItems.RoundedRap;
                    }

                    foreach (var recieveItems in foundTrade.Recieve)
                    {
                        RecieveNames.Add(recieveItems.ItemName);
                        recieveRap += recieveItems.RoundedRap;
                    }
                    Misc.Output.Basic($"Trade added to queue: [{string.Join(", ", OfferNames)}] for [{string.Join(", ", RecieveNames)}] ({Convert.ToString(foundTrade.PercentIncrease)}%) ({offerRap} -> {recieveRap})");
                    Settings.TradeQueue.Add(foundTrade);
                }
                Thread.Sleep(1);
            }
        }

        private static Trade FindBestTrade(List<LimitedData.Item> MyItems, List<LimitedData.Item> TheirItems, long traderId)
        {
            
            List<LimitedData.Item> myTemp = new List<LimitedData.Item>();
            List<LimitedData.Item> theirTemp = new List<LimitedData.Item>();

            foreach(LimitedData.Item item in MyItems)
            {
                if(!Settings.NotForTrade.Contains(item.ItemId))
                {
                    myTemp.Add(item);
                }
            }
            
            foreach (LimitedData.Item item in TheirItems)
            {
                if (!Settings.BlacklistedItems.Contains(item.ItemId))
                {
                    theirTemp.Add(item);
                }
            }
            
            List<List<LimitedData.Item>> MyCombinatrions = CreateCombo(myTemp);
            List<List<LimitedData.Item>> TraderCombinations = CreateCombo(theirTemp);

            double bestDeal = 0; //highest percentage increase
            double bestScore = 0;

            //we get combinations for both our and their items, then we cross check each combinations to see which is most profitable
            for(int x = 0; x <= 1; x++)
            {
                foreach (var myList in MyCombinatrions)
                {
                    double myRap = 0;
                    double theirRap = 0;

                    double myScore = 0;
                    double theirScore = 0;
                    List<LimitedData.Item> offer = new List<LimitedData.Item>();
                    List<LimitedData.Item> recieve = new List<LimitedData.Item>();

                    foreach (var theirList in TraderCombinations)
                    {
                        foreach (var myItem in myList)
                        {
                            if(myItem != null)
                            {
                                myRap += myItem.RoundedRap;
                                myScore += myItem.Score;
                                offer.Add(myItem);
                            }
                        }
                        foreach (var theirItem in theirList)
                        {
                            if(theirItem != null)
                            {
                                theirRap += theirItem.RoundedRap;
                                theirScore += theirItem.Score;
                                recieve.Add(theirItem);
                            }
                        }

                        double percentIncrease = Math.Round((theirRap - myRap) / theirRap * 100, 2);

                        if (percentIncrease >= Settings.MinimumProfit && percentIncrease <= Settings.MaximumProfit && myScore < theirScore)
                        {
                            //good trade found
                            if(x > 0)
                            {
                                //second time in, find the best trade found last time then get outta there
                                if(percentIncrease == bestDeal && bestScore == myScore + theirScore)
                                {
                                    Trade createTrade = new Trade();
                                    createTrade.TraderId = traderId;
                                    createTrade.PercentIncrease = percentIncrease;
                                    foreach(var items in offer)
                                    {
                                        createTrade.Offer.Add(items);
                                    }
                                    foreach (var items in recieve)
                                    {
                                        createTrade.Recieve.Add(items);
                                    }

                                    return createTrade;
                                }
                            }
                            else
                            {
                                bool badTrade = false;
                                //bad trade = same item in offer and recieve
                                foreach(var mine in offer)
                                {
                                    foreach(var theirs in recieve)
                                    {
                                        if(mine.ItemName == theirs.ItemName)
                                        {
                                            badTrade = true;
                                        }
                                    }
                                }
                                if((bestDeal < percentIncrease && bestScore < myScore + theirScore && !badTrade) || (bestScore < myScore + theirScore && !badTrade))
                                {
                                    bestDeal = percentIncrease;
                                    bestScore = myScore + theirScore;
                                }
                                badTrade = false;
                            }
                        }
                        myRap = 0;
                        theirRap = 0;
                        myScore = 0;
                        theirScore = 0;
                        offer.Clear();
                        recieve.Clear();
                    }
                }
            }
            return null;

        }

        private static List<List<LimitedData.Item>> CreateCombo(List<LimitedData.Item> Inventory)
        {
            List<List<LimitedData.Item>> Combinations = new List<List<LimitedData.Item>>();
            for (int i = 1; i <= 4; i++)
            {
                List<List<LimitedData.Item>> temp = new List<List<LimitedData.Item>>();
                for (int j = 0; j <= Inventory.Count - i; j++)
                {
                    List<LimitedData.Item> current = new List<LimitedData.Item>();
                    for (int k = 0; k < i; k++)
                    {
                        current.Add(Inventory[j + k]);
                    }
                    temp.Add(current);
                }
                Combinations.AddRange(temp);
            }
            return Combinations;
        }
    }
}
