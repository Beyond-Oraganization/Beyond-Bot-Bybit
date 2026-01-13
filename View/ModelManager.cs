using System;
using System.Collections.Generic;
using BeyondBot.Controller;

namespace BeyondBot.View
{
    class ModelManager
    {
        public static ModelManager? instance;
        public static ModelManager Instance => instance ??= new ModelManager();
        IDBController dbController = MySQLController.Instance;
        // This class can be expanded to manage models if needed
        public void DisplayOrderDetails(Model.Order order)
        {
            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine($"Symbol: {order.Symbol}");
            Console.WriteLine($"Quantity: {order.Quantity}");
            Console.WriteLine($"Price: {order.Price}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Type: {order.Type}");
            Console.WriteLine($"Side: {order.Side}");
            Console.WriteLine($"Created At: {order.CreatedAt}");
        }
        public void DisplayKLineDetails(Model.KLine kline)
        {
            Console.WriteLine($"Open Time: {kline.OpenTime}");
            Console.WriteLine($"Close Time: {kline.CloseTime}");
            Console.WriteLine($"Open Price: {kline.OpenPrice}");
            Console.WriteLine($"Close Price: {kline.ClosePrice}");
            Console.WriteLine($"High Price: {kline.HighPrice}");
            Console.WriteLine($"Low Price: {kline.LowPrice}");
            Console.WriteLine($"Volume: {kline.Volume}");
        }
        public void DisplayOrders(List<Model.Order> orders)
        {
            foreach (var order in orders)
            {
                DisplayOrderDetails(order);
                Console.WriteLine("-----------------------");
            }
        }

        public void DisplayKLines(List<Model.KLine> klines)
        {
            foreach (var kline in klines)
            {
                DisplayKLineDetails(kline);
                Console.WriteLine("-----------------------");
            }
        }
        
        public void DisplayKlines(Model.Symbol symbol, Model.TimeframeInterval timeframe, List<Model.KLine> klines)
        {
            foreach (var kline in klines)
            {
                Console.WriteLine($"KLines for Symbol: {symbol.SymbolName}, Timeframe: {timeframe}");
                DisplayKLineDetails(kline);
                Console.WriteLine("-----------------------");
            }
        }

        public void SaveOrder(Model.Order order)
        {
            try
            {
                dbController.InsertOrder(order);
                // Implementation for saving order to database or file
                Console.WriteLine($"Order {order.OrderId} saved.");
            } catch (Exception ex)
            {
                Console.WriteLine("Error saving order: " + ex.Message);
            }
        }

        public void ShowKlines(Model.Symbol symbol, Model.TimeframeInterval timeframe)
        {
            try
            {
                var klines = dbController.GetKLines(symbol, timeframe);
                DisplayKlines(symbol, timeframe, klines);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying KLines: " + ex.Message);
            }
        }

        public void ShowOrders()
        {
            try
            {
                var orders = dbController.GetOrders();
                DisplayOrders(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying orders: " + ex.Message);
            }
        }
    }
}