using Microsoft.Extensions.DependencyInjection;

namespace SuperStore.Shared;

internal sealed record MessagingConfiguration(IServiceCollection Services) : IMessagingConfiguration;