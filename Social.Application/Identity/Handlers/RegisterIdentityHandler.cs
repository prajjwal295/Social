using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Social.Application.Identity.Commands;
using Social.Application.Models;
using Social.Application.Services;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.UserProfileAggegate;
using Social.Domain.Exceptions;
using Social.Infrastructure.Messaging.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Social.Application.Identity.Handlers
{
    internal class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly IPublishEndpoint _publishEndPoint;

        public RegisterIdentityHandler(DataContext context, UserManager<IdentityUser> userManager, JwtService jwtService,IPublishEndpoint publishEndPoint)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
            _publishEndPoint = publishEndPoint;
        }

        public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<string>();

            try
            {
                var existingIdentity = await _userManager.FindByEmailAsync(request.EmailAddress);

                if (existingIdentity != null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.InfrastructureError,
                        Message = "User Name is already in use"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var identity = new IdentityUser
                {
                    Email = request.Username,
                    UserName = request.Username,
                };

                //creating transaction
                var transaction = await _context.Database.BeginTransactionAsync();

                var createIdentiy = await _userManager.CreateAsync(identity, request.Password);

                if (!createIdentiy.Succeeded)
                {
                    await transaction.RollbackAsync();
                    result.IsError = true;

                    foreach (var identityError in createIdentiy.Errors)
                    {
                        var error = new Error
                        {
                            Code = Enums.ErrorCode.IdentityCreationFailed,
                            Message = identityError.Description
                        };
                        result.Errors.Add(error);
                    }
                    return result;
                }

                var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username, request.Phone, request.DateOfBirth, request.CurrentCity);
                var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

                try
                {
                    await _context.UserProfiles.AddAsync(profile);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    //ROLLBACK THE TRANSACTIONS
                    await transaction.RollbackAsync();
                    throw;
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                    new Claim("IdentityId", identity.Id),
                    new Claim("UserProfileId", profile.UserProfileId.ToString())
                };

                var token = _jwtService.CreateToken(new ClaimsIdentity(claims));
                result.Payload = _jwtService.WriteToken(token);
                await _publishEndPoint.Publish(new UserRegisteredEvent
                {
                    Email = request.EmailAddress,
                    Username = request.Username,
                    FirstName = request.FirstName
                });
                return result;
            }
            catch (UserProfileNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(er =>
                {
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.ValidationError,
                        Message = er
                    };
                    result.Errors.Add(error);
                });

            }
            catch (Exception ex)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = Enums.ErrorCode.UnknownError,
                    Message = ex.Message
                };
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
