using BeyondBot.Model;

namespace BeyondBot.Controller.Strategy
{
    abstract class AbstractStrategy
    {
        protected IDBController dbController;
        List<StrategyCache> strategyCaches;

        public AbstractStrategy()
        {
            dbController = Trader.Instance.DBController;
            strategyCaches = new List<StrategyCache>();
        }
        // Checks if current market conditions meet the strategy criteria
        public abstract bool Check();
        // Saves the strategy state to the database
        public void Save(List<StrategyCache> strategyCaches)
        {
            foreach (var strategy in strategyCaches)
            {
                dbController.InsertStrategy(strategy);
            }
        }
        // Extracts the strategy state from the database
        public abstract AbstractStrategy Extract();
        public StrategyCache ConvertToStrategyCache(KLine kLine, AbstractStrategy abstractStrategy)
        {
            // Convert the abstract strategy state to a string representation
            string strategyData = ToString(); // Implement serialization logic here

            return new StrategyCache(kLine.ID, strategyData);
        }
        public abstract override string ToString();
    }
}