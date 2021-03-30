using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace BLL_VirtualTrader
{
    public class Trading
    {
        private string fileStr;
        private string resultText;

        public Trading(string fileStr)
        {
            this.fileStr = fileStr;
        }
        
        public void Start()
        {
            if (fileStr.Length > 0)
            {
                fileStr = RemoveEmptyValues(fileStr);
                
                decimal [] dailySharePrices = ConvertCsvToSharePrices(fileStr);
                if (dailySharePrices.Any())
                {
                    StockTrade stockTrade = GetBestTradeYield(dailySharePrices);

                    resultText = string.Format("{0}({1}),{2}({3})",
                        stockTrade.BuyWeek, stockTrade.BuyPrice, stockTrade.SellWeek, stockTrade.SellPrice);

                    if (stockTrade.BuyPrice >= stockTrade.SellPrice)
                    {
                        resultText += "\r\n[unable to return profit.]";
                    }

                    Console.WriteLine(resultText);
                }
            }
        }

        public string Result()
        {
            return resultText; 
        }

        public string RemoveEmptyValues(string fileStr)
        {
            return fileStr.Replace(",,", ",0,");
        }

        public StockTrade GetBestTradeYield(decimal [] dailySharePrices)
        {
            KeyValuePair<int, decimal>[] orderedByMinFirst = OrderByMinFirst(dailySharePrices
                .Take(dailySharePrices.Length - 1).ToArray());
            KeyValuePair<int, decimal>[] orderedByMaxFirst = OrderByMaxFirst(dailySharePrices
                .Skip(1).Take(dailySharePrices.Length - 1).ToArray());

            StockTrade stockTrade = new StockTrade();

            int indexBestBuy = 0;
            
            decimal strongestChange = 0;

            foreach (var kvp in orderedByMinFirst)
            {
                indexBestBuy = kvp.Key;

                // search for higher price values only after the index of the current lowest price:
                var higherLaterBestPrice = orderedByMaxFirst
                    .Where(p => p.Key > indexBestBuy-1 && p.Value > kvp.Value)
                    .OrderByDescending(p=>p.Value)
                    .ThenBy(q=>q.Key).FirstOrDefault();

                decimal difference = higherLaterBestPrice.Value - kvp.Value;

                // if we can get same ROI in shorter time..
                if (difference > strongestChange || (difference == strongestChange && stockTrade.SellWeek - stockTrade.BuyWeek > higherLaterBestPrice.Key - kvp.Key))
                {
                    stockTrade.ROI = difference;

                    stockTrade.BuyPrice = kvp.Value;
                    stockTrade.SellPrice = higherLaterBestPrice.Value;

                    stockTrade.BuyWeek = indexBestBuy;
                    stockTrade.SellWeek = higherLaterBestPrice.Key;
                    strongestChange = difference;
                }
            }

            // normalise week index to week numbers:
            stockTrade.BuyWeek += 1;
            stockTrade.SellWeek += 2;

            return stockTrade;
        }

        public static KeyValuePair<int, decimal>[] OrderByMaxFirst(decimal[] dailySharePrices)
        {
            return dailySharePrices.Select((m, i) =>
            new KeyValuePair<int, decimal>(i, m))
                .OrderByDescending(x => x.Value)
                .ThenByDescending(x => x.Key) // get highest earlier trade value ordered by closest first where they're the same - less days for same profit is preferred
                .ToArray();
        }

        public static KeyValuePair<int, decimal>[] OrderByMinFirst(decimal[] dailySharePrices)
        {
            return dailySharePrices.Select((m, i) =>
            new KeyValuePair<int, decimal>(i, m))
                .OrderBy(x => x.Value)
                .ThenBy(x => x.Key) // same low price? Buy late, sell early = higher profit to days ratio
                .ToArray();
        }

        public decimal [] ConvertCsvToSharePrices(string fileStr)
        {
            return fileStr.Split(',').Select(decimal.Parse).ToArray();
        }
    }
}
