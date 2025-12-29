using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeyondBot.View;

namespace BeyondBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Client client = new Client();
            await client.CommandLineAsync();
        }
    }
}
