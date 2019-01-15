using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Block_Indicate.Class;
using Block_Indicate.Data;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Block_Indicate.Controllers
{
    public class SmartTradeController : Controller
    {
        private readonly ApplicationDbContext db;

        public SmartTradeController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
            NavPrice navPrice = new NavPrice();
            Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            SmartTradeViewModel smartView = new SmartTradeViewModel();
            smartView.Customer = customer;
            UpdateTrades updateTrades = new UpdateTrades();
            await updateTrades.UpdateAsync(customer.Id);
            smartView.BinanceBalances = await binanceBalancesAsync;
            smartView.EstimatedBTC = smartView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
            smartView.TotalBTC = Decimal.Round(Convert.ToDecimal(smartView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
            smartView.CurrentPrices = await currentPricesAsync;
            List<string> Tokens = new List<string> { "ETHBTC", "LTCBTC", "BNBBTC", "NEOBTC", "GASBTC", "BTCUSDT", "MCOBTC", "WTCBTC", "LRCBTC", "QTUMBTC", "YOYOBTC", "OMGBTC", "ZRXBTC", "STRATBTC", "SNGLSBTC", "BQXBTC", "KNCBTC", "FUNBTC", "SNMBTC", "IOTABTC", "LINKBTC", "XVGBTC", "SALTBTC", "MDABTC", "MTLBTC", "SUBBTC", "EOSBTC", "SNTBTC", "ETCBTC", "MTHBTC", "ENGBTC", "DNTBTC", "ZECBTC", "BNTBTC", "ASTBTC", "DASHBTC", "OAXBTC", "BTGBTC", "EVXBTC", "REQBTC", "VIBBTC", "TRXBTC", "POWRBTC", "ARKBTC", "XRPBTC", "MODBTC", "ENJBTC", "STORJBTC", "KMDBTC", "RCNBTC", "NULSBTC", "RDNBTC", "XMRBTC", "DLTBTC", "AMBBTC", "BATBTC", "BCPTBTC", "ARNBTC", "GVTBTC", "CDTBTC", "GXSBTC", "POEBTC", "QSPBTC", "BTSBTC", "XZCBTC", "LSKBTC", "TNTBTC", "FUELBTC", "MANABTC", "BCDBTC", "DGDBTC", "ADXBTC", "ADABTC", "PPTBTC", "CMTBTC", "XLMBTC", "CNDBTC", "LENDBTC", "WABIBTC", "TNBBTC", "WAVESBTC", "GTOBTC", "ICXBTC", "OSTBTC", "ELFBTC", "AIONBTC", "NEBLBTC", "BRDBTC", "EDOBTC", "WINGSBTC", "NAVBTC", "LUNBTC", "APPCBTC", "VIBEBTC", "RLCBTC", "INSBTC", "PIVXBTC", "IOSTBTC", "STEEMBTC", "NANOBTC", "VIABTC", "BLZBTC", "AEBTC", "NCASHBTC", "POABTC", "ZILBTC", "ONTBTC", "STORMBTC", "XEMBTC", "WANBTC", "WPRBTC", "QLCBTC", "SYSBTC", "GRSBTC", "CLOAKBTC", "GNTBTC", "LOOMBTC", "REPBTC", "TUSDBTC", "ZENBTC", "SKYBTC", "CVCBTC", "THETABTC", "IOTXBTC", "QKCBTC", "AGIBTC", "NXSBTC", "DATABTC", "SCBTC", "NPXSBTC", "KEYBTC", "NASBTC", "MFTBTC", "DENTBTC", "ARDRBTC", "HOTBTC", "VETBTC", "DOCKBTC", "POLYBTC", "PHXBTC", "HCBTC", "GOBTC", "PAXBTC", "RVNBTC", "DCRBTC", "USDCBTC", "MITHBTC", "BCHABCBTC" };
            ViewBag.Tokens = new SelectList(Tokens);
            smartView.Trades = db.Trades.Where(t => t.Active == true && t.Type == "SmartTrade").ToList();
            return View(smartView);
        }

        public async Task<IActionResult> CreateSmartTrade(Trade trade)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            BuildTrade build = new BuildTrade();
            build.Customer = customer;
            build.Trade = trade;
            SmartTrade smartTrade = new SmartTrade(build);
            await smartTrade.Decide();
            return RedirectToAction("Index");
        }
    }
}