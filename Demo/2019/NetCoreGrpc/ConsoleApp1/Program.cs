using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GrpcService1.Sample;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("press enter to start");
            Console.ReadLine();

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            await Sample(channel);

            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }

        private static async Task Sample(GrpcChannel channel)
        {
            var client = new SampleClient(channel);
            var call = client.GetValues();

            async Task sender()
            {
                for (int i = 1; i < 32; i++)
                {
                    await call.RequestStream.WriteAsync(new SampleRequest { Bits = i, Length = i });
                    await Task.Delay(500);
                }
                await call.RequestStream.CompleteAsync();
            }

            async IAsyncEnumerable<int> receiver()
            {
                await foreach (var res in call.ResponseStream.ReadAllAsync())
                {
                    foreach (var value in res.Values)
                    {
                        yield return value;
                    }
                }
            }

            _ = sender();

            await foreach (var value in receiver())
            {
                Console.WriteLine(value);
            }
        }
    }
}
