using DocKick.Dtos.Categories;
using FluentValidation;

namespace DocKick.Validation
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(q => q.Name)
                .NotEmpty();
        }
    }
}