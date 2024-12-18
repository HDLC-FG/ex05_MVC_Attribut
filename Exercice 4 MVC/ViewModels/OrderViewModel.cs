﻿using System.ComponentModel.DataAnnotations;
using BO;
using Exercice_5_MVC.ValidateAttribute;

namespace Exercice_5_MVC.ViewModels
{
    public class OrderViewModel
    {
        [Required]
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
        [ValidateTotalAmount]
        public double? TotalAmount { get; set; } = 0;

        [Required]
        [Display(Name = "Order status")]
        [ValidateOrderStatus]
        public string OrderStatus { get; set; } = string.Empty;

        //[Required]
        public string ArticleSelected { get; set; } = string.Empty;
        public string ListArticlesSelected { get; set; } = string.Empty;
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
                OrderStatus = OrderStatus,
                OrderDetails = OrderDetails.Select(x => x.ToModel()).ToList()
            };
        }
    }
}