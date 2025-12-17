using Social.Domain.Aggregates.UserProfileAggegate;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.PostAggregate
{
    public class Post
    {

        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();

        [JsonConstructor]
        private Post()
        {
        }

        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public IEnumerable<PostComment> Comments { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }


        // factory method
        public static Post CreatePost(Guid userProfileId, string textContent)
        {
            var validator = new PostValidator();

            var objToValidate = new Post
            {
                UserProfileId = userProfileId,
                TextContent = textContent,
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            };


            var validationResult = validator.Validate(objToValidate);

            if (validationResult.IsValid)
            {
                return objToValidate;
            }

            var exception = new PostNotValidException("Post is Not Valid");

            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw  exception;
        }

        //public methods
        public void UpdatePostText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostNotValidException("Cannot Update Post. Post text is not valid");
                exception.ValidationErrors.Add("The Provied text is either null or contains only white space");
                throw exception;
            }

            TextContent = newText;
            LastModified = DateTime.Now;
        }

        public void AddComment(PostComment newCommment)
        {
            _comments.Add(newCommment);
        }

        public void RemoveComment(PostComment toRemove)
        {
            _comments.Remove(toRemove);
        }

        public void AddInteraction(PostInteraction toAdd)
        {
            _interactions.Add(toAdd);
        }

        public void RemoveInteraction(PostInteraction toRemove)
        {
            _interactions.Remove(toRemove);
        }
    }
}
