using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.UserProfile.Request;
using Social.Api.Contracts.UserProfile.Response;
using Social.Api.Extenstions;
using Social.Api.Filters;
using Social.Application.UserProfiles.Commands;
using Social.Application.UserProfiles.Queries;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserProfileController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            var query = new GetAllUserProfiles();
            var response = await _mediator.Send(query);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
                return Ok(profiles);
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate profile)
        {
            var command = _mapper.Map<CreateUserCommand>(profile);
            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);
                return CreatedAtAction(nameof(GetUserProfileById), new { id = response.Payload.UserProfileId.ToString() }, userProfile);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetUserProfileById([FromRoute] string id)
        {
            var query = new GetUserProfileById
            {
                UserProfileId = Guid.Parse(id)
            };

            var response = await _mediator.Send(query);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }
            else
            {
                var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);
                return Ok(userProfile);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> UpdateUserDetails([FromRoute] string id, UserProfileCreateUpdate profile)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(profile);
            command.UserProfileId = Guid.Parse(id);
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

        [HttpDelete]
        [Route("{id}")]
        [ValidateModel]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeleteUserProfile([FromRoute] string id)
        {
            var command = new DeleteUserCommand();
            command.UserProfileId = Guid.Parse(id);
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

        [HttpPost]
        [Route("{id}/follow")]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> FollowUser([FromRoute] string id)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new FollowUserCommand
            {
                FollowerId = userProfileId,
                FolloweeId = Guid.Parse(id)
            };

            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            return Ok(response.Payload);
        }

        [HttpPost]
        [Route("{id}/unfollow")]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> UnfollowUser([FromRoute] string id)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var command = new UnfollowUserCommand
            {
                FollowerId = userProfileId,
                FolloweeId = Guid.Parse(id)
            };

            var response = await _mediator.Send(command);

            if (response.IsError)
            {
                return HandleErrorResponse(response.Errors);
            }

            return Ok(response.Payload);
        }
    }
}
