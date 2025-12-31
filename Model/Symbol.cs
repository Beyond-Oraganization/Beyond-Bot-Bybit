using System;

namespace BeyondBot.Model
{
    /// <summary>
    /// Represents a trading symbol (e.g., BTCUSDT, ETHUSDT).
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Unique identifier for the symbol record.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The symbol name (e.g., "BTCUSDT").
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Constructor for creating a new symbol (without ID, as it's auto-generated).
        /// </summary>
        /// <param name="symbolName">The name of the symbol.</param>
        public Symbol(string symbolName)
        {
            SymbolName = symbolName;
        }

        /// <summary>
        /// Constructor for loading an existing symbol from the database.
        /// </summary>
        /// <param name="id">The unique ID of the symbol.</param>
        /// <param name="symbolName">The name of the symbol.</param>
        public Symbol(int id, string symbolName)
        {
            ID = id;
            SymbolName = symbolName;
        }
    }
}