using System;
using System.ComponentModel.DataAnnotations;
using static BO.Enums;

namespace BO
{
    public class ValidateOrderStatus : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var isParsed = Enum.TryParse(typeof(OrderStatus), value.ToString(), true, out object? result);

            if (!isParsed || (isParsed && !Enum.IsDefined(typeof(OrderStatus), result!)))
            {
                return new ValidationResult("Order status must to be : Passed, InProgress, Shipped or Delivered");
            }

            return ValidationResult.Success;
        }
    }
}
