using BeyondBot.Model;
using BeyondBot.Model.Indicator;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cms;

namespace BeyondBot.Controller
{
    class Trader
    {
        static Trader? instance;
        public static Trader Instance => instance ??= new Trader();
        IDBController dbController = MySQLController.Instance;
        IAPIController apiController = BybitController.Instance;
        public IDBController DBController { get { return dbController; }} 
        public IAPIController APIController { get { return apiController; }} 

        // This class can be expanded to manage trading operations if needed
        public void StartTrading()
        {
            
        }
        public async Task InitializeMarketAsync(string symbolName, TimeframeInterval timeframeInterval)
        {
            // Initialization logic if needed

            //Save new symbol and timeframe to database if not exists
            var symbolNames = dbController.GetSymbols(symbolName).ConvertAll(s => s.SymbolName);
            Symbol symbol;
            if(symbolNames.Count == 0)
            {
                dbController.InsertSymbol(symbolName);
                symbol = dbController.GetSymbols(symbolName).First();
            }
            else
            {
                symbol = dbController.GetSymbols(symbolName).First();
            }

            var timeframes = dbController.GetTimeframes(symbol.ID, timeframeInterval);
            Timeframe timeframe;
            if(timeframes.Count == 0)
            {
                Timeframe newTimeframe = new Timeframe(symbol.ID, timeframeInterval);
                dbController.InsertTimeframe(newTimeframe);
                timeframe = dbController.GetTimeframes(symbol.ID, timeframeInterval).First();
            }
            else
            {
                timeframe = timeframes.First();
            }


            
            // Get first kline from internet
            // Note: Bybit API returns klines in descending order (newest first), so we use LastOrDefault() to get the oldest
            var firstKline = (await apiController.GetMarketDataAsync(symbolName, timeframe.Interval.ToString(), 1000, startTime: new DateTime(2019,1,1), endTime: DateTime.Now)).OrderBy(k => k.CloseTime).FirstOrDefault();
            if(firstKline != null)
            {
                dbController.InsertKLine(firstKline, timeframe.ID);
            }
            else
            {
                throw new Exception("Failed to retrieve initial kline data from API.");
            }
            
            DateTime lastKlineTime = firstKline.OpenTime;
            do
            {
                // Fetch latest klines from API
                var klines = await apiController.GetMarketDataAsync(symbolName, timeframe.Interval.ToString(), 1000, startTime: lastKlineTime, endTime: DateTime.Now);
                // Reverse to get ascending order (oldest to newest) since Bybit returns descending order
                klines = klines.ToList();
                klines.Reverse();
                if(klines.Count == 0) break;

                foreach(var kline in klines)
                {
                    dbController.InsertKLine(kline, timeframe.ID);
                }

                lastKlineTime = klines.Last().CloseTime;
            }while(true);
        }

        public async Task UpdateMarketDataAsync()
        {
            // Implementation for updating market data if needed

            var symbols = dbController.GetSymbols();
            var timeframes = dbController.GetTimeframes();
            foreach(var symbol in symbols)
            {
                foreach(var timeframe in timeframes.Where(t => t.SymbolID == symbol.ID))
                {
                    var klines = dbController.GetKLines(symbol, timeframe.Interval);
                    DateTime lastKlineTime = klines.Count > 0 ? klines.Max(k => k.CloseTime) : DateTime.UnixEpoch;

                    do
                    {
                        var newKlines = await apiController.GetMarketDataAsync(symbol.SymbolName, timeframe.Interval.ToString(), 1000, startTime: lastKlineTime, endTime: DateTime.Now);
                        // Reverse to get ascending order (oldest to newest) since Bybit returns descending order
                        newKlines = newKlines.ToList();
                        newKlines.Reverse();
                        if(newKlines.Count == 0) break;

                        foreach(var kline in newKlines)
                        {
                            dbController.InsertKLine(kline, timeframe.ID);
                        }

                        lastKlineTime = newKlines.Last().CloseTime;
                    }while(true);
                }
            }
        }

        public async Task UpdateMarketDataAsync(string symbolName)
        {
            // Implementation for updating market data for a specific symbol if needed

            var symbols = dbController.GetSymbols(symbolName);
            if(symbols.Count == 0) throw new Exception("Symbol not found in database.");

            var symbol = symbols.First();
            var timeframes = dbController.GetTimeframes().Where(t => t.SymbolID == symbol.ID).ToList();
            foreach(var timeframe in timeframes)
            {
                var klines = dbController.GetKLines(symbol, timeframe.Interval);
                DateTime lastKlineTime = klines.Count > 0 ? klines.Max(k => k.CloseTime) : DateTime.UnixEpoch;

                do
                {
                    var newKlines = await apiController.GetMarketDataAsync(symbol.SymbolName, timeframe.Interval.ToString(), 1000, startTime: lastKlineTime, endTime: DateTime.Now);
                    // Reverse to get ascending order (oldest to newest) since Bybit returns descending order
                    newKlines = newKlines.ToList();
                    newKlines.Reverse();
                    if(newKlines.Count == 0) break;

                    foreach(var kline in newKlines)
                    {
                        dbController.InsertKLine(kline, timeframe.ID);
                    }

                    lastKlineTime = newKlines.Last().CloseTime;
                }while(true);
            }
        }

        public async Task UpdateMarketDataAsync(string symbolName, TimeframeInterval timeframeInterval)
        {
            // Implementation for updating market data for a specific symbol and timeframe if needed

            var symbols = dbController.GetSymbols(symbolName);
            if(symbols.Count == 0) throw new Exception("Symbol not found in database.");

            var symbol = symbols.First();
            var timeframes = dbController.GetTimeframes(symbol.ID, timeframeInterval);
            if(timeframes.Count == 0) throw new Exception("Timeframe not found for the given symbol in database.");

            var timeframe = timeframes.First();
            var klines = dbController.GetKLines(symbol, timeframe.Interval);
            DateTime lastKlineTime = klines.Count > 0 ? klines.Max(k => k.CloseTime) : DateTime.UnixEpoch;

            do
            {
                var newKlines = await apiController.GetMarketDataAsync(symbol.SymbolName, timeframe.Interval.ToString(), 1000, startTime: lastKlineTime, endTime: DateTime.Now);
                // Reverse to get ascending order (oldest to newest) since Bybit returns descending order
                newKlines = newKlines.ToList();
                newKlines.Reverse();
                if(newKlines.Count == 0) break;

                foreach(var kline in newKlines)
                {
                    dbController.InsertKLine(kline, timeframe.ID);
                }

                lastKlineTime = newKlines.Last().CloseTime;
            }while(true);
        }

        public void CalculateIndicators()
        {
            // Implementation for calculating indicators if needed

            var symbols = dbController.GetSymbols();
            var timeframes = dbController.GetTimeframes();

            foreach(var symbol in symbols)
            {
                foreach(var timeframe in timeframes.Where(t => t.SymbolID == symbol.ID))
                {
                    var klines = dbController.GetKLines(symbol, timeframe.Interval);
                    // Placeholder: Calculate indicators based on klines
                    // For example, calculate moving averages, RSI, etc.

                    int depth = 23;

                    var atrs = new ATR().Calculate(klines, depth);
                    var emas = new EMA().Calculate(klines, depth);
                    var rmas = new RMA().Calculate(klines, depth);

                    foreach(var atr in atrs)
                    {
                        IndicatorCache cache = atr.ConvertToCache(atr.KLineID);
                        dbController.InsertStrategy(cache);
                    }

                    foreach(var ema in emas)
                    {
                        IndicatorCache cache = ema.ConvertToCache(ema.KLineID);
                        dbController.InsertStrategy(cache);
                    }

                    foreach(var rma in rmas)
                    {
                        IndicatorCache cache = rma.ConvertToCache(rma.KLineID);
                        dbController.InsertStrategy(cache);
                    }
                }
            }
        }

        public void CalculateIndicators(string symbolName)
        {
            // Implementation for calculating indicators for a specific symbol if needed

            var symbols = dbController.GetSymbols(symbolName);
            if(symbols.Count == 0) throw new Exception("Symbol not found in database.");

            var symbol = symbols.First();
            var timeframes = dbController.GetTimeframes().Where(t => t.SymbolID == symbol.ID).ToList();

            foreach(var timeframe in timeframes)
            {
                var klines = dbController.GetKLines(symbol, timeframe.Interval);
                // Placeholder: Calculate indicators based on klines
                // For example, calculate moving averages, RSI, etc.

                int depth = 23;

                var atrs = new ATR().Calculate(klines, depth);
                var emas = new EMA().Calculate(klines, depth);
                var rmas = new RMA().Calculate(klines, depth);

                foreach(var atr in atrs)
                {
                    IndicatorCache cache = atr.ConvertToCache(atr.KLineID);
                    dbController.InsertStrategy(cache);
                }

                foreach(var ema in emas)
                {
                    IndicatorCache cache = ema.ConvertToCache(ema.KLineID);
                    dbController.InsertStrategy(cache);
                }

                foreach(var rma in rmas)
                {
                    IndicatorCache cache = rma.ConvertToCache(rma.KLineID);
                    dbController.InsertStrategy(cache);
                }
            }
        }

        public void CalculateIndicators(string symbolName, TimeframeInterval timeframeInterval)
        {
            // Implementation for calculating indicators for a specific symbol and timeframe if needed

            var symbols = dbController.GetSymbols(symbolName);
            if(symbols.Count == 0) throw new Exception("Symbol not found in database.");

            var symbol = symbols.First();
            var timeframes = dbController.GetTimeframes(symbol.ID, timeframeInterval);
            if(timeframes.Count == 0) throw new Exception("Timeframe not found for the given symbol in database.");

            var timeframe = timeframes.First();
            var klines = dbController.GetKLines(symbol, timeframe.Interval);
            // Placeholder: Calculate indicators based on klines
            // For example, calculate moving averages, RSI, etc.

            int depth = 23;

            var atrs = new ATR().Calculate(klines, depth);
            var emas = new EMA().Calculate(klines, depth);
            var rmas = new RMA().Calculate(klines, depth);

            foreach(var atr in atrs)
            {
                IndicatorCache cache = atr.ConvertToCache(atr.KLineID);
                dbController.InsertStrategy(cache);
            }

            foreach(var ema in emas)
            {
                IndicatorCache cache = ema.ConvertToCache(ema.KLineID);
                dbController.InsertStrategy(cache);
            }

            foreach(var rma in rmas)
            {
                IndicatorCache cache = rma.ConvertToCache(rma.KLineID);
                dbController.InsertStrategy(cache);
            }
        }

        
    }
}