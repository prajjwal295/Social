using FluentValidation;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;

namespace Social.Domain.Validators.UserProfileValidators
{
    public class BasicInfoValidator : AbstractValidator<BasicInfo>
    {
        public BasicInfoValidator()
        {
            RuleFor(i => i.FirstName)
                .NotNull()
                    .WithMessage("First name is required.")
                .MinimumLength(3)
                    .WithMessage("First name must be at least 3 characters long.")
                .MaximumLength(50)
                    .WithMessage("First name must not exceed 50 characters.");

            RuleFor(i => i.LastName)
                .NotNull()
                    .WithMessage("Last name is required.")
                .MinimumLength(3)
                    .WithMessage("Last name must be at least 3 characters long.")
                .MaximumLength(50)
                    .WithMessage("Last name must not exceed 50 characters.");

            RuleFor(i => i.EmailAddress)
                .NotNull()
                    .WithMessage("Email address is required.")
                .EmailAddress()
                    .WithMessage("Invalid email address format.");

            RuleFor(i => i.DateOfBirth)
                .NotNull()
                    .WithMessage("Date of birth is required.")
                .InclusiveBetween(DateTime.Now.AddYears(-125), DateTime.Now.AddYears(-18))
                    .WithMessage("Age must be between 18 and 125 years.");
        }
    }
}
