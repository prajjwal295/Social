namespace Social.Api.Contracts.Posts.Response
{
    public class PostCommentResponse
    {
        public string CommentText { get;  set; }
        public Guid UserProfileId { get;  set; }
    }
}
