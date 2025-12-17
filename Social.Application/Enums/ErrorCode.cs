using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Enums
{
    public enum ErrorCode
    {
        NotFound = 404,
        ServerError = 500,
        ValidationError = 101,
        UnknownError = 999,
        Unauthorized = 401,

        InfrastructureError = 201,
        IdentityCreationFailed = 202,
        IdentityNotFound = 203,
        IdentityPasswordIncorrect = 204
    }
}
