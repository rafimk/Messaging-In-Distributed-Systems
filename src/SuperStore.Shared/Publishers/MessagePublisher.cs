using System.Runtime.Loader;
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
    
    public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message, string messageId = default)
        where TMessage : class, IMessage
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        var properties = _channel.CreateBasicProperties();
        properties.MessageId = messageId ?? Guid.NewGuid().ToString("N");
        
        // _channel.ConfirmSelect();
        // _channel.BasicAcks += (sender, args) =>
        // {
        //     Console.WriteLine("ACK PROVIDED");
        // };
        //
        // _channel.BasicReturn += (sender, args) =>
        // {
        //     Console.WriteLine("Return provided/no que available to deliver");
        // };
        
        _channel.ExchangeDeclare(exchange, "topic", durable:false, autoDelete: false);
        _channel.BasicPublish(exchange, routingKey, mandatory: true, properties, body);

        return Task.CompletedTask;
    }

}