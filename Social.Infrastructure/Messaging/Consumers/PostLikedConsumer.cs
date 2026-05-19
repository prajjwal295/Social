using MassTransit;
using Social.DAL.DbContext;
using Social.Infrastructure.Messaging.Events;

namespace Social.Infrastructure.Messaging.Consumers
{
    public class PostLikedConsumer : IConsumer<PostLikedEvent>
    {
        private readonly DataContext _context;
        public PostLikedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PostLikedEvent> context)
        {
            var message = context.Message;
        }
    }
}
