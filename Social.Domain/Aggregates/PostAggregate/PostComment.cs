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
    public class PostComment
    {
        private PostComment() { }
        public Guid CommentId { get; private set; }
        public Guid PostId { get; private set; }
        public string CommentText { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ModifiedDate { get; private set; }

        /// <summary>
        /// Create Post Comment
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userProfileId"></param>
        /// <param name="commmentText"></param>
        /// <returns></returns>
        public static PostComment CreateComment(Guid postId, Guid userProfileId, string commmentText)
        {
            var validator = new PostCommentValidator();

            var objToValidate = new PostComment
            {
                PostId = postId,
                UserProfileId = userProfileId,
                CommentText = commmentText,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            ValidationResult validationResult = validator.Validate(objToValidate);

            if (validationResult.IsValid)
            {
                return objToValidate;
            }

            var exception = new PostCommentNotValidException("The Post Comment is Not Valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }

        public void UpdateComment(string commentText)
        {
            CommentText = commentText;
            ModifiedDate = DateTime.Now;
        }
    }
}
