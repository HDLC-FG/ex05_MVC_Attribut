using BO;

namespace Exercice_5_MVC.ViewModels
{
    public class ArticleViewModel
    {
        public ArticleViewModel()
        {
        }

        public ArticleViewModel(int id, string name, string description, decimal price, int stockQuantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Article ToModel()
        {
            return new Article 
            { 
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                StockQuantity = StockQuantity
            };
        }
    }
}
