using BeyondBot.Model;

namespace BeyondBot.Controller.Strategy
{
    abstract class AbstractStrategy
    {
        protected IDBController dbController;
        List<IndicatorCache> strategyCaches;

        public AbstractStrategy()
        {
            dbController = Trader.Instance.DBController;
            strategyCaches = new List<IndicatorCache>();
        }
        // Checks if current market conditions meet the strategy criteria
        public abstract bool Check();
        
    }
}