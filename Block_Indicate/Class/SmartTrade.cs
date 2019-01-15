using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class SmartTrade
    {
        private BuildTrade build;
        public SmartTrade(BuildTrade build)
        {
            this.build = build;
        }

        public async Task Decide()
        {
            if (build.Trade.IsTrailingTakeProfit && !build.Trade.IsTrailingStopLoss)
            {
                await CreateTrailingTake();
            }else if (build.Trade.IsTrailingStopLoss && !build.Trade.IsTrailingTakeProfit)
            {
                await CreateTrailingStop();
            }else if (build.Trade.IsTrailingTakeProfit && build.Trade.IsTrailingStopLoss)
            {
                await CreateTrailingBoth();
            }else if (!build.Trade.IsStopLoss)
            {
                await CreateNoStop();
            }
            else if (!build.Trade.IsTakeProfit)
            {
                await CreateNoTake();
            }
            else if (!build.Trade.IsTakeProfit && !build.Trade.IsStopLoss)
            {
                await CreateBuy();
            }
            else
            {
                await Create();
            }
        }
        public async Task Create()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
            Sell sell = new Sell();
            build = await sell.LimitAsync(build);
            await smartTradeCustomer.Update(build);
            Message message = new Message();
            await message.SmartCreated(build);
            var scanStop = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.Sell();
                    await smartTradeCustomer.Update(build);
                    await message.SmartConcluded(build);
                }
            });
        }

        public async Task CreateTrailingTake()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            Trailing trailing = new Trailing(build);
            build = trailing.Take();
            await smartTradeCustomer.Update(build);
            Message messageTrail = new Message();
            var scanTake = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.Take();
                    build = scan.TrailingTake();
                    await smartTradeCustomer.Update(build);
                    await messageTrail.SmartConcluded(build);
                }
            });
            Message message = new Message();
            await message.SmartCreated(build);
            var scanStop = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.SellNoOrder();
                    if (build.TrailingTake == false)
                    {
                        await smartTradeCustomer.Update(build);
                        await message.SmartConcluded(build);
                    }
                }
            });
        }




        public async Task CreateTrailingStop()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
            Sell sell = new Sell();
            build = await sell.LimitAsync(build);
            await smartTradeCustomer.Update(build);
            Message message = new Message();
            await message.SmartCreated(build);
            var scanStop = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.Sell();
                    await smartTradeCustomer.Update(build);
                    await message.SmartConcluded(build);
                }
            });
        }
        public async Task CreateTrailingBoth()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
            Sell sell = new Sell();
            build = await sell.LimitAsync(build);
            await smartTradeCustomer.Update(build);
            Message message = new Message();
            await message.SmartCreated(build);
            var scanStop = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.Sell();
                    await smartTradeCustomer.Update(build);
                    await message.SmartConcluded(build);
                }
            });
        }

        public async Task CreateNoStop()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
            Sell sell = new Sell();
            build = await sell.LimitAsync(build);
            await smartTradeCustomer.Update(build);
            Message message = new Message();
            await message.SmartCreated(build);
        }

        public async Task CreateNoTake()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
            Message message = new Message();
            await message.SmartCreated(build);
            var scanStop = Task.Run(async () => {
                while (build.Trade.Active)
                {
                    Scan scan = new Scan(build);
                    build = scan.Sell();
                    await smartTradeCustomer.Update(build);
                    await message.SmartConcluded(build);
                }
            });
        }

        public async Task CreateBuy()
        {
            SmartTradeCustomer smartTradeCustomer = new SmartTradeCustomer(build);
            build = await smartTradeCustomer.Create();
            CalculateAmount calculateAmount = new CalculateAmount();
            build = await calculateAmount.GetAmountAsync(build);
            Buy buy = new Buy(build);
            build = await buy.MarketAsync();
            await smartTradeCustomer.Update(build);
        }
    }
}

