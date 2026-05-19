using MassTransit;
using Social.DAL.DbContext;
using Social.Infrastructure.Messaging.Events;

namespace Social.Infrastructure.Messaging.Consumers
{
    public class UserCommentedConsumer : IConsumer<UserCommentedEvent>
    {
        private readonly DataContext _context;
        public UserCommentedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserCommentedEvent> context)
        {
            var message = context.Message;
        }
    }
}
