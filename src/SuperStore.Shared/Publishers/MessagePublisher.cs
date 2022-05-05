using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SuperStore.Shared.Connections;

namespace SuperStore.Shared.Publishers;

internal sealed class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;

    public MessagePublisher(IChannelFactory channelFactory)
        => _channel = channelFactory.Create();
    
    public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message) where TMessage : class, IMessage
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        _channel.ExchangeDeclare(exchange, "topic", durable:false, autoDelete: false);
        
        var properties = _channel.CreateBasicProperties();
        
        _channel.BasicPublish(exchange, routingKey, mandatory: true, properties, body);

        return Task.CompletedTask;
    }

}