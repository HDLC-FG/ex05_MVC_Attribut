using BO;

namespace Exercice_5_MVC.Service
{
    public class OrderService
    {
        private static ApplicationDbContext dbContext = new ApplicationDbContext();

        public List<Order> GetOrders()
        {
            return dbContext.Orders;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            return dbContext.OrderDetails;
        }

        public List<Article> GetArticles()
        {
            return dbContext.ArticleReferences;
        }

        public void AddArticleQuantity(int id, int quantity)
        {
            dbContext.ArticleReferences.Find(x => x.Id == id).StockQuantity += quantity;
        }

        public Order? Get(int id)
        {
            return dbContext.Orders.Find(x => x.Id == id);
        }

        public void Add(Order order)
        {
            dbContext.Orders.Add(order);
        }

        public void Update(Order order)
        {
            var oldOrder = dbContext.Orders.Find(x => x.Id == order.Id);
            oldOrder = order;
        }

        public void Delete(Order order)
        {
            dbContext.Orders.Remove(order);
        }
    }
}
