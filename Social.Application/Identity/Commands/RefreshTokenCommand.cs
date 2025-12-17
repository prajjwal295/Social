using MediatR;
using Social.Application.DTO;
using Social.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Identity.Commands
{
    public class RefreshTokenCommand : IRequest<OperationResult<AuthenticationResultDto>>
    {
        public string Token { get; set; }
    }
}
