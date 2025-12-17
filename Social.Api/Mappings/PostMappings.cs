using AutoMapper;
using Social.Api.Contracts.Posts.Request;
using Social.Api.Contracts.Posts.Response;
using Social.Application.Posts.Commands;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.Mappings
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponse>();
            CreateMap<CreateUpdatePost, CreatePostCommand>();
            CreateMap<CreateUpdatePost, UpdatePostCommand>();
            CreateMap<CreatePostComment , CreatePostCommand>();
            CreateMap<PostComment, PostCommentResponse>();
            CreateMap<PostInteraction ,  PostInteractionResponse>();
        }
    }
}
