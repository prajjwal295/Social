using FluentValidation;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Validators.PostValidators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(pc => pc.TextContent)
                .NotNull()
                    .WithMessage("Post text is required.")
                .NotEmpty()
                    .WithMessage("Post text cannot be empty.")
                .MinimumLength(1)
                    .WithMessage("Post text must contain at least 1 character.")
                .MaximumLength(300)
                    .WithMessage("Post text must not exceed 300 characters.");
        }
    }
}
