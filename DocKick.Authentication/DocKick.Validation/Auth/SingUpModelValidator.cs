using DocKick.DataTransferModels.Users;
using FluentValidation;

namespace DocKick.Validation.Auth
{
    public class SingUpModelValidator : AbstractValidator<SignUpModel>
    {
        public SingUpModelValidator()
        {
            RuleFor(q => q.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(q => q.Password)
                .NotEmpty();

            RuleFor(q => q.ConfirmPassword)
                .NotEmpty()
                .Equal(q => q.Password);

            RuleFor(q => q.ReturnUrl)
                .NotEmpty();
        }
    }
}