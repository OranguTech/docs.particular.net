using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#pragma warning disable CS0618 // Type or member is obsolete
        #region ConfigureDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(@"..\..\..\..\storage");

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete

        #region CustomJsonSerializerOptions
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new DatabusPropertyConverterFactory());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);
        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press 'D' to send a databus large message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D)
            {
                await SendMessageLargePayload(endpointInstance);
                continue;
            }
            break;
        }
        await endpointInstance.Stop();
    }

    static async Task SendMessageLargePayload(IEndpointInstance endpointInstance)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region SendMessageLargePayload

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new DataBusProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message);

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete

        Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
    }
}
