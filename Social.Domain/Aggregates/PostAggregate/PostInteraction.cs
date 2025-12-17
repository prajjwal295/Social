using FluentValidation.Results;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction() { }
        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public InteractionType Interaction { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }

        // factory method
        public static PostInteraction CreateInteraction(Guid postId , Guid userProfileId , InteractionType interaction)
        {
            var validator = new PostInteractionValidator();
            var objToValidate  =  new PostInteraction
            {
                PostId = postId,
                UserProfileId = userProfileId,
                Interaction = interaction,
                CreatedDate = DateTime.Now,
            };

            ValidationResult validationResult = validator.Validate(objToValidate);

            if (validationResult.IsValid)
            {
                return objToValidate;
            }

            var exception = new PostInteractionNotValidException("The Post Interaction is Not Valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }
    }
}
