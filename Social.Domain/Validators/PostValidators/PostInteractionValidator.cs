using FluentValidation;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Validators.PostValidators
{
    internal class PostInteractionValidator : AbstractValidator<PostInteraction>
    {
        public PostInteractionValidator()
        {
            RuleFor(x => x.Interaction)
                .NotNull()
                .WithMessage("Interaction Type Cannot be Null");

            RuleFor(x => x.PostId)
                .NotNull()
                .WithMessage("Post Id Cannot be Null");


            RuleFor(x => x.UserProfileId)
                .NotNull()
            .WithMessage("User Profile Id Cannot be Null");

        }
    }
}
