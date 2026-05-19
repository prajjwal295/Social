using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;
        public UpdateUserProfileBasicInfoCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            try
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);

                if (userProfile == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No UserProfile with Id {request.UserProfileId}",
                    });
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone
                    , request.DateOfBirth, request.CurrentCity,request.ProfilePicutreUrl , request.ProfilePicturePublicId);

                userProfile.UpdateBasicInfo(basicInfo);

                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();

                result.Payload = userProfile;
                return result;
            }

            catch(UserProfileNotValidException ex)
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
