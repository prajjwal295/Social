using AutoMapper;
using Social.Api.Contracts.Conversation.Response;
using Social.Domain.Aggregates.ConversationAggreagate;

namespace Social.Api.Mappings
{
    public class ConversationMappings : Profile
    { 
        public ConversationMappings()
        {
            CreateMap<Message, MessageResponseDto>();
        }
    }
}
