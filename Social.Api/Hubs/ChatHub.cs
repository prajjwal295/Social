using Microsoft.AspNetCore.SignalR;
using Social.Application.Conversation.Commands;

public class ChatHub : Hub
{
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Group(conversationId)
            .SendAsync("UserJoinedConversation", Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        await Clients.Group(conversationId)
            .SendAsync("UserLeftConversation", Context.ConnectionId, conversationId);
    }

    public async Task SendMessageToConversation(string conversationId, SendMessageCommand message)
    {
        await Clients.Group(conversationId).SendAsync("ReceiveMessage", message);
    }
}
