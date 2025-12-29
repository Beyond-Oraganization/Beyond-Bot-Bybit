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
                            + "analyze market - analyzes market data\n");
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
                    #endregion

                    default:
                        Console.WriteLine($"Unknown command: {input}\nType 'help' for a list of commands.");
                        break;
                } 
            }
        }
    }
}
