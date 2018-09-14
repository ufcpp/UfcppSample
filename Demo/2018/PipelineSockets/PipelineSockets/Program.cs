using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PipelineSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            const int NumClients = 1;

            const int Port = 16738;
            var endPoint = new IPEndPoint(IPAddress.Loopback, Port);

            var s = new MyServer();
            s.Listen(endPoint);

            s.Received += S_Received;

            async Task runClient()
            {
                var c = await MyClient.CreateAsync(endPoint);

                c.Received += C_Received;

                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(10);
                    await c.SendAsync((byte)i, new byte[i]);
                }
            }

            Task.WhenAll(Enumerable.Range(0, NumClients).Select(_ => runClient()));

            Console.ReadLine();
        }

        private static void C_Received(MyClient sender, byte opCode, byte[] payload)
        {
            Console.WriteLine($"client {sender.Id} {opCode}, {string.Join("", payload.Select(b => b.ToString("X2")))}");
        }

        private static void S_Received(MyConnection sender, byte opCode, byte[] payload)
        {
            Console.WriteLine($"server {sender.Id} {opCode}, {string.Join("", payload.Select(b => b.ToString("X2")))}");

            // とりあえずおうむ返し
            sender.SendAsync(opCode, payload);
        }
    }
}
