using BeyondBot.Model;
using Bybit.Net.Clients;
using Bybit.Net.Objects.Models.V5;
using Bybit.Net.Enums;
using CryptoExchange.Net.Authentication;

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
            Model.OrderStatus status; 
            switch (bybitOrder.Status)
            {
                case Bybit.Net.Enums.OrderStatus.New:
                    status = Model.OrderStatus.New;
                    break;
                case Bybit.Net.Enums.OrderStatus.PartiallyFilled:
                    status = Model.OrderStatus.PartiallyFilled;
                    break;
                case Bybit.Net.Enums.OrderStatus.Filled:
                    status = Model.OrderStatus.Filled;
                    break;
                case Bybit.Net.Enums.OrderStatus.Cancelled:
                    status = Model.OrderStatus.Canceled;
                    break;
                case Bybit.Net.Enums.OrderStatus.Rejected:
                    status = Model.OrderStatus.Rejected;
                    break;
                default:
                    status = Model.OrderStatus.None;
                    break;
            }
            
            Model.OrderType type;
            switch (bybitOrder.OrderType)
            {
                case Bybit.Net.Enums.OrderType.Market:
                    type = Model.OrderType.Market;
                    break;
                case Bybit.Net.Enums.OrderType.Limit:
                    type = Model.OrderType.Limit;
                    break;
                default:
                    type = Model.OrderType.None;
                    break;
            }

            Model.OrderSide side;
            switch (bybitOrder.Side)
            {
                case Bybit.Net.Enums.OrderSide.Buy:
                    side = Model.OrderSide.Buy;
                    break;
                case Bybit.Net.Enums.OrderSide.Sell:
                    side = Model.OrderSide.Sell;
                    break;
                default:
                    side = Model.OrderSide.None;
                    break;
            }

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

        public async Task<Order> CloseOrderAsync(Order order)
        {
            // Placeholder: assume symbol is known or hardcoded; in real scenario, store or retrieve symbol
            string symbol = "XAUTUSDT"; // TODO: Get actual symbol
            string orderId = order.OrderId;
            if(order.Side == Model.OrderSide.Buy)
            {
                var result = await client.V5Api.Trading.PlaceOrderAsync(Category.Linear, order.Symbol, Bybit.Net.Enums.OrderSide.Sell, NewOrderType.Market, order.Quantity);
                if (!result.Success) throw new Exception("Failed to cancel order: " + result.Error?.Message);
            }
            else
            {
                var result = await client.V5Api.Trading.PlaceOrderAsync(Category.Linear, order.Symbol, Bybit.Net.Enums.OrderSide.Buy, NewOrderType.Market, order.Quantity);
                if (!result.Success) throw new Exception("Failed to cancel order: " + result.Error?.Message);
            }

            var orderResult = await client.V5Api.Trading.GetOrdersAsync(Category.Linear, symbol: symbol, orderId: orderId);
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
                "1w" => KlineInterval.OneWeek,
                "1M" => KlineInterval.OneMonth,
                _ => KlineInterval.OneHour
            };
            
            var result = await client.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, symbol, klineInterval, limit: limit, startTime: startTime == default ? null : startTime, endTime: endTime == default ? null : endTime);
            if (!result.Success) throw new Exception("Failed to get klines: " + result.Error?.Message);
            
            return result.Data.List.Select(k => new KLine(k.StartTime, k.StartTime.AddSeconds(Convert.ToInt32(klineInterval)), k.OpenPrice, k.ClosePrice, k.HighPrice, k.LowPrice, k.Volume)).ToList();
        }
        public async Task<KLine> GetOldestKLineAsync(string symbol = "XAUTUSDT", string interval = "1M")
        {
            var klines = await GetMarketDataAsync(symbol, interval, 1000, startTime: new DateTime(2020,5,1), endTime: DateTime.Now);
            foreach (var kline in klines)
            {
                Console.WriteLine($"KLine Open Time: {kline.OpenTime}, Close Time: {kline.CloseTime}");
            }
            if (klines.Count == 0) throw new Exception("No klines found for the specified time.");
            return klines.OrderBy(k => k.OpenTime).First();
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