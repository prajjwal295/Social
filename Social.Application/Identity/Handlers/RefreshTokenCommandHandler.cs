using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Social.Application.DTO;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.Services;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.UserProfileAggegate;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Identity.Handlers
{
    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, OperationResult<AuthenticationResultDto>>
    {
        public readonly DataContext _context;
        private readonly JwtService _jwtService;
        private readonly UserManager<IdentityUser> _userManager;

        public RefreshTokenCommandHandler(DataContext context, JwtService jwtService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<OperationResult<AuthenticationResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AuthenticationResultDto>();
            try
            {
                var authenticationModel = new AuthenticationResultDto();

                var userProfile = await _context.
                    UserProfiles
                    .Include(u => u.RefreshToken)
                    .FirstOrDefaultAsync
                    (u => u.RefreshToken
                    .Any(t => t.Token == request.Token));

                if (userProfile == null)
                {
                    authenticationModel.IsAuthenticated = false;
                    authenticationModel.Message = $"Token did not match any users.";
                    result.Payload = authenticationModel;
                    return result;
                }

                var refreshToken = userProfile.RefreshToken.Single(x => x.Token == request.Token);

                if (!refreshToken.IsActive)
                {
                    authenticationModel.IsAuthenticated = false;
                    authenticationModel.Message = $"Token Not Active.";
                    result.Payload = authenticationModel;
                    return result;
                }

                // revoke this one
                refreshToken.Revoked = DateTime.UtcNow;
                var newRefreshToken = _jwtService.CreateRefreshToken();
                userProfile.RefreshToken.Add(newRefreshToken);
                _context.Update(userProfile);
                await _context.SaveChangesAsync();

                //GENERATE NEW JWT
                var user = await _userManager.FindByEmailAsync(userProfile.BasicInfo.EmailAddress);
                var token = GetToken(user, userProfile);

                authenticationModel.Token = token;
                authenticationModel.RefreshToken = newRefreshToken.Token;
                authenticationModel.Message = "Refresh Token Updated";
                authenticationModel.IsAuthenticated = true;
                authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
                authenticationModel.Email = user.Email;
                result.Payload = new AuthenticationResultDto();
                result.Payload = authenticationModel;

                return result;

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
    }
}
