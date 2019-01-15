using Binance.Net;
using Binance.Net.Objects;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class Scan
    {
        private BuildTrade build;
        public Scan(BuildTrade build)
        {
            this.build = build;
        }

        public BuildTrade Sell()
        {
            
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Trade.Symbol, KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                    if (data.Data.Low < build.Trade.StopLoss && build.Trade.Active == true)
                    {
                        Sell sell = new Sell();
                        build.Trade.SellPrice = sell.MarketReturn(build.Trade, build.Customer);
                        build.Trade.Active = false;
                        return;
                    }

                });

                while (build.Trade.Active)
                {
                    using (var clientRest = new BinanceClient())
                    {
                        var orderStatus = clientRest.QueryOrder(build.Trade.Symbol, build.Trade.OrderId);
                        if (orderStatus.Success)
                        {
                            if (orderStatus.Data.Status == OrderStatus.Filled || orderStatus.Data.Status == OrderStatus.Canceled)
                            {
                                build.Trade.FinalPercentageResult = (orderStatus.Data.Price - build.Trade.BuyPrice) / build.Trade.BuyPrice * 100;
                                build.Trade.SellPrice = orderStatus.Data.Price;
                                build.Trade.Active = false;
                                return build;
                            }
                        }
                    }
                }
                return build;
            }
        }

        public BuildTrade SellNoOrder()
        {
            bool active = true;
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Trade.Symbol, KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                    if (data.Data.Low < build.Trade.StopLoss && build.Trade.Active && active)
                    {
                        Sell sell = new Sell();
                        build = sell.MarketNoOrder(build);
                        return;
                    }else if (build.TrailingTake == true)
                    {
                        return;
                    }
                });

                while (build.Trade.Active && active)
                {

                }
                return build;
            }
        }

        public BuildTrade Take()
        {

            using (var client = new BinanceSocketClient())
            {
                bool active = true;
                var successKline = client.SubscribeToKlineStream(build.Trade.Symbol, KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                    if (data.Data.High > build.Trade.TakeProfit && active && build.Trade.Active)
                    {
                        build.Trade.TrailingTakeStopLoss = build.Trade.TakeProfit - (build.Trade.TakeProfit * (build.Trade.TrailingTakeProfitPercent / 100));
                        active = false;
                        return;
                    }
                });

                while (active && build.Trade.Active)
                {

                }
                return build;
            }
        }

        public BuildTrade TrailingTake()
        {

            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Trade.Symbol, KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                    if (data.Data.Low < build.Trade.TrailingTakeStopLoss && build.Trade.Active == true)
                    {
                        Sell sell = new Sell();
                        build = sell.MarketNoOrder(build);
                        return;
                    }else if (data.Data.High > build.Trade.TrailingTakeStopLoss + (build.Trade.TrailingTakeStopLoss * 0.005m) && build.Trade.Active)
                    {
                        build.Trade.TrailingTakeStopLoss = build.Trade.TrailingTakeStopLoss + (build.Trade.TrailingTakeStopLoss * 0.005m);
                        build.Trade.TakeProfit = build.Trade.TrailingTakeStopLoss + (build.Trade.TrailingTakeStopLoss * 0.01m);
                    }
                });

                while (build.Trade.Active)
                {

                }
                return build;
            }
        }
    }
}
