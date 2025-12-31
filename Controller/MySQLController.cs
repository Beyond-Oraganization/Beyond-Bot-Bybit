using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BeyondBot.Model;

namespace BeyondBot.Controller
{
    /// <summary>
    /// Controller class for interacting with the MySQL database.
    /// Provides CRUD operations for KLines, Orders, Strategies, Symbols, and Timeframes.
    /// Uses MySQL Connector/NET for database connectivity.
    /// </summary>
    public class MySQLController
    {
        /// <summary>
        /// Connection string for the MySQL database.
        /// Update this with your actual database credentials and server details.
        /// </summary>
        private readonly string connectionString = "server=localhost;database=BeyondBotDB;uid=root;pwd=;";

        // Symbol CRUD Operations

        /// <summary>
        /// Retrieves all Symbols from the database.
        /// Note: This method fetches all records; consider adding filters for large datasets.
        /// </summary>
        /// <returns>A list of all Symbol objects.</returns>
        public List<Symbol> GetSymbols()
        {
            var list = new List<Symbol>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, Symbol FROM Symbols", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var symbol = new Symbol(
                            reader.GetInt32("ID"),
                            reader.GetString("Symbol")
                        );
                        list.Add(symbol);
                    }
                }
            }
            return list;
        }

        public List<Symbol> GetSymbols(string symbolName)
        {
            var list = new List<Symbol>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, Symbol FROM Symbols WHERE Symbol=@symbolName", conn);
                cmd.Parameters.AddWithValue("@symbolName", symbolName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var symbol = new Symbol(
                            reader.GetInt32("ID"),
                            reader.GetString("Symbol")
                        );
                        list.Add(symbol);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Inserts a new Symbol into the database.
        /// </summary>
        /// <param name="symbol">The Symbol object to insert (ID will be auto-generated).</param>
        public void InsertSymbol(string symbolName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO Symbols (Symbol) VALUES (@s)", conn);
                cmd.Parameters.AddWithValue("@s", symbolName);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an existing Symbol in the database.
        /// </summary>
        /// <param name="id">The ID of the Symbol to update.</param>
        /// <param name="symbol">The updated Symbol object.</param>
        public void UpdateSymbol(int id, string symbolName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE Symbols SET Symbol=@s WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@s", symbolName);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a Symbol from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the Symbol to delete.</param>
        public void DeleteSymbol(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Symbols WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSymbol(string symbolName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Symbols WHERE Symbol=@symbolName", conn);
                cmd.Parameters.AddWithValue("@symbolName", symbolName);
                cmd.ExecuteNonQuery();
            }
        }
        // Timeframe CRUD Operations

        /// <summary>
        /// Retrieves all Timeframes from the database.
        /// Note: This method fetches all records; consider adding filters for large datasets.
        /// </summary>
        /// <returns>A list of all Timeframe objects.</returns>
        public List<Timeframe> GetTimeframes()
        {
            var list = new List<Timeframe>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, SymbolID, Timeframe FROM Timeframes", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var timeframe = new Timeframe(
                            reader.GetInt32("ID"),
                            reader.GetInt32("SymbolID"),
                            Timeframe.StringToInterval(reader.GetString("Timeframe"))
                        );
                        list.Add(timeframe);
                    }
                }
            }
            return list;
        }

        public List<Timeframe> GetTimeframes(int symbolID)
        {
            var list = new List<Timeframe>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, SymbolID, Timeframe FROM Timeframes WHERE SymbolID=@symbolID", conn);
                cmd.Parameters.AddWithValue("@symbolID", symbolID);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var timeframe = new Timeframe(
                            reader.GetInt32("ID"),
                            reader.GetInt32("SymbolID"),
                            Timeframe.StringToInterval(reader.GetString("Timeframe"))
                        );
                        list.Add(timeframe);
                    }
                }
            }
            return list;
        }

        public List<Timeframe> GetTimeframes(int symbolID, TimeframeInterval interval)
        {
            var list = new List<Timeframe>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, SymbolID, Timeframe FROM Timeframes WHERE SymbolID=@symbolID AND Timeframe=@interval", conn);
                cmd.Parameters.AddWithValue("@symbolID", symbolID);
                cmd.Parameters.AddWithValue("@interval", Timeframe.IntervalToString(interval));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var timeframe = new Timeframe(
                            reader.GetInt32("ID"),
                            reader.GetInt32("SymbolID"),
                            Timeframe.StringToInterval(reader.GetString("Timeframe"))
                        );
                        list.Add(timeframe);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Inserts a new Timeframe into the database.
        /// </summary>
        /// <param name="timeframe">The Timeframe object to insert (ID will be auto-generated).</param>
        public void InsertTimeframe(Timeframe timeframe)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO Timeframes (SymbolID, Timeframe) VALUES (@sid, @tf)", conn);
                cmd.Parameters.AddWithValue("@sid", timeframe.SymbolID);
                cmd.Parameters.AddWithValue("@tf", Timeframe.IntervalToString(timeframe.Interval));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an existing Timeframe in the database.
        /// </summary>
        /// <param name="id">The ID of the Timeframe to update.</param>
        /// <param name="timeframe">The updated Timeframe object.</param>
        public void UpdateTimeframe(int id, Timeframe timeframe)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE Timeframes SET SymbolID=@sid, Timeframe=@tf WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@sid", timeframe.SymbolID);
                cmd.Parameters.AddWithValue("@tf", Timeframe.IntervalToString(timeframe.Interval));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a Timeframe from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the Timeframe to delete.</param>
        public void DeleteTimeframe(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Timeframes WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // KLine CRUD Operations

        /// <summary>
        /// Retrieves all KLines from the database.
        /// Note: This method fetches all records; consider adding filters for large datasets.
        /// </summary>
        /// <returns>A list of all KLine objects.</returns>
        public List<KLine> GetKLines()
        {
            var list = new List<KLine>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT OpenTime, CloseTime, OpenPrice, ClosePrice, HighPrice, LowPrice, Volume FROM KLines", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var kline = new KLine(
                            reader.GetDateTime("OpenTime"),
                            reader.GetDateTime("CloseTime"),
                            reader.GetDecimal("OpenPrice"),
                            reader.GetDecimal("ClosePrice"),
                            reader.GetDecimal("HighPrice"),
                            reader.GetDecimal("LowPrice"),
                            reader.GetDecimal("Volume")
                        );
                        list.Add(kline);
                    }
                }
            }
            return list;
        }

        public List<KLine> GetKlines(Symbol symbol, TimeframeInterval interval)
        {
            var list = new List<KLine>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
                SELECT k.OpenTime, k.CloseTime, k.OpenPrice, k.ClosePrice, k.HighPrice, k.LowPrice, k.Volume
                FROM KLines k
                JOIN Timeframes t ON k.TimeframeID = t.ID
                JOIN Symbols s ON t.SymbolID = s.ID
                WHERE s.Symbol = @symbolName AND t.Timeframe = @interval", conn);
                cmd.Parameters.AddWithValue("@symbolName", symbol.SymbolName);
                cmd.Parameters.AddWithValue("@interval", Timeframe.IntervalToString(interval));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var kline = new KLine(
                            reader.GetDateTime("OpenTime"),
                            reader.GetDateTime("CloseTime"),
                            reader.GetDecimal("OpenPrice"),
                            reader.GetDecimal("ClosePrice"),
                            reader.GetDecimal("HighPrice"),
                            reader.GetDecimal("LowPrice"),
                            reader.GetDecimal("Volume")
                        );
                        list.Add(kline);
                    }
                }
            }
            return list;
        }

        public List<KLine> GetKlines(Symbol symbol, TimeframeInterval interval, DateTime startTime, DateTime endTime)
        {
            var list = new List<KLine>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
                SELECT k.OpenTime, k.CloseTime, k.OpenPrice, k.ClosePrice, k.HighPrice, k.LowPrice, k.Volume
                FROM KLines k
                JOIN Timeframes t ON k.TimeframeID = t.ID
                JOIN Symbols s ON t.SymbolID = s.ID
                WHERE s.Symbol = @symbolName AND t.Timeframe = @interval AND k.OpenTime >= @startTime AND k.CloseTime <= @endTime", conn);
                cmd.Parameters.AddWithValue("@symbolName", symbol.SymbolName);
                cmd.Parameters.AddWithValue("@interval", Timeframe.IntervalToString(interval));
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@endTime", endTime);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var kline = new KLine(
                            reader.GetDateTime("OpenTime"),
                            reader.GetDateTime("CloseTime"),
                            reader.GetDecimal("OpenPrice"),
                            reader.GetDecimal("ClosePrice"),
                            reader.GetDecimal("HighPrice"),
                            reader.GetDecimal("LowPrice"),
                            reader.GetDecimal("Volume")
                        );
                        list.Add(kline);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Inserts a new KLine into the database.
        /// </summary>
        /// <param name="kline">The KLine object to insert.</param>
        /// <param name="timeframeID">The ID of the associated Timeframe.</param>
        public void InsertKLine(KLine kline, int timeframeID)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO KLines (TimeframeID, OpenTime, CloseTime, OpenPrice, ClosePrice, HighPrice, LowPrice, Volume) VALUES (@tid, @ot, @ct, @op, @cp, @hp, @lp, @v)", conn);
                cmd.Parameters.AddWithValue("@tid", timeframeID);
                cmd.Parameters.AddWithValue("@ot", kline.OpenTime);
                cmd.Parameters.AddWithValue("@ct", kline.CloseTime);
                cmd.Parameters.AddWithValue("@op", kline.OpenPrice);
                cmd.Parameters.AddWithValue("@cp", kline.ClosePrice);
                cmd.Parameters.AddWithValue("@hp", kline.HighPrice);
                cmd.Parameters.AddWithValue("@lp", kline.LowPrice);
                cmd.Parameters.AddWithValue("@v", kline.Volume);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an existing KLine in the database.
        /// </summary>
        /// <param name="id">The ID of the KLine to update.</param>
        /// <param name="kline">The updated KLine object.</param>
        public void UpdateKLine(int id, KLine kline)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE KLines SET OpenTime=@ot, CloseTime=@ct, OpenPrice=@op, ClosePrice=@cp, HighPrice=@hp, LowPrice=@lp, Volume=@v WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ot", kline.OpenTime);
                cmd.Parameters.AddWithValue("@ct", kline.CloseTime);
                cmd.Parameters.AddWithValue("@op", kline.OpenPrice);
                cmd.Parameters.AddWithValue("@cp", kline.ClosePrice);
                cmd.Parameters.AddWithValue("@hp", kline.HighPrice);
                cmd.Parameters.AddWithValue("@lp", kline.LowPrice);
                cmd.Parameters.AddWithValue("@v", kline.Volume);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a KLine from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the KLine to delete.</param>
        public void DeleteKLine(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM KLines WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // Order CRUD Operations

        /// <summary>
        /// Retrieves all Orders from the database.
        /// Note: This method fetches all records; consider adding filters for large datasets.
        /// </summary>
        /// <returns>A list of all Order objects.</returns>
        public List<Order> GetOrders()
        {
            var list = new List<Order>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT OrderId, Symbol, Quantity, Price, Status, Type, Side, CreatedAt FROM Orders", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var order = new Order(
                            reader.GetString("OrderId"),
                            reader.GetString("Symbol"),
                            reader.GetDecimal("Quantity"),
                            reader.GetDecimal("Price"),
                            Enum.Parse<OrderStatus>(reader.GetString("Status")),
                            Enum.Parse<OrderType>(reader.GetString("Type")),
                            Enum.Parse<OrderSide>(reader.GetString("Side")),
                            reader.GetDateTime("CreatedAt")
                        );
                        list.Add(order);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Inserts a new Order into the database.
        /// </summary>
        /// <param name="order">The Order object to insert.</param>
        public void InsertOrder(Order order)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO Orders (OrderId, Symbol, Quantity, Price, Status, Type, Side, CreatedAt) VALUES (@oid, @s, @q, @p, @st, @t, @sd, @ct)", conn);
                cmd.Parameters.AddWithValue("@oid", order.OrderId);
                cmd.Parameters.AddWithValue("@s", order.Symbol);
                cmd.Parameters.AddWithValue("@q", order.Quantity);
                cmd.Parameters.AddWithValue("@p", order.Price);
                cmd.Parameters.AddWithValue("@st", order.Status.ToString());
                cmd.Parameters.AddWithValue("@t", order.Type.ToString());
                cmd.Parameters.AddWithValue("@sd", order.Side.ToString());
                cmd.Parameters.AddWithValue("@ct", order.CreatedAt);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an existing Order in the database.
        /// </summary>
        /// <param name="orderId">The OrderId of the Order to update.</param>
        /// <param name="order">The updated Order object.</param>
        public void UpdateOrder(string orderId, Order order)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE Orders SET Symbol=@s, Quantity=@q, Price=@p, Status=@st, Type=@t, Side=@sd, CreatedAt=@ct WHERE OrderId=@oid", conn);
                cmd.Parameters.AddWithValue("@oid", orderId);
                cmd.Parameters.AddWithValue("@s", order.Symbol);
                cmd.Parameters.AddWithValue("@q", order.Quantity);
                cmd.Parameters.AddWithValue("@p", order.Price);
                cmd.Parameters.AddWithValue("@st", order.Status.ToString());
                cmd.Parameters.AddWithValue("@t", order.Type.ToString());
                cmd.Parameters.AddWithValue("@sd", order.Side.ToString());
                cmd.Parameters.AddWithValue("@ct", order.CreatedAt);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes an Order from the database by its OrderId.
        /// </summary>
        /// <param name="orderId">The OrderId of the Order to delete.</param>
        public void DeleteOrder(string orderId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Orders WHERE OrderId=@oid", conn);
                cmd.Parameters.AddWithValue("@oid", orderId);
                cmd.ExecuteNonQuery();
            }
        }

        // Strategy CRUD Operations

        /// <summary>
        /// Retrieves all Strategies from the database.
        /// Note: This method fetches all records; consider adding filters for large datasets.
        /// </summary>
        /// <returns>A list of all Strategy objects.</returns>
        public List<Strategy> GetStrategies()
        {
            var list = new List<Strategy>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT ID, KLineID, StrategyData FROM Strategies", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var strategy = new Strategy(
                            reader.GetInt32("ID"),
                            reader.GetInt32("KLineID"),
                            reader.GetString("StrategyData")
                        );
                        list.Add(strategy);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Inserts a new Strategy into the database.
        /// </summary>
        /// <param name="strategy">The Strategy object to insert (ID will be auto-generated).</param>
        public void InsertStrategy(Strategy strategy)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO Strategies (KLineID, StrategyData) VALUES (@kid, @sd)", conn);
                cmd.Parameters.AddWithValue("@kid", strategy.KLineID);
                cmd.Parameters.AddWithValue("@sd", strategy.StrategyData);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates an existing Strategy in the database.
        /// </summary>
        /// <param name="id">The ID of the Strategy to update.</param>
        /// <param name="strategy">The updated Strategy object.</param>
        public void UpdateStrategy(int id, Strategy strategy)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE Strategies SET KLineID=@kid, StrategyData=@sd WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@kid", strategy.KLineID);
                cmd.Parameters.AddWithValue("@sd", strategy.StrategyData);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a Strategy from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the Strategy to delete.</param>
        public void DeleteStrategy(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Strategies WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}