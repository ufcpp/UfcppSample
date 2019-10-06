using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService1
{
    public class SampleService : Sample.SampleBase
    {
        public override async Task GetValues(IAsyncStreamReader<SampleRequest> requestStream, IServerStreamWriter<SampleResponse> responseStream, ServerCallContext context)
        {
            var rand = new Random();

            await foreach (var req in requestStream.ReadAllAsync())
            {
                var mask = (1 << req.Bits) - 1;
                var len = req.Length;

                var res = new SampleResponse();

                for (int i = 0; i < len; i++)
                {
                    res.Values.Add(rand.Next() & mask);
                }

                await responseStream.WriteAsync(res);
            }
        }
    }
}
