using System.Threading.Tasks;

namespace Social.Application.Common.Interfaces
{
	public interface IEventBus
	{
		Task PublishAsync<T>(T @event);
	}
}