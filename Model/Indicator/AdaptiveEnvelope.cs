using System;

namespace BeyondBot.Model.Indicator
{
    class AdaptiveEnvelope
    {
        public int ID { get; set; }
        public int Depth { get; set; }
        public decimal Center { get; set; }
        public DateTime DateTime { get; set; }
        public int NumberOfLines { get; set; }
        public List<decimal>? Low { get; set; }
        public List<decimal>? High { get; set; }

        public AdaptiveEnvelope()
        {}
        public AdaptiveEnvelope(int id, DateTime dateTime, decimal center, int depth, int numberOfLines, List<decimal> low, List<decimal> high)
        {
            ID = id;
            DateTime = dateTime;
            Center = center;
            Depth = depth;
            NumberOfLines = numberOfLines;
            Low = low;
            High = high;
        }

        public List<AdaptiveEnvelope> Calculate(List<KLine> klines, List<MovingAverage> movingAverages, int deviation = 1, int movingCount = 2)
        {
            List<AdaptiveEnvelope> envelopes = new List<AdaptiveEnvelope>();
            //It's nesecary because atrs are deviation variables
            //(Optimization) Можемо замінити цей метод на зроблений GetRMA()
            //бо нічого не зміниться
            List<MovingAverage> atrs = new ATR().Calculate(klines, movingAverages[0].Depth);

            AdaptiveEnvelope envelope;
            for (int a = 0; a < klines.Count; a++)
            {
                envelope = new AdaptiveEnvelope();
                envelope.Center = movingAverages[a].Value;

                envelope.ID = klines[a].ID;
                envelope.NumberOfLines = movingCount;
                envelope.Depth = movingAverages[a].Depth;
                envelope.DateTime = klines[a].OpenTime;

                envelope.Low = new List<decimal>();
                envelope.High = new List<decimal>();
                for (int i = 1; i <= movingCount; i++)
                {
                    envelope.Low.Add(envelope.Center - atrs[a].Value * deviation * i);
                    envelope.High.Add(envelope.Center + atrs[a].Value * deviation * i);
                }

                envelopes.Add(envelope);
            }

            return envelopes;
        }

        public override string ToString()
        {
            if(Low == null || High == null)
                return $"{ID};{Depth};{Center};{DateTime};{NumberOfLines};;";
            else
                return $"{ID};{Depth};{Center};{DateTime};{NumberOfLines};{string.Join(",", Low)};{string.Join(",", High)}";
        }

        public AdaptiveEnvelope Parse(string data)
        {
            var parts = data.Split(';');
            var lowParts = parts[5].Split(',');
            var highParts = parts[6].Split(',');

            List<decimal> lows = new List<decimal>();
            List<decimal> highs = new List<decimal>();

            foreach (var low in lowParts)
                lows.Add(decimal.Parse(low));
            foreach (var high in highParts)
                highs.Add(decimal.Parse(high));

            return new AdaptiveEnvelope(
                id: int.Parse(parts[0]),
                depth: int.Parse(parts[1]),
                center: decimal.Parse(parts[2]),
                dateTime: DateTime.Parse(parts[3]),
                numberOfLines: int.Parse(parts[4]),
                low: lows,
                high: highs
            );
        }
    }
}