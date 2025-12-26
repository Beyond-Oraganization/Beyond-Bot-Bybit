// See https://aka.ms/new-console-template for more information
using System.Threading;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("BeyondBot_Bybit started. Running 24/7...");

while (true)
{
    try
    {
        // Placeholder for bot logic
        // Add your trading bot code here
        Log.Information($"Bot is running... {DateTime.Now}");

        // Sleep for a short interval to avoid high CPU usage
        Thread.Sleep(1000); // 1 second, adjust as needed
    }
    catch (Exception ex)
    {
        Log.Error($"Error: {ex.Message}");
        // Optionally, add logging or recovery logic
    }
}
