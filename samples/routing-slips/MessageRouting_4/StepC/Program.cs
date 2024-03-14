using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageRouting.RoutingSlips;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.RoutingSlips.StepC";
        var endpointConfiguration = new EndpointConfiguration("Samples.RoutingSlips.StepC");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.EnableFeature<RoutingSlips>();

        var endpoint = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpoint.Stop();
    }
}
