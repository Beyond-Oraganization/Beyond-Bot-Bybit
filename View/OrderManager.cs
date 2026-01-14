using System.Threading.Tasks;
using BeyondBot.Controller;

namespace BeyondBot.View
{
    class OrderManager
    {
        private static OrderManager? instance;
        public static OrderManager Instance => instance ??= new OrderManager();

        void DisplayOrders(List<Model.Order> orders)
        {
            Console.WriteLine("Current Orders:");
            foreach (var order in orders)
            {
                Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
            }
        }

        void DisplayOrders(Model.Order order)
        {
            Console.WriteLine("Current Order:");
            Console.WriteLine($"ID: {order.OrderId}, Symbol: {order.Symbol}, Quantity: {order.Quantity}, Price: {order.Price}, Status: {order.Status}, Type: {order.Type}, Side: {order.Side}, Created At: {order.CreatedAt}");
        }

        public async Task ViewOrdersAsync()
        {
            try
            {
                var orders = await Trader.Instance.APIController.GetOrdersAsync();

                if(orders.Count == 0)
                {
                    Console.WriteLine("No orders found.");
                    return;
                }

                DisplayOrders(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error viewing orders: " + ex.Message);
            }
        }

        public async Task PlaceOrderAsync()
        {
            try
            {
                // Implementation for placing an order
                Console.WriteLine("Placing a new order...");
                // Example parameters; in real scenario, gather from user input
                var order = await Trader.Instance.APIController.PlaceOrderAsync("XAUTUSDT", Model.OrderSide.Buy, Model.OrderType.Market, 0.001m);
                DisplayOrders(order);
                ModelManager.Instance.SaveOrder(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error placing order: " + ex.Message);
            }
        }
    }
}