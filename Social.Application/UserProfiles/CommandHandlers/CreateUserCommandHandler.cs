using AutoMapper;
using MediatR;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.UserProfileAggegate;
using Social.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.UserProfiles.CommandHandlers
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public CreateUserCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
        }


        public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            try
            {
                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);

                var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

                result.Payload = userProfile;
                return result;
            }

            catch (UserProfileNotValidException ex)
            {
                Console.WriteLine(ex);
                result.IsError = true;

                ex.ValidationErrors.ForEach(e =>
                {
                    var error = new Error
                    {
                        Code = ErrorCode.ValidationError,
                        Message = e,
                    };

                    result.Errors.Add(error);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.ServerError,
                    Message = ex.Message,
                });
            }

            return result;
        }
    }
}
