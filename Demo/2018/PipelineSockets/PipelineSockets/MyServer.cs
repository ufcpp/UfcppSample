using System.Collections.Concurrent;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;

namespace PipelineSockets
{
    class MyServer : SocketServer
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private int _id;

        public delegate void ReceivedEventHandler(MyConnection sender, byte opCode, byte[] payload);
        public event ReceivedEventHandler Received;
        private readonly ConcurrentDictionary<int, MyConnection> _connections = new ConcurrentDictionary<int, MyConnection>();

        internal void OnReceived(MyConnection sender, byte opCode, byte[] payload) => Received?.Invoke(sender, opCode, payload);

        protected override Task OnClientConnectedAsync(in ClientConnection client)
        {
            var id = Interlocked.Increment(ref _id);
            var transport = client.Transport;

            return RunAsync(id, transport);
        }

        private async Task RunAsync(int id, IDuplexPipe transport)
        {
            var o = transport.Output;
            o.Write(id);
            await o.FlushAsync();
            
            var c = new MyConnection(this, transport);
            _connections[id] = c;

            // Server からの kick とか Client からの切断機能持たせる？
            await _cts.Token;
        }
    }
}
