using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Posts.Request;
using Social.Api.Contracts.Posts.Response;
using Social.Api.Extenstions;
using Social.Api.Filters;
using Social.Application.Posts.Commands;
using Social.Application.Posts.Queries;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PostController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetAllPosts([FromQuery]string? id)
        {
            var query = new GetAllPosts
            {
                UserProfileId = id == null ? null : Guid.Parse(id)
            };
            var response = await _mediator.Send(query);

            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(_mapper.Map<List<PostResponse>>(response.Payload));

        }

        [HttpGet]
        [Route("id")]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetPostById
            {
                PostId = Guid.Parse(id)
            };

            var response = await _mediator.Send(query);

            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(_mapper.Map<PostResponse>(response.Payload));

        }

        [HttpPost]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePost postCreate)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = _mapper.Map<CreatePostCommand>(postCreate);
            command.UserProfileId = userProfileId;

            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                var postResponse = _mapper.Map<PostResponse>(response.Payload);
                return CreatedAtAction(nameof(GetById), new { id = response.Payload.PostId.ToString() }, postResponse);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> UpdatePost([FromRoute] string id, [FromBody] CreateUpdatePost createUpdatePost)
        {
            var command = _mapper.Map<UpdatePostCommand>(createUpdatePost);
            command.PostId = Guid.Parse(id);
            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                var postResponse = _mapper.Map<PostResponse>(response.Payload);
                return Ok(postResponse);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost([FromRoute] string id)
        {
            var command = new DeletePostCommand
            {
                PostId = Guid.Parse(id)
            };

            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                return NoContent();
            }
        }


        [HttpGet]
        [Route("{postId}/comments")]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetCommnetsByPostId([FromRoute] string postId)
        {

            var query = new GetPostComments
            {
                PostId = Guid.Parse(postId)
            };

            var response = await _mediator.Send(query);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                return Ok(_mapper.Map<List<PostCommentResponse>>(response.Payload));
            }
        }

        [HttpPost]
        [Route("{postId}/comments")]
        [ValidateGuid("postId")]
        [Authorize]
        public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] CreatePostComment postComment)
        {
            var command = new CreatePostCommentCommand
            {
                PostId = Guid.Parse(postId),
                CommentText = postComment.CommentText,
                UserProfileId = Guid.Parse(postComment.UserProfileId)
            };

            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                return Ok(_mapper.Map<PostCommentResponse>(response.Payload));
            }
        }

        [HttpPost]
        [Route("{postId}/interactions")]
        [ValidateGuid("postId")]
        [Authorize]
        public async Task<IActionResult> AddInterationToPost([FromRoute] string postId, [FromBody] CreatePostInteration postInteration)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new CreatePostInteractionCommand
            {
                type = postInteration.InteractionType,
                PostId = Guid.Parse(postId),
                UserProfileId = userProfileId
            };

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResponse(result.Errors);
            }

            return Ok(_mapper.Map<PostInteractionResponse>(result.Payload));
        }

        [HttpDelete]
        [Route("interactions/{interactionId}")]
        [ValidateGuid("interactionId")]
        [Authorize]
        public async Task<IActionResult> DeleteInteraction([FromRoute] string interactionId)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new DeletePostInteractionCommand
            {
                InteractionId = Guid.Parse(interactionId),
                UserProfileId = userProfileId
            };

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResponse(result.Errors);
            }

            return NoContent();
        }

        
    }
}
