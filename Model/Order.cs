using System;
using System.Collections.Generic;

namespace BeyondBot.Model
{
    class Order
    {
        public string OrderId { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderStatus Status { get; set; }
        public OrderType Type { get; set; }
        public OrderSide Side { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order(string orderId, string symbol, decimal quantity, decimal price, OrderStatus status, OrderType type, OrderSide side)
        {
            OrderId = orderId;
            Symbol = symbol;
            Quantity = quantity;
            Price = price;
            Status = status;
            Type = type;
            Side = side;
            CreatedAt = DateTime.Now;
        }

        public Order(string orderId, string symbol, decimal quantity, decimal price, OrderStatus status, OrderType type, OrderSide side, DateTime createdAt)
        {
            OrderId = orderId;
            Symbol = symbol;
            Quantity = quantity;
            Price = price;
            Status = status;
            Type = type;
            Side = side;
            CreatedAt = createdAt;
        }
    }

    enum OrderType
    {
        Market,
        Limit,
        StopLoss,
        TakeProfit
    }

    enum OrderStatus
    {
        New,
        PartiallyFilled,
        Filled,
        Canceled,
        Rejected
    }

    enum OrderSide
    {
        Buy,
        Sell
    }
}