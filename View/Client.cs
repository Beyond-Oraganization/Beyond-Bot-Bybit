using System;
using System.Collections.Generic;
using BeyondBot.Controller;

namespace BeyondBot.View
{
    class Client
    {
        
        public async Task CommandLineAsync()
        {
            while (true)
            {
                Console.Write("Beyond-Bot > ");
                string input = Console.ReadLine() ?? "";

                switch (input.ToLower())
                {
                    case "exit":
                        Console.WriteLine("Exiting the application.");
                        return;
                    case "help":
                        Console.WriteLine("Available commands: help - shows all comands\n"
                            + "exit - stops program\n"
                            + "status - shows bot status\n"
                            + "db status - shows database connection status\n"
                            + "restart - restarts the bot\n"
                            + "place order - places a new order\n"
                            + "cancel order - cancels an order\n"
                            + "view orders - displays all orders\n"
                            + "start trading - starts trading operations\n"
                            + "stop trading - stops trading operations\n"
                            + "initialize - initializes the bot\n"
                            + "analyze market - analyzes market data\n"
                            + "show symbols - shows all symbols in database\n"
                            + "add symbol - adds a new symbol to database\n"
                            + "update symbol - updates an existing symbol in database\n"
                            + "delete symbol - deletes a symbol from database\n"
                            + "show timeframes - shows all timeframes in database\n"
                            + "add timeframe - adds a new timeframe to database\n"
                            + "update timeframe - updates an existing timeframe in database\n"
                            + "delete timeframe - deletes a timeframe from database\n"
                            + "show klines - shows all klines in database\n"
                            + "show saved orders - shows all saved orders in database\n"
                            + "delete all saved orders - deletes all saved orders from database\n");
                        break;
                    case "":
                        // Ignore empty input
                        break;

                    #region Additional Commands
                    //Bot Management Commands
                    case "status":
                        if (await Trader.Instance.APIController.StatusAsync())
                            Console.WriteLine("Bot is running smoothly.");
                        else
                            Console.WriteLine("Bot encountered issues.");
                        break;
                    case "db status":
                        Console.WriteLine("Checking database status...");
                        // Implement database status check if needed
                        Trader.Instance.DBController.TestConnection();
                        break;
                    case "restart":
                        Console.WriteLine("Restarting the bot...");
                        break;
                    //Order Management Commands
                    case "place order":
                        try
                        {
                            var order = await Trader.Instance.APIController.PlaceOrderAsync("XAUTUSDT", Model.OrderSide.Buy, Model.OrderType.Market, 0.001m);
                            Console.WriteLine($"Order placed: ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
                            Trader.Instance.DBController.InsertOrder(order);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error placing order: " + ex.Message);
                        }
                        Console.WriteLine("Placing a new order...");
                        break;
                    case "cancel order":
                    try
                        {
                            Console.WriteLine("Cancelling the order...");
                            var orders = Trader.Instance.DBController.GetOrders();
                            var order = orders.LastOrDefault();
                            if (order != null)
                            {
                                await Trader.Instance.APIController.CloseOrderAsync(order);
                                Console.WriteLine($"Order {order.OrderId} cancelled.");
                            }
                            else
                            {
                                Console.WriteLine("No orders found to cancel.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error cancelling order: " + ex.Message);
                        }
                        break;
                    case "view orders":
                        Console.WriteLine("Displaying all orders...");
                        await OrderManager.Instance.ViewOrdersAsync();
                        break;
                    //Trading Commands
                    case "start trading":
                        Console.WriteLine("Starting trading operations...");
                        break;
                    case "stop trading":
                        Console.WriteLine("Stopping trading operations...");
                        break;
                    case "initialize":
                        Console.WriteLine("Initializing the bot...");
                        break;
                    //Market Analysis Commands
                    case "analyze market":
                        Console.WriteLine("Analyzing market data...");
                        break;
                    //Database Management Commands
                    case "show symbols":
                        Console.WriteLine("Fetching symbols from database...");
                        break;
                    case "add symbol":
                        Console.WriteLine("Adding a new symbol to database...");
                        break;
                    case "update symbol":
                        Console.WriteLine("Updating symbol in database...");
                        break;
                    case "delete symbol":
                        Console.WriteLine("Deleting symbol from database...");
                        break;
                    case "show timeframes":
                        Console.WriteLine("Fetching timeframes from database...");
                        break;
                    case "add timeframe":
                        Console.WriteLine("Adding a new timeframe to database...");
                        break;
                    case "update timeframe":
                        Console.WriteLine("Updating timeframe in database...");
                        break;
                    case "delete timeframe":
                        Console.WriteLine("Deleting timeframe from database...");
                        break;
                    case "show klines":
                        Console.WriteLine("Fetching klines from database...");
                        
                        break;
                    case "show saved orders":
                    try
                        {
                            Console.WriteLine("Do you want to filter by symbol? (yes/no)");
                            input = Console.ReadLine() ?? "";

                            if (input.ToLower() == "yes")
                            {
                                Console.Write("Enter symbol: ");
                                string symbol = Console.ReadLine() ?? "";
                                var ordersBySymbol = Trader.Instance.DBController.GetOrders(symbol);
                                foreach (var order in ordersBySymbol)
                                {
                                    Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
                                }
                            }
                            else
                            {
                                var allOrders = Trader.Instance.DBController.GetOrders();
                                foreach (var order in allOrders)
                                {
                                    Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error fetching saved orders: " + ex.Message);
                        }
                        break;
                    case "delete all saved orders":
                        try
                        {
                            Trader.Instance.DBController.DeleteOrder();
                            Console.WriteLine("All saved orders have been deleted from the database.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error deleting saved orders: " + ex.Message);
                        }
                        break;
                    #endregion

                    default:
                        Console.WriteLine($"Unknown command: {input}\nType 'help' for a list of commands.");
                        break;
                } 
            }
        }
    }
}
