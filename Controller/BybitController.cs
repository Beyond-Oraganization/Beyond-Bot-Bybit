using BeyondBot.Model;
using Bybit.Net.Clients;
using Bybit.Net.Objects.Models.V5;
using Bybit.Net.Enums;
using CryptoExchange.Net.Authentication;
using System.Runtime.CompilerServices;

//ByBit: YT1Of1CddArC6yKFUk
//ByBit-secret: DWKdiH6X7Kog6lSVmfrEnbRNG0kunGXUYoZs

namespace BeyondBot.Controller
{
    class BybitController : IAPIController
    {
        private static BybitController? instance;
        private BybitRestClient client = new BybitRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials("YT1Of1CddArC6yKFUk", "DWKdiH6X7Kog6lSVmfrEnbRNG0kunGXUYoZs");
            // Optional: Use testnet
            // options.Environment = BybitEnvironment.Testnet;
        });
        private BybitController() {}

        public static BybitController Instance => instance ??= new BybitController();

        public Order ConvertToOrder(BybitOrder bybitOrder)
        {
            Model.OrderStatus status = (Model.OrderStatus)Enum.Parse(typeof(Model.OrderStatus), bybitOrder.Status.ToString());
            Model.OrderType type = (Model.OrderType)Enum.Parse(typeof(Model.OrderType), bybitOrder.OrderType.ToString());
            Model.OrderSide side = (Model.OrderSide)Enum.Parse(typeof(Model.OrderSide), bybitOrder.Side.ToString());
            return new Order(bybitOrder.OrderId, bybitOrder.Symbol, bybitOrder.Quantity, bybitOrder.Price ?? 0, status, type, side, bybitOrder.CreateTime);
        }

        public async Task<Order> PlaceOrderAsync(string symbol, Model.OrderSide orderSide, Model.OrderType orderType, decimal quantity, decimal? price = null)
        {
            Bybit.Net.Enums.OrderSide side = orderSide == Model.OrderSide.Buy ? Bybit.Net.Enums.OrderSide.Buy : Bybit.Net.Enums.OrderSide.Sell;
            NewOrderType type = orderType == Model.OrderType.Market ? NewOrderType.Market : NewOrderType.Limit;
            
            var result = await client.V5Api.Trading.PlaceOrderAsync(Category.Linear, symbol, side, type, quantity, price);
            if (!result.Success) throw new Exception("Failed to place order: " + result.Error?.Message);

            var orderResult = await client.V5Api.Trading.GetOrdersAsync(Category.Linear, symbol: symbol, orderId: result.Data.OrderId);
            if (!orderResult.Success || orderResult.Data.List.Count() == 0) throw new Exception("Failed to get order");

            var bybitOrder = orderResult.Data.List[0];
            return ConvertToOrder(bybitOrder);
        }

        public async Task<Order> CancelOrderAsync(string orderId)
        {
            // Placeholder: assume symbol is known or hardcoded; in real scenario, store or retrieve symbol
            string symbol = "XAUTUSDT"; // TODO: Get actual symbol
            var result = await client.V5Api.Trading.CancelOrderAsync(Category.Linear, symbol, orderId: orderId);
            if (!result.Success) throw new Exception("Failed to cancel order: " + result.Error?.Message);

            var orderResult = await client.V5Api.Trading.GetOrdersAsync(Category.Linear, symbol: symbol, orderId: result.Data.OrderId);
            if (!orderResult.Success || orderResult.Data.List.Count() == 0) throw new Exception("Failed to get order");

            var bybitOrder = orderResult.Data.List[0];
            return ConvertToOrder(bybitOrder);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            string symbol = "XAUTUSDT"; // Placeholder
            var result = await client.V5Api.Trading.GetOrdersAsync(Category.Linear, symbol: symbol);
            if (!result.Success) throw new Exception("Failed to get orders: " + result.Error?.Message);

            return result.Data.List.Select(order => ConvertToOrder(order)).ToList();
        }

        public async Task<List<KLine>> GetMarketDataAsync(string symbol, string interval, int limit, DateTime startTime = default, DateTime endTime = default)
        {
            KlineInterval klineInterval = interval switch
            {
                "1m" => KlineInterval.OneMinute,
                "5m" => KlineInterval.FiveMinutes,
                "15m" => KlineInterval.FifteenMinutes,
                "30m" => KlineInterval.ThirtyMinutes,
                "1h" => KlineInterval.OneHour,
                "4h" => KlineInterval.FourHours,
                "1d" => KlineInterval.OneDay,
                _ => KlineInterval.OneHour
            };

            var result = await client.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, symbol, klineInterval, limit: limit, startTime: startTime == default ? null : startTime, endTime: endTime == default ? null : endTime);
            if (!result.Success) throw new Exception("Failed to get klines: " + result.Error?.Message);

            return result.Data.List.Select(k => new KLine(k.StartTime, k.StartTime.AddSeconds((int)klineInterval), k.OpenPrice, k.ClosePrice, k.HighPrice, k.LowPrice, k.Volume)).ToList();
        }

        public Task<bool> StatusAsync()
        {
            // Implementation for status check
            Console.WriteLine("BybitController is operational.");
            try
            {
                var result = client.V5Api.Account.GetBalancesAsync(AccountType.Unified).Result;
                if (result.Success)
                {
                    Console.WriteLine("API connection successful. Wallet balance retrieved.");
                    return Task.FromResult(true);
                }
                else
                {
                    Console.WriteLine("API connection failed: " + result.Error?.Message);
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during status check: " + ex.Message);
                return Task.FromResult(false);
            }
        }
    }
}