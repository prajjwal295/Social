
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.Application.DTO;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.Services;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.UserProfileAggegate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Social.Application.Identity.Handlers
{
    internal class LoginIdentityHandler : IRequestHandler<LoginIdentity, OperationResult<AuthenticationResultDto>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public LoginIdentityHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<OperationResult<AuthenticationResultDto>> Handle(LoginIdentity request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AuthenticationResultDto>();

            try
            {
                var authenticationModel = new AuthenticationResultDto();
                var existingUser = await _userManager.FindByEmailAsync(request.UserName);

                if (existingUser == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = Enums.ErrorCode.IdentityNotFound,
                        Message = "Wrong Username, Login Failed"
                    });
                    return result;
                }

                var checkPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);

                if (!checkPassword)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = Enums.ErrorCode.IdentityPasswordIncorrect,
                        Message = "Wrong Password, Login Failed"
                    });
                    return result;
                }

                var profile = await _context.UserProfiles
                    .Include(p => p.RefreshToken)
                    .FirstOrDefaultAsync(p => p.IdentityId == existingUser.Id, cancellationToken);

                authenticationModel.Email = profile.BasicInfo.EmailAddress;
                authenticationModel.Token = GetToken(existingUser, profile);
                authenticationModel.IsAuthenticated = true;
                await SetRefreshToken(profile, authenticationModel);

                result.Payload = authenticationModel;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = Enums.ErrorCode.UnknownError,
                    Message = ex.Message
                });
            }

            return result;
        }

        private string GetToken(IdentityUser user, UserProfile profile)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("IdentityId", user.Id),
                new Claim("UserProfileId", profile?.UserProfileId.ToString())
            });

            var token = _jwtService.CreateToken(identity);
            return _jwtService.WriteToken(token);
        }

        private async Task SetRefreshToken(UserProfile profile, AuthenticationResultDto authenticationModel)
        {
            if (profile.RefreshToken.Any(a => a.IsActive))
            {
                var activeRefreshToken = profile.RefreshToken.FirstOrDefault(a => a.IsActive);
                authenticationModel.RefreshToken = activeRefreshToken.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = _jwtService.CreateRefreshToken();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                profile.RefreshToken.Add(refreshToken);
                _context.Update(profile);
                await _context.SaveChangesAsync();
            }
        }
    }
}
