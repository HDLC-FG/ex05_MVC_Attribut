using BO;

namespace Exercice_5_MVC.ViewModels
{
    public class ArticleViewModel
    {
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
