using System.ComponentModel.DataAnnotations;
using BO;
using Exercice_5_MVC.ValidateAttribute;
using static BO.Enums;

namespace Exercice_5_MVC.ViewModels
{
    public class OrderViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Customer name")]
        [StringLength(100, ErrorMessage = "Customer name length can't be more than 100.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email address is incorrect.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Shipping address")]
        [StringLength(200, ErrorMessage = "Shipping address length can't be more than 200.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Order date")]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [Required]
        [Display(Name = "Total amount")]
        [ValidateTotalAmount(ErrorMessage = "Total amount must be greater then 0.")]
        public double? TotalAmount { get; set; } = 0;

        [Required]
        [Display(Name = "Order status")]
        [ValidateOrderStatus(ErrorMessage = "Order status must to be : Passed, InProgress, Shipped or Delivered")]
        public OrderStatus? OrderStatus { get; set; }

        //[Required]
        //public string ArticleSelected { get; set; } = string.Empty;

        [ValidateListArticlesSelected(1, ErrorMessage = "At leat one article is required.")]
        public List<ArticleSelectedViewModel>? ListArticlesSelected { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
        public int WarehouseId { get; internal set; }

        public Order ToModel()
        {
            return new Order
            {
                Id = Id!.Value,
                CustomerName = CustomerName,
                Email = Email,
                ShippingAddress = ShippingAddress,
                OrderDate = OrderDate!.Value,
                TotalAmount = TotalAmount!.Value,
                OrderStatus = OrderStatus.Value.ToString(),
                OrderDetails = OrderDetails.Select(x => x.ToModel()).ToList()
            };
        }
    }
}
