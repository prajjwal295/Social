namespace Social.Application.Common.Interface
{ 
    public interface IEventBus 
    { 
        Task PublishAsync<T>(T @event); 
    } 
}