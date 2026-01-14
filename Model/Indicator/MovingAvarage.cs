namespace BeyondBot.Model.Indicator
{
    abstract class MovingAvarage
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Depth { get; set; }
        public decimal Value { get; set; }
        public DateTime DateTime { get; set; }
        public MovingAvarage()
        {}
        public MovingAvarage(int id, string name,  int depth, decimal value, DateTime dateTime)
        {
            ID = id;
            Name = name;
            DateTime = dateTime;
            Value = value;
            Depth = depth;
        }

        public abstract List<MovingAvarage> Calculate(List<KLine> klines, int depth);

        public override string ToString()
        {
            return $"{ID};{Name};{Depth};{Value};{DateTime}";
        }

        public IndicatorCache ConvertToCache(int klineID)
        {
            return new IndicatorCache(klineID, ToString());
        }

        public abstract MovingAvarage Parse(string data);
    }
}
