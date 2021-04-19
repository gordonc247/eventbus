using System.Threading.Tasks;

namespace Codefire.EventBus
{
    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent evt);
    }
}
