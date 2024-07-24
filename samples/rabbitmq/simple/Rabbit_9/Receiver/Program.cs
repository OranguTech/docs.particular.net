﻿using System;

namespace Receiver
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "SimpleReceiver";
            var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleReceiver");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            transport.UseConventionalRoutingTopology(QueueType.Quorum);
            transport.ConnectionString("host=localhost");
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
        }
    }
}
