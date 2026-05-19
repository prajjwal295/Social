using MassTransit;
using Social.DAL.DbContext;
using Social.Infrastructure.Messaging.Events;


namespace Social.Infrastructure.Messaging.Consumers
{
    public class UserFollowedConsumer : IConsumer<UserFollowedEvent>
    {
        private readonly DataContext _context;
        public UserFollowedConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserFollowedEvent> context)
        {
            var message = context.Message;

           
        }
    }
}
