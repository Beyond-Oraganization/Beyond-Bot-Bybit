namespace BeyondBot.Model.Indicator
{
    class RMA : MovingAvarage
    {
        public RMA() : base()
        {
        }
        public RMA(int id, string name, int depth, decimal value, DateTime dateTime) : base(id, name, depth, value, dateTime)
        {
        }

        public override List<MovingAvarage> Calculate(List<KLine> klines, int depth)
        {
            List<MovingAvarage> rmas = new List<MovingAvarage>();
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
                    Value = kline.HighPrice - kline.LowPrice
                });
            }

            return rmas;
        }
    }
}