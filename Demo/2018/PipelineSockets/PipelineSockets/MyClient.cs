using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;

namespace PipelineSockets
{
    class MyClient
    {
        private readonly IDuplexPipe _pipe;
        private readonly CancellationTokenSource _cts;
        private readonly Task _readLoop;

        public int Id { get; }

        public MyClient(int id, IDuplexPipe pipe)
        {
            Id = id;
            _pipe = pipe;

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _readLoop = ReadLoop(ct);
        }

        public static async Task<MyClient> CreateAsync(EndPoint endPoint)
        {
            var s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            await s.ConnectAsync(endPoint);
            var p = SocketConnection.Create(s);

            var i = p.Input;
            var r = await i.ReadAsync();

            int read(ReadOnlySequence<byte> buffer)
            {
                var reader = new BufferReader<byte>(buffer);
                if (!reader.Read(out int x)) throw new Exception();
                return x;
            }

            var id = read(r.Buffer);
            return new MyClient(id, p);
        }

        public MyClient(int id)
        {
            Id = id;
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
            Received?.Invoke(this, opCode, payload);
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

        public delegate void ReceivedEventHandler(MyClient sender, byte opCode, byte[] payload);
        public event ReceivedEventHandler Received;
    }
}
