using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineSockets
{
    class MyConnection
    {
        private readonly MyServer _server;
        private readonly IDuplexPipe _pipe;
        private readonly CancellationTokenSource _cts;
        private readonly Task _readLoop;

        public int Id { get; private set; }

        public MyConnection(MyServer server, IDuplexPipe pipe)
        {
            _server = server;
            _pipe = pipe;

            _cts = new CancellationTokenSource();
            _readLoop = ReadLoop(_cts.Token);
        }

        private async Task ReadLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var i = _pipe.Input;
                var result = await i.ReadAsync(ct);
                var pos = ReadMessage(result);
                i.AdvanceTo(pos);
            }
        }

        private SequencePosition ReadMessage(ReadResult readResult)
        {
            var reader = new BufferReader<byte>(readResult.Buffer);

            reader.Read(out byte opCode);
            reader.Read(out int len);
            var payload = new byte[len];
            reader.CoptyTo(payload);
            _server.OnReceived(this, opCode, payload);

            return reader.Position;
        }

        public async ValueTask SendAsync(byte opCode, byte[] payload)
        {
            var o = _pipe.Output;
            o.Write(opCode);
            o.Write(payload.Length);
            o.Write(payload);
            await o.FlushAsync(_cts.Token);
        }
    }
}
