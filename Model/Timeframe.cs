using System;

namespace BeyondBot.Model
{
    /// <summary>
    /// Enum representing common timeframe intervals for trading.
    /// </summary>
    public enum TimeframeInterval
    {
        OneMinute,
        ThreeMinutes,
        FiveMinutes,
        FifteenMinutes,
        ThirtyMinutes,
        OneHour,
        TwoHours,
        FourHours,
        SixHours,
        EightHours,
        TwelveHours,
        OneDay,
        ThreeDays,
        OneWeek,
        OneMonth
    }

    /// <summary>
    /// Represents a timeframe associated with a symbol.
    /// </summary>
    public class Timeframe
    {
        /// <summary>
        /// Unique identifier for the timeframe record.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Foreign key referencing the ID of the associated Symbol.
        /// </summary>
        public int SymbolID { get; set; }

        /// <summary>
        /// The timeframe interval.
        /// </summary>
        public TimeframeInterval Interval { get; set; }

        /// <summary>
        /// Constructor for creating a new timeframe (without ID, as it's auto-generated).
        /// </summary>
        /// <param name="symbolID">The ID of the associated symbol.</param>
        /// <param name="interval">The timeframe interval.</param>
        public Timeframe(int symbolID, TimeframeInterval interval)
        {
            SymbolID = symbolID;
            Interval = interval;
        }

        /// <summary>
        /// Constructor for loading an existing timeframe from the database.
        /// </summary>
        /// <param name="id">The unique ID of the timeframe.</param>
        /// <param name="symbolID">The ID of the associated symbol.</param>
        /// <param name="interval">The timeframe interval.</param>
        public Timeframe(int id, int symbolID, TimeframeInterval interval)
        {
            ID = id;
            SymbolID = symbolID;
            Interval = interval;
        }

        /// <summary>
        /// Converts the enum to a string representation (e.g., "1m", "5m").
        /// </summary>
        /// <param name="interval">The timeframe interval.</param>
        /// <returns>String representation of the interval.</returns>
        public static string IntervalToString(TimeframeInterval interval)
        {
            return interval switch
            {
                TimeframeInterval.OneMinute => "1m",
                TimeframeInterval.ThreeMinutes => "3m",
                TimeframeInterval.FiveMinutes => "5m",
                TimeframeInterval.FifteenMinutes => "15m",
                TimeframeInterval.ThirtyMinutes => "30m",
                TimeframeInterval.OneHour => "1h",
                TimeframeInterval.TwoHours => "2h",
                TimeframeInterval.FourHours => "4h",
                TimeframeInterval.SixHours => "6h",
                TimeframeInterval.EightHours => "8h",
                TimeframeInterval.TwelveHours => "12h",
                TimeframeInterval.OneDay => "1d",
                TimeframeInterval.ThreeDays => "3d",
                TimeframeInterval.OneWeek => "1w",
                TimeframeInterval.OneMonth => "1M",
                _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
            };
        }

        /// <summary>
        /// Converts a string representation to the enum (e.g., "1m" to OneMinute).
        /// </summary>
        /// <param name="intervalString">The string representation of the interval.</param>
        /// <returns>The corresponding TimeframeInterval enum value.</returns>
        public static TimeframeInterval StringToInterval(string intervalString)
        {
            return intervalString switch
            {
                "1m" => TimeframeInterval.OneMinute,
                "3m" => TimeframeInterval.ThreeMinutes,
                "5m" => TimeframeInterval.FiveMinutes,
                "15m" => TimeframeInterval.FifteenMinutes,
                "30m" => TimeframeInterval.ThirtyMinutes,
                "1h" => TimeframeInterval.OneHour,
                "2h" => TimeframeInterval.TwoHours,
                "4h" => TimeframeInterval.FourHours,
                "6h" => TimeframeInterval.SixHours,
                "8h" => TimeframeInterval.EightHours,
                "12h" => TimeframeInterval.TwelveHours,
                "1d" => TimeframeInterval.OneDay,
                "3d" => TimeframeInterval.ThreeDays,
                "1w" => TimeframeInterval.OneWeek,
                "1M" => TimeframeInterval.OneMonth,
                _ => throw new ArgumentException($"Unknown timeframe interval: {intervalString}", nameof(intervalString))
            };
        }
    }
}