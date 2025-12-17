using FluentValidation;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Validators.PostValidators
{
    public class PostCommentValidator : AbstractValidator<PostComment>
    {
        public PostCommentValidator()
        {
            RuleFor(pc => pc.CommentText)
                .NotNull()
                    .WithMessage("Comment text is required.")
                .NotEmpty()
                    .WithMessage("Comment text cannot be empty.")
                .MinimumLength(1)
                    .WithMessage("Comment text must contain at least 1 character.")
                .MaximumLength(300)
                    .WithMessage("Comment text must not exceed 300 characters.");
        }
    }
}
