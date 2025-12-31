using System;
using System.Collections.Generic;

namespace BeyondBot.Model
{
    public class KLine
    {
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal Volume { get; set; }

        public KLine(DateTime openTime, DateTime closeTime, decimal openPrice, decimal closePrice, decimal highPrice, decimal lowPrice, decimal volume)
        {
            OpenTime = openTime;
            CloseTime = closeTime;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            HighPrice = highPrice;
            LowPrice = lowPrice;
            Volume = volume;
        }
    }
}