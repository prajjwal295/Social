using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Social.Api.Contracts.Conversation.Request;
using Social.Api.Controllers;
using Social.Api.Extenstions;
using Social.Api.Filters;
using Social.Application.Conversation.Commands;
using Social.Application.Conversation.Query;
using Social.Domain.Aggregates.ConversationAggreagate;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub> _hubContext;

    public ConversationsController(
        IMediator mediator,
        IMapper mapper,
        IHubContext<ChatHub> hubContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDto dto)
    {
        var userId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreateConversationCommand
        {
            CreatedBy = userId,
            Name = dto.Name,
            PhotoUrl = dto.PhotoUrl,
            Participants = dto.ParticipantsIds.Select(Guid.Parse).ToList()
        };

        var result = await _mediator.Send(command);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        await _hubContext.Clients.Group(result.Payload.ConversationId.ToString())
            .SendAsync("UserJoinedConversation", new
            {
                ConversationId = result.Payload.ConversationId.ToString(),
                UserId = userId
            });

        return Ok(result.Payload);
    }

    [HttpPost("{conversationId}/messages")]
    [ValidateGuid("conversationId")]
    [Authorize]
    public async Task<IActionResult> SendMessage(
        [FromRoute] string conversationId,
        [FromBody] SendMessageDto dto)
    {
        var senderId = HttpContext.GetUserProfileIdClaimValue();

        var command = new SendMessageCommand
        {
            ConversationId = Guid.Parse(conversationId),
            SenderId = senderId,
            Message = dto.TextContent
        };

        var result = await _mediator.Send(command);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        await _hubContext.Clients.Group(conversationId)
            .SendAsync("ReceiveMessage", new
            {
                ConversationId = conversationId,
                SenderId = senderId,
                Message = dto.TextContent,
                SentAt = DateTime.UtcNow
            });

        return Ok(result.Payload);
    }

    [HttpGet("{conversationId}/messages")]
    [ValidateGuid("conversationId")]
    [Authorize]
    public async Task<IActionResult> GetMessages(
        [FromRoute] string conversationId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = HttpContext.GetUserProfileIdClaimValue();

        var query = new GetMessageQuery
        {
            UserProfileId = userId,
            ConversationId = Guid.Parse(conversationId),
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        return Ok(result.Payload);
    }

    [HttpDelete("{conversationId}")]
    [ValidateGuid("conversationId")]
    [Authorize]
    public async Task<IActionResult> LeaveConversation([FromRoute] string conversationId)
    {
        var userId = HttpContext.GetUserProfileIdClaimValue();

        var command = new LeaveConversationCommand
        {
            UserProfileId = userId,
            ConversationId = Guid.Parse(conversationId)
        };

        var result = await _mediator.Send(command);

        if (result.IsError)
            return HandleErrorResponse(result.Errors);

        await _hubContext.Clients.Group(conversationId)
            .SendAsync("UserLeftConversation", new
            {
                ConversationId = conversationId,
                UserId = userId
            });

        return Ok(result.Payload);
    }
}
