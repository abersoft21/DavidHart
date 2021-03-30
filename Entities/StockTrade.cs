using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StockTrade
    {
        public string StockCode
        {
            get;
            set;
        }

        public int BuyWeek
        {
            get;
            set;
        }
        public decimal BuyPrice
        {
            get;
            set;
        }
        public int SellWeek
        {
            get;
            set;
        }
        public decimal SellPrice
        {
            get;
            set;
        }

        public decimal ROI
        {
            get;
            set;
        }
    }
}
