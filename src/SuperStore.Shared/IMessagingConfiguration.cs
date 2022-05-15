using Microsoft.Extensions.DependencyInjection;

namespace SuperStore.Shared;

public interface IMessagingConfiguration
{
    IServiceCollection Services { get; }
}