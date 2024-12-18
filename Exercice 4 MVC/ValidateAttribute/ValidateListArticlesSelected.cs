using static BO.Enums;
using System.ComponentModel.DataAnnotations;
using Exercice_5_MVC.ViewModels;

namespace Exercice_5_MVC.ValidateAttribute
{
    public class ValidateListArticlesSelected : ValidationAttribute
    {
        private readonly int minCount;

        public ValidateListArticlesSelected(int minCount)
        {
            this.minCount = minCount;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var listArticleSelected = value as List<ArticleSelectedViewModel>;

            if (listArticleSelected == null || listArticleSelected.Count < minCount)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
