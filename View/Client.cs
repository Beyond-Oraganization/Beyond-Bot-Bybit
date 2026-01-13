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
                            + "show saved orders - shows all saved orders in database\n");
                        break;
                    case "":
                        // Ignore empty input
                        break;

                    #region Additional Commands
                    //Bot Management Commands
                    case "status":
                        if (await BybitController.Instance.StatusAsync())
                            Console.WriteLine("Bot is running smoothly.");
                        else
                            Console.WriteLine("Bot encountered issues.");
                        break;
                    case "restart":
                        Console.WriteLine("Restarting the bot...");
                        break;
                    //Order Management Commands
                    case "place order":
                        Console.WriteLine("Placing a new order...");
                        await OrderManager.Instance.PlaceOrderAsync();
                        break;
                    case "cancel order":
                        Console.WriteLine("Cancelling the order...");
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
                        Console.WriteLine("Fetching orders from database...");
                        Console.WriteLine("Do you want to filter by symbol? (yes/no)");
                        input = Console.ReadLine() ?? "";

                        if (input.ToLower() == "yes")
                        {
                            Console.Write("Enter symbol: ");
                            string symbol = Console.ReadLine() ?? "";
                            var ordersBySymbol = .Instance.GetOrders(symbol);
                            foreach (var order in ordersBySymbol)
                            {
                                Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
                            }
                        }
                        else
                        {
                            var allOrders = ModelManager.Instance.Get;
                            foreach (var order in allOrders)
                            {
                                Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
                            }
                        }
                    #endregion

                    default:
                        Console.WriteLine($"Unknown command: {input}\nType 'help' for a list of commands.");
                        break;
                } 
            }
        }
    }
}
