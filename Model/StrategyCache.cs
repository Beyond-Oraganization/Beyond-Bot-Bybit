using System;

namespace BeyondBot.Model
{
    /// <summary>
    /// Represents a trading strategy cache associated with a specific KLine.
    /// This class stores strategy data as a string, allowing flexibility for different strategy types.
    /// </summary>
    public class StrategyCache
    {
        /// <summary>
        /// Unique identifier for the strategy record.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Foreign key referencing the ID of the associated KLine.
        /// </summary>
        public int KLineID { get; set; }

        /// <summary>
        /// The data of the strategy, stored as a string. This can contain JSON or any serialized data for the strategy logic.
        /// </summary>
        public string StrategyData { get; set; }

        /// <summary>
        /// Constructor for creating a new strategy cache (without ID, as it's auto-generated).
        /// </summary>
        /// <param name="kLineID">The ID of the KLine this strategy cache is associated with.</param>
        /// <param name="strategyData">The strategy data as a string.</param>
        public StrategyCache(int kLineID, string strategyData)
        {
            KLineID = kLineID;
            StrategyData = strategyData;
        }

        /// <summary>
        /// Constructor for loading an existing strategy cache from the database.
        /// </summary>
        /// <param name="id">The unique ID of the strategy cache.</param>
        /// <param name="kLineID">The ID of the associated KLine.</param>
        /// <param name="strategyData">The strategy data.</param>
        public StrategyCache(int id, int kLineID, string strategyData)
        {
            ID = id;
            KLineID = kLineID;
            StrategyData = strategyData;
        }
    }
}