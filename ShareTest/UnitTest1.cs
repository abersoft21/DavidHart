using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_VirtualTrader;
using System.Linq;
using System.Collections.Generic;
using Entities;

namespace ShareTest
{
    [TestClass]
    public class UnitTest1
    {
        private Trading trading;

        private string doublePeak_3days_1day = "99,2,4,8,16,2,16";
        private decimal[] doublePeak_3days_1day_decimal_array = new decimal[7] { 99, 2, 4, 8, 16, 2, 16 };
        private decimal[] doublePeak_1day_2days_decimal_array = new decimal[7] { 99, 2, 16, 2, 8, 16, 0 };
        
        private string nonEmptyValues = "99,1,0,4,0,2,16";

        private decimal[] randomListOne = new decimal[5] { 2.1m, -0.1m, 5, 5.5m, 0 };
        private decimal[] randomListOne_Ascending = new decimal[5] { -0.1m, 0, 2.1m, 5, 5.5m };
        private decimal[] randomListOne_Descending = new decimal[5] { 5.5m, 5, 2.1m, 0, -0.1m };

        // fault conditions:
        private decimal[] noTradeViable = new decimal[2] { 99.99m, 99.99m };
        private string someEmptyValues = "99,1,,4,,2,16";

        [TestInitialize]
        public void TestInit()
        {
            trading = trading ?? new Trading(doublePeak_3days_1day);
        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_ConvertCsvToSharePrices()
        {
            var actual = trading.ConvertCsvToSharePrices(doublePeak_3days_1day);
            var expected = doublePeak_3days_1day_decimal_array;

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_RemoveEmptyValues()
        {
            var actual = trading.RemoveEmptyValues(someEmptyValues);
            var expected = nonEmptyValues;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_Ascending()
        {
            var actual = Trading.OrderByMinFirst(randomListOne).Select(kvp => kvp.Value).ToArray();
            var expected = randomListOne_Ascending;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_Descending()
        {
            var actual = Trading.OrderByMaxFirst(randomListOne).Select(kvp => kvp.Value).ToArray();
            var expected = randomListOne_Descending;
            CollectionAssert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_()
        {
            var actual = trading.GetBestTradeYield(noTradeViable);
            Assert.AreEqual(actual.BuyPrice, actual.SellPrice);            
        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_doublePeak_GetBestROI()
        {
            var actual = trading.GetBestTradeYield(doublePeak_3days_1day_decimal_array);
            
            var expected = new StockTrade() { BuyPrice = 2, BuyWeek = 6, SellPrice = 16, SellWeek = 7 };

            Assert.IsTrue(actual.BuyWeek == expected.BuyWeek);
            Assert.IsTrue(actual.BuyPrice == expected.BuyPrice);
            Assert.IsTrue(actual.SellWeek == expected.SellWeek);
            Assert.IsTrue(actual.SellPrice == expected.SellPrice);

        }

        [TestMethod]
        public void Test_BLL_VirtualTrader_Trading_doublePeak_First_GetBestROI()
        {
            var actual = trading.GetBestTradeYield(doublePeak_1day_2days_decimal_array);
            
            var expected = new StockTrade() { BuyPrice = 2, BuyWeek = 2, SellPrice = 16, SellWeek =3 };

            Assert.IsTrue(actual.BuyWeek == expected.BuyWeek);
            Assert.IsTrue(actual.BuyPrice == expected.BuyPrice);
            Assert.IsTrue(actual.SellWeek == expected.SellWeek);
            Assert.IsTrue(actual.SellPrice == expected.SellPrice);
        }
    }
}
