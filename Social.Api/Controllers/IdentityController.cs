using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Identity.Request;
using Social.Api.Contracts.Identity.Response;
using Social.Api.Filters;
using Social.Application.Identity.Commands;

namespace Social.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpPost]
        [ValidateModel]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUser)
        {
            var command = _mapper.Map<RegisterIdentity>(registerUser);
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResponse(result.Errors);
            }

            var authenticationResult = new AuthenticationResult
            {
                Token = result.Payload
            };

            return Ok(authenticationResult);
        }

        [HttpPost]
        [ValidateModel]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var command = _mapper.Map<LoginIdentity>(request);

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResponse(result.Errors);
            }

            SetRefreshTokenInCookie(result.Payload.RefreshToken);
            return Ok(result.Payload);
        }

        [HttpGet]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var command = new RefreshTokenCommand
            {
                Token = refreshToken
            };

            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResponse(result.Errors);
            }

            if(result.Payload.IsAuthenticated)
            {
                SetRefreshTokenInCookie(result.Payload.RefreshToken);
            }

            return Ok(result.Payload);
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

    }
}
