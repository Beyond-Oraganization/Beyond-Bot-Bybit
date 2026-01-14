namespace BeyondBot.Model.Indicator
{
    abstract class MovingAverage
    {
        public int ID { get; set; }
        public int KLineID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Depth { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public MovingAverage()
        {}
        public MovingAverage(int kLineId, string name,  int depth, decimal value, DateTime dateTime)
        {
            KLineID = kLineId;
            Name = name;
            DateTime = dateTime;
            Value = value;
            Depth = depth;
        }

        public MovingAverage(int id, int kLineId, string name,  int depth, decimal value, DateTime dateTime)
        {
            ID = id;
            KLineID = kLineId;
            Name = name;
            DateTime = dateTime;
            Value = value;
            Depth = depth;
        }

        public abstract List<MovingAverage> Calculate(List<KLine> klines, int depth);

        public override string ToString()
        {
            return $"{KLineID};{Name};{Depth};{Value};{DateTime}";
        }

        public IndicatorCache ConvertToCache(int klineID)
        {
            return new IndicatorCache(klineID, ToString());
        }

        public abstract MovingAverage Parse(string data);
    }
}
