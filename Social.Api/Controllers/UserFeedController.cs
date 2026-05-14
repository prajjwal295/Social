using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Extenstions;
using Social.Application.UserFeed.Queries;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFeedController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserFeedController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user timeline (feed)
        /// </summary> 
        [HttpGet]
        public async Task<IActionResult> GetUserFeed()
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();
            var query = new GetUserFeedQuery
            {
                UserId =userProfileId
            };

            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandleErrorResponse(response.Errors);

            return Ok(response.Payload);
        }
    }
}
