namespace BeyondBot.Model.Indicator
{
    class RMA : MovingAverage
    {
        public RMA() : base()
        {
        }
        public RMA(int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(kLineId, name, depth, value, dateTime)
        {
        }
        public RMA(int id, int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(id, kLineId, name, depth, value, dateTime)
        {
        }

        public override List<MovingAverage> Calculate(List<KLine> klines, int depth)
        {
            List<MovingAverage> rmas = new List<MovingAverage>();
            decimal weighting = 1 / depth;

            //First ema calculating
            RMA rma = new RMA(klines[0].ID, "RMA", depth, klines[0].HighPrice - klines[0].LowPrice, klines[0].OpenTime);

            rmas.Add(rma);

            foreach (var kline in klines)
            {
                rmas.Add(new RMA()
                {
                    ID = kline.ID,
                    Name = "RMA",
                    Depth = depth,
                    DateTime = kline.OpenTime,
                    Value = (kline.HighPrice - kline.LowPrice) * weighting + rmas[rmas.Count - 1].Value * (1 - weighting)
                });
            }

            return rmas;
        }

        public override MovingAverage Parse(string data)
        {
            var parts = data.Split(';');
            return new RMA(
                int.Parse(parts[0]),
                parts[1],
                int.Parse(parts[2]),
                decimal.Parse(parts[3]),
                DateTime.Parse(parts[4])
            );
        }
    }
}