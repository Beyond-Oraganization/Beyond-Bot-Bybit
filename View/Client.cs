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
                            + "symbol - manages symbols in database (+ 'help' for more)\n"
                            + "timeframe - manages timeframes in database (+ 'help' for more)\n"
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
                        await Trader.Instance.InitializeMarketAsync("XAUTUSDT", Model.TimeframeInterval.OneHour);
                        break;
                    //Market Analysis Commands
                    case "analyze market":
                        Console.WriteLine("Analyzing market data...");
                        break;
                    //Database Management Commands
                    case "symbol":
                        input = Console.ReadLine() ?? "";
                        switch (input.ToLower())
                        {
                            case "help":
                                Console.WriteLine("Symbol commands: show - shows all symbols in database\n"
                                    + "add - adds a new symbol to database\n"
                                    + "update - updates an existing symbol in database\n"
                                    + "delete - deletes a symbol from database\n");
                                break;
                            case "show":
                                Console.WriteLine("Fetching symbols from database...");
                                try
                                {
                                    var symbols = Trader.Instance.DBController.GetSymbols();
                                    if (symbols.Count == 0)
                                    {
                                        Console.WriteLine("No symbols found in the database.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Found {symbols.Count} symbol(s):");
                                        foreach (var symbol in symbols)
                                        {
                                            Console.WriteLine($"ID: {symbol.ID}, Symbol Name: {symbol.SymbolName}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error fetching symbols: " + ex.Message);
                                }
                                break;
                            
                            case "add":
                                // Handled in "add symbol" case

                                    try
                                    {
                                        Console.WriteLine("Adding a new symbol to database...");
                                        Console.Write("Enter symbol name:");
                                        string symbolName = Console.ReadLine() ?? "";
                                        if (string.IsNullOrWhiteSpace(symbolName))
                                        {
                                            Console.WriteLine("Invalid symbol name.");
                                            break;
                                        }
                                        Trader.Instance.DBController.InsertSymbol(symbolName);
                                        System.Console.WriteLine("Symbol added successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error adding symbol: " + ex.Message);
                                    }
                                break;
                            case "update":
                                // Handled in "update symbol" case

                                try
                                {
                                    Console.WriteLine("Updating symbol in database...");
                                    Console.Write("Enter symbol name:");
                                    string symbolName = Console.ReadLine() ?? "";
                                    if (string.IsNullOrWhiteSpace(symbolName))
                                    {
                                        Console.WriteLine("Invalid symbol name.");
                                        break;
                                    }

                                    var existingSymbol = Trader.Instance.DBController.GetSymbols(symbolName).First();
                                    if (existingSymbol == null)
                                    {
                                        Console.WriteLine("Symbol not found in database.");
                                        break;
                                    }

                                    Console.Write("Enter new symbol name: ");
                                    string newSymbolName = Console.ReadLine() ?? "";
                                    if (string.IsNullOrWhiteSpace(newSymbolName))
                                    {
                                        Console.WriteLine("Invalid new symbol name.");
                                        break;
                                    }
                                    Trader.Instance.DBController.UpdateSymbol(existingSymbol.ID, newSymbolName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error updating symbol: " + ex.Message);
                                }
                                break;
                            case "delete":
                                // Handled in "delete symbol" case
                                try
                                {
                                    Console.WriteLine("Deleting symbol from database...");
                                    Console.Write("Enter symbol name:");
                                    string symbolName = Console.ReadLine() ?? "";
                                    if (string.IsNullOrWhiteSpace(symbolName))
                                    {
                                        Console.WriteLine("Invalid symbol name.");
                                        break;
                                    }

                                    var existingSymbol = Trader.Instance.DBController.GetSymbols(symbolName).First();
                                    if (existingSymbol == null)
                                    {
                                        Console.WriteLine("Symbol not found in database.");
                                        break;
                                    }

                                    Trader.Instance.DBController.DeleteSymbol(existingSymbol.ID);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error deleting symbol: " + ex.Message);
                                }
                                break;
                            default:
                                Console.WriteLine($"Unknown symbol command: {input}. Write 'symbol help' for available commands.");
                                break;
                        }
                        break;
                    case "timeframe":
                        input = Console.ReadLine() ?? "";
                        switch (input.ToLower())
                        {
                            case "help":
                                Console.WriteLine("Timeframe commands: show - shows all timeframes in database\n"
                                    + "add - adds a new timeframe to database\n"
                                    + "update - updates an existing timeframe in database\n"
                                    + "delete - deletes a timeframe from database\n");
                                break;
                            case "show":
                                // Handled in "show timeframes" case

                                Console.WriteLine("Fetching timeframes from database...");
                                try
                                {
                                    Console.Write("Enter symbol name to filter by (or press Enter to skip): ");
                                    string symbolName = Console.ReadLine() ?? "";

                                    List<Model.Timeframe> timeframes;
                                    if (string.IsNullOrWhiteSpace(symbolName))
                                    {
                                        timeframes = Trader.Instance.DBController.GetTimeframes();
                                    }
                                    else
                                    {
                                        var symbol = Trader.Instance.DBController.GetSymbols(symbolName).FirstOrDefault();
                                        if (symbol == null)
                                        {
                                            Console.WriteLine("Symbol not found in database.");
                                            break;
                                        }
                                        timeframes = Trader.Instance.DBController.GetTimeframes(symbol.ID);
                                    }

                                    foreach (var timeframe in timeframes)
                                    {
                                        Console.WriteLine($"ID: {timeframe.ID}, SymbolID: {timeframe.SymbolID}, Interval: {timeframe.Interval}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error fetching timeframes: " + ex.Message);
                                }
                                break;
                            case "add":
                                // Handled in "add timeframe" case
                                Console.WriteLine("Adding a new timeframe to database...");
                                try
                                {
                                    Console.Write("Enter symbol name for the timeframe: ");
                                    string symbolName = Console.ReadLine() ?? "";
                                    var symbol = Trader.Instance.DBController.GetSymbols(symbolName).FirstOrDefault();
                                    if (symbol == null)
                                    {
                                        Console.WriteLine("Symbol not found in database.");
                                        break;
                                    }

                                    Console.Write("Enter timeframe interval (e.g., OneMinute, FiveMinutes): ");
                                    string intervalInput = Console.ReadLine() ?? "";
                                    if (!Enum.TryParse(intervalInput, out Model.TimeframeInterval interval))
                                    {
                                        Console.WriteLine("Invalid timeframe interval.");
                                        break;
                                    }

                                    var timeframe = new Model.Timeframe(symbol.ID, interval);
                                    Trader.Instance.DBController.InsertTimeframe(timeframe);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error adding timeframe: " + ex.Message);
                                }
                                break;
                            case "delete":
                                // Handled in "delete timeframe" case

                                Console.WriteLine("Deleting timeframe from database...");
                                try
                                {
                                    Console.Write("Enter timeframe ID to delete: ");
                                    string idInput = Console.ReadLine() ?? "";
                                    if (!int.TryParse(idInput, out int timeframeID))
                                    {
                                        Console.WriteLine("Invalid timeframe ID.");
                                        break;
                                    }

                                    Trader.Instance.DBController.DeleteTimeframe(timeframeID);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error deleting timeframe: " + ex.Message);
                                }
                                break;
                            default:
                                Console.WriteLine($"Unknown timeframe command: {input}. Write 'timeframe help' for available commands.");
                                break;
                        }
                        break;
                    case "kline":
                        input = Console.ReadLine() ?? "";
                        switch (input.ToLower())
                        {
                            case "help":
                                Console.WriteLine("KLine commands: show - shows all klines in database\n");
                                break;
                            case "show":
                                // Handled in "show klines" case
                                Console.WriteLine("Fetching klines from database...");
                                try
                                {
                                    Console.Write("Enter symbol name to filter by (or press Enter to skip): ");
                                    string symbolName = Console.ReadLine() ?? "";
                                    Console.Write("Enter timeframe interval to filter by (or press Enter to skip): ");
                                    string intervalInput = Console.ReadLine() ?? "";

                                    List<Model.KLine> klines;
                                    if (string.IsNullOrWhiteSpace(symbolName) && string.IsNullOrWhiteSpace(intervalInput))
                                    {
                                        klines = Trader.Instance.DBController.GetKLines();
                                    }
                                    else if (!string.IsNullOrWhiteSpace(symbolName) && string.IsNullOrWhiteSpace(intervalInput))
                                    {
                                        var symbol = Trader.Instance.DBController.GetSymbols(symbolName).FirstOrDefault();
                                        if (symbol == null)
                                        {
                                            Console.WriteLine("Symbol not found in database.");
                                            break;
                                        }
                                        klines = new List<Model.KLine>();
                                        var timeframes = Trader.Instance.DBController.GetTimeframes(symbol.ID);
                                        foreach (var timeframe in timeframes)
                                        {
                                            klines.AddRange(Trader.Instance.DBController.GetKLines(symbol, timeframe.Interval));
                                        }
                                    }
                                    else if (!string.IsNullOrWhiteSpace(symbolName) && !string.IsNullOrWhiteSpace(intervalInput))
                                    {
                                        var symbol = Trader.Instance.DBController.GetSymbols(symbolName).FirstOrDefault();
                                        if (symbol == null)
                                        {
                                            Console.WriteLine("Symbol not found in database.");
                                            break;
                                        }
                                        if (!Enum.TryParse(intervalInput, out Model.TimeframeInterval interval))
                                        {
                                            Console.WriteLine("Invalid timeframe interval.");
                                            break;
                                        }
                                        klines = Trader.Instance.DBController.GetKLines(symbol, interval);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Timeframe interval provided without symbol name. Please provide a symbol name.");
                                        break;
                                    }

                                    foreach (var kline in klines)
                                    {
                                        Console.WriteLine($"StartTime: {kline.OpenTime}, EndTime: {kline.CloseTime}, Open: {kline.OpenPrice}, Close: {kline.ClosePrice}, High: {kline.HighPrice}, Low: {kline.LowPrice}, Volume: {kline.Volume}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error fetching klines: " + ex.Message);
                                }
                                break;
                            case "show oldest":
                                // Handled in "show oldest kline" case
                                try
                                {
                                    Console.WriteLine("Fetching oldest kline from API...");
                                    Console.Write("Enter symbol name: ");
                                    string symbolName = Console.ReadLine() ?? "XAUTUSDT";
                                    Console.Write("Enter timeframe interval (e.g., 1m, 5m, 15m, 30m, 1h, 4h, 1d): ");
                                    string intervalInput = Console.ReadLine() ?? "1M";

                                    var oldestKline = await Trader.Instance.APIController.GetOldestKLineAsync(symbolName, intervalInput);
                                    Console.WriteLine($"Oldest KLine - StartTime: {oldestKline.OpenTime}, EndTime: {oldestKline.CloseTime}, Open: {oldestKline.OpenPrice}, Close: {oldestKline.ClosePrice}, High: {oldestKline.HighPrice}, Low: {oldestKline.LowPrice}, Volume: {oldestKline.Volume}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error fetching oldest kline: " + ex.Message);
                                }
                                break;
                            case "delete":
                                // Handled in "delete klines" case
                                try
                                {
                                    Console.WriteLine("Deleting klines from database...");
                                    // Implementation for deleting klines if needed
                                    var klines = Trader.Instance.DBController.GetKLines();
                                    foreach (var kline in klines)
                                    {
                                        Trader.Instance.DBController.DeleteKLine(kline);
                                    }
                                    Console.WriteLine("All klines have been deleted from the database.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error deleting klines: " + ex.Message);
                                }
                                break;
                            default:
                                Console.WriteLine($"Unknown kline command: {input}. Write 'klines help' for available commands.");
                                break;
                        }
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
