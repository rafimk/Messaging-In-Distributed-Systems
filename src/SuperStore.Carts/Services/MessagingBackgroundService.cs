using System.Reflection.Metadata;
using SuperStore.Carts.Messages;
using SuperStore.Shared.Connections;
using SuperStore.Shared.Subscribers;

namespace SuperStore.Carts.Services;

public class MessagingBackgroundService : BackgroundService
{
    private readonly IChannelFactory _channelFactory;
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ILogger<MessagingBackgroundService> _logger;

    public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger, IChannelFactory channelFactory)
    {
        _messageSubscriber = messageSubscriber;
        _logger = logger;
        _channelFactory = channelFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _channelFactory.Create();
        
        channel.ExchangeDeclare("Funds", "topic", durable:false, autoDelete:false, arguments:null);
        _messageSubscriber
            .SubscribeMessage<FundsMessage>("carts-service-eu-many-words-queue", "EU.#", "Funds",
                (msg, args) =>
                {
                    _logger.LogInformation($"Received EU multiple message for customer : {msg.CustomerId} | Funds {msg.CurrentFunds} | Routing Key : {args.RoutingKey}");
                    return Task.CompletedTask;
                })
            .SubscribeMessage<FundsMessage>("carts-service-eu-single-word-queue", "EU.*", "Funds",
                (msg, args) =>
                {
                    _logger.LogInformation($"Received EU single message for customer : {msg.CustomerId} | Funds {msg.CurrentFunds} | Routing Key : {args.RoutingKey}");
                    return Task.CompletedTask;
                });
    }
}