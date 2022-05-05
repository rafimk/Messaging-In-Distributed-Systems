using SuperStore.Funds.Messages;
using SuperStore.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMessaging();
var app = builder.Build();

app.MapGet("/", () => "FundService Is UP!!");
app.MapGet("/message/send/EU/{country}", async (IMessagePublisher messagePublisher, string country) =>
{
    var message = new FundsMessage(CustomerId: 123, CurrentFunds: 10.00m);
    await messagePublisher.PublishAsync<FundsMessage>("Funds", routingKey: $"EU.{country}", message);
});

app.Run();