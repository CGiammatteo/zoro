namespace Zoro.LimitedData
{
    internal class ItemScoring
    {
        public static double Score(long item, int avgRap, int avgSales)
        {
            double score = 0;

            int itemRap = Rolimons.RoliHelper.GrabItemRap(item);
            int itemValue = Rolimons.RoliHelper.GrabItemValue(item);
            bool isProjected = Rolimons.RoliHelper.GrabProjected(item);
            int trendingStatus = Rolimons.RoliHelper.GrabItemTrend(item);
            int demandStatus = Rolimons.RoliHelper.GrabItemDemand(item);

            if(itemRap != itemValue)
            {
                score += 1;
            }

            if(itemValue > itemRap)
            {
                score += 0.5;
            }

            if(isProjected == true)
            {
                score -= 2;
            }

            if(ItemHelper.CustomProjectedDetection(avgRap, itemRap) == true)
            {
                score -= 1;
            }

            if(avgSales < Settings.MinumumDailySales)
            {
                score -= 1;
            }
            else
            {
                score += 0.5;
            }

            switch (trendingStatus)
            {
                case -1:
                    break;
                case 0:
                    score -= 0.5;
                    break;
                case 1:
                    score -= 1;
                    break;
                case 2:
                    score += 0.5;
                    break;
                case 3:
                    score += 1;
                    break;
                case 4:
                    score += 0.25;
                    break;
                default: break;
            }

            switch (demandStatus)
            {
                case -1:
                    break;
                case 0:
                    score -= 1;
                    break;
                case 1:
                    score -= 0.5;
                    break;
                case 2:
                    score += 0.5;
                    break;
                case 3:
                    score += 1;
                    break;
                case 4:
                    score += 2;
                    break;
                default: break;
            }

            return score;
        }
    }
}
