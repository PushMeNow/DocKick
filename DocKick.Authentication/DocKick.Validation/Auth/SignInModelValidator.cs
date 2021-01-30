using DocKick.DataTransferModels.Users;
using FluentValidation;

namespace DocKick.Validation.Auth
{
    public class SignInModelValidator : AbstractValidator<SingInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(q => q.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(q => q.Password)
                .NotEmpty();

            RuleFor(q => q.ReturnUrl)
                .NotEmpty();
        }
    }
}