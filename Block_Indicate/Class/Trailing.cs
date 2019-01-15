using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class Trailing
    {
        public BuildTrade build;
        public Trailing(BuildTrade build)
        {
            this.build = build;
        }

        public BuildTrade Take()
        {
            build.Trade.StopLoss = Decimal.Round(build.Trade.BuyPrice - (build.Trade.BuyPrice * (build.Trade.StopLossPercent / 100)), build.AskDecimalLength);
            build.TrailingTake = false;
            return build;
        }
    }
}
