using BO;

namespace Exercice_5_MVC.ViewModels
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public int ArticleId { get; set; }
        public ArticleViewModel Article { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderDetail ToModel()
        {
            return new OrderDetail
            {
                Id = Id,
                OrderId = OrderId,
                Order = Order.ToModel(),
                ArticleId = ArticleId,
                Article = Article.ToModel(),
                Quantity = Quantity,
                UnitPrice = UnitPrice
            };
        }
    }
}
