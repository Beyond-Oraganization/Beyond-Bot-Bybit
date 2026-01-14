
namespace BeyondBot.Model.Indicator
{
    class ATR : MovingAverage
    {
        public string Description => "Average True Range";
        public ATR() : base()
        {
        }
        public ATR(int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(kLineId, name, depth, value, dateTime)
        {
        }

        public ATR(int id, int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(id, kLineId, name, depth, value, dateTime)
        {
        }

        public override List<MovingAverage> Calculate(List<KLine> klines, int depth)
        {
            //List that will be returned
            List<MovingAverage> atrs = new List<MovingAverage>();

            //True range is differance between high and low of kline
            List<decimal> trueRanges = new List<decimal>();
            for (int i = 0; i < klines.Count; i++)
                trueRanges.Add(klines[i].HighPrice - klines[i].LowPrice);

            //We don't use atr, because in TV we use rma for calculatin true ranges
            //avarages
            RMA rma = new RMA();
            var rmaTrueRanges = rma.Calculate(klines, depth).ConvertAll(tr => tr.Value);

            //ATR is basicaly RMA but indexed for every kline
            MovingAverage atr;
            for (int i = 0; i < klines.Count; i++)
            {
                atr = new ATR();
                atr.Value = rmaTrueRanges[i];

                atr.KLineID = klines[i].ID;
                atr.DateTime = klines[i].OpenTime;
                atr.Depth = (int)depth;

                atrs.Add(atr);
            }

            return atrs;
        }

        public override MovingAverage Parse(string data)
        {
            var parts = data.Split(';');
            return new ATR(
                int.Parse(parts[0]),
                parts[1],
                int.Parse(parts[2]),
                decimal.Parse(parts[3]),
                DateTime.Parse(parts[4])
            );
        }
    }
}