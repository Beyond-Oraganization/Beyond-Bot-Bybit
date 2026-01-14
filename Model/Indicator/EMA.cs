namespace BeyondBot.Model.Indicator
{
    class EMA : MovingAverage
    {
        public EMA() : base()
        {
        }
        public EMA(int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(kLineId, name, depth, value, dateTime)
        {
        }

        public EMA(int id, int kLineId, string name, int depth, decimal value, DateTime dateTime) : base(id, kLineId, name, depth, value, dateTime)
        {
        }

        public override List<MovingAverage> Calculate(List<KLine> klines, int depth)
        {
            List<MovingAverage> emas = new List<MovingAverage>();
            decimal weighting = 2 / (depth + 1);

            //First ema calculating
            MovingAverage ema = new EMA();
            if (klines.Count != 0)
            {
                ema.Value = klines[0].ClosePrice * weighting + klines[0].OpenPrice * (1 - weighting);
                ema.DateTime = klines[0].OpenTime;
                ema.KLineID = klines[0].ID;
                ema.Depth = (int)depth;

                emas.Add(ema);

                //Calculating ema for the rest of the klines
                for (int i = 1; i <= klines.Count - 1; i++)
                {
                    ema = new EMA();
                    ema.Value = klines[i].ClosePrice * weighting + emas[i - 1].Value * (1 - weighting);
                    ema.DateTime = klines[i].OpenTime;
                    ema.KLineID = klines[i].ID;
                    ema.Depth = (int)depth;
                    ema.Name = "EMA";
                    emas.Add(ema);
                }
            }

            return emas;
        }

        public override MovingAverage Parse(string data)
        {
            var parts = data.Split(';');
            return new EMA(
                int.Parse(parts[0]),
                parts[1],
                int.Parse(parts[2]),
                decimal.Parse(parts[3]),
                DateTime.Parse(parts[4])
            );
        }
    }
}