
namespace BeyondBot.Model.Indicator
{
    class ATR : MovingAvarage
    {
        public ATR() : base()
        {
        }
        public ATR(int id, string name, int depth, decimal value, DateTime dateTime) : base(id, name, depth, value, dateTime)
        {
        }

        public override List<MovingAvarage> Calculate(List<KLine> klines, int depth)
        {
            //List that will be returned
            List<MovingAvarage> atrs = new List<MovingAvarage>();

            //True range is differance between high and low of kline
            List<decimal> trueRanges = new List<decimal>();
            for (int i = 0; i < klines.Count; i++)
                trueRanges.Add(klines[i].HighPrice - klines[i].LowPrice);

            //We don't use atr, because in TV we use rma for calculatin true ranges
            //avarages
            RMA rma = new RMA();
            var rmaTrueRanges = rma.Calculate(klines, depth).ConvertAll(tr => tr.Value);

            //ATR is basicaly RMA but indexed for every kline
            MovingAvarage atr;
            for (int i = 0; i < klines.Count; i++)
            {
                atr = new ATR();
                atr.Value = rmaTrueRanges[i];

                atr.ID = klines[i].ID;
                atr.DateTime = klines[i].OpenTime;
                atr.Depth = (int)depth;

                atrs.Add(atr);
            }

            return atrs;
        }
    }
}