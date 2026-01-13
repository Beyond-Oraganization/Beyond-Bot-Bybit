using System;
using System.Collections.Generic;
using BeyondBot.Model;
using Bybit.Net.Enums;

namespace BeyondBot.Controller
{
    interface IDBController
    {
        List<Symbol> GetSymbols();
        List<Symbol> GetSymbols(string symbolName);
        void InsertSymbol(string symbolName);
        void UpdateSymbol(int symbolID, string newSymbolName);
        void DeleteSymbol(int symbolID);
        void DeleteSymbol(string symbolName);

        List<Timeframe> GetTimeframes();
        List<Timeframe> GetTimeframes(int symbolID);
        List<Timeframe> GetTimeframes(int symbolID, TimeframeInterval interval);
        void InsertTimeframe(Timeframe timeframe);
        void UpdateTimeframe(int timeframeID, Timeframe timeframe);
        void DeleteTimeframe(int timeframeID);

        List<KLine> GetKLines();
        List<KLine> GetKLines(Symbol symbol, TimeframeInterval timeframeInterval);
        List<KLine> GetKLines(Symbol symbol, TimeframeInterval timeframeInterval, DateTime startTime, DateTime endTime);
        void InsertKLine(KLine kline, int timeframeID);
        void UpdateKLine(KLine kline);
        void DeleteKLine(KLine kline);

        List<Order> GetOrders();
        List<Order> GetOrders(string symbol);
        void InsertOrder(Order order);
        void UpdateOrder(string orderID, Order order);
        void DeleteOrder(string orderID);

        List<StrategyCache> GetStrategies();
        void InsertStrategy(StrategyCache strategy);
        void UpdateStrategy(int strategyID, StrategyCache strategy);
        void DeleteStrategy(int strategyID);
    }
}