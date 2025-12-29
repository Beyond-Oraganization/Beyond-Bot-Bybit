using System;
using System.Collections.Generic;
using BeyondBot.Model;

namespace BeyondBot.Controller
{
    interface IAPIController
    {
        Task<bool> StatusAsync();
        Task<Order> PlaceOrderAsync(string symbol, OrderSide orderSide, OrderType orderType, decimal quantity, decimal? price = null);
        Task<Order> CancelOrderAsync(string orderId);
        Task<List<Order>> GetOrdersAsync();
        Task<List<KLine>> GetMarketDataAsync(string symbol, string interval, int limit, DateTime startTime = default, DateTime endTime = default);
    }
}