namespace BeyondBot.Model.Indicator
{
    class EMA : MovingAvarage
    {
        public EMA() : base()
        {
        }
        public EMA(int id, string name, int depth, decimal value, DateTime dateTime) : base(id, name, depth, value, dateTime)
        {
        }

        public override List<MovingAvarage> Calculate(List<KLine> klines, int depth)
        {
            List<MovingAvarage> emas = new List<MovingAvarage>();
            decimal weighting = 2 / (depth + 1);

            //First ema calculating
            MovingAvarage ema = new EMA();
            if (klines.Count != 0)
            {
                ema.Value = klines[0].ClosePrice * weighting + klines[0].OpenPrice * (1 - weighting);
                ema.DateTime = klines[0].OpenTime;
                ema.ID = klines[0].ID;
                ema.Depth = (int)depth;

                emas.Add(ema);

                //Calculating ema for the rest of the klines
                for (int i = 1; i <= klines.Count - 1; i++)
                {
                    ema = new EMA();
                    ema.Value = klines[i].ClosePrice * weighting + emas[i - 1].Value * (1 - weighting);
                    ema.DateTime = klines[i].OpenTime;
                    ema.ID = klines[i].ID;
                    ema.Depth = (int)depth;
                    emas.Add(ema);
                }
            }

            return emas;
        }
    }
}