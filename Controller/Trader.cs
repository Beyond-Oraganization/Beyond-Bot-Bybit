using BeyondBot.Model;

namespace BeyondBot.Controller
{
    class Trader
    {
        public static Trader? instance;
        public static Trader Instance => instance ??= new Trader();
        IDBController dbController = MySQLController.Instance;
        IAPIController apiController = BybitController.Instance;
        public IDBController DBController { get { return dbController; }} 
        public IAPIController APIController { get { return apiController; }} 

        // This class can be expanded to manage trading operations if needed
    }
}