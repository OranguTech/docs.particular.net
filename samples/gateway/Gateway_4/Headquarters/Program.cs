using NServiceBus;
using NServiceBus.Gateway;
using Shared;

Console.Title = "Headquarters";
var endpointConfiguration = new EndpointConfiguration("Samples.Gateway.Headquarters");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<XmlSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region HeadquartersGatewayConfig
var gatewayConfig = endpointConfiguration.Gateway(new NonDurableDeduplicationConfiguration());
gatewayConfig.AddReceiveChannel("http://localhost:25899/Headquarters/");
gatewayConfig.AddSite("RemoteSite", "http://localhost:25899/RemoteSite/");
#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press 'Enter' to send a message to RemoteSite which will reply.");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    string[] siteKeys = { "RemoteSite" };

    var priceUpdated = new PriceUpdated
    {
        ProductId = 2,
        NewPrice = 100.0,
        ValidFrom = DateTime.Today,
    };
    await endpointInstance.SendToSites(siteKeys, priceUpdated);

    Console.WriteLine("Message sent, check the output in RemoteSite");
}
await endpointInstance.Stop();