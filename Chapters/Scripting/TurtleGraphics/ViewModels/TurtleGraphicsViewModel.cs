using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace TurtleGraphics.ViewModels
{
    /// <summary>
    /// 亀の動き制御本体。
    /// </summary>
    public class TurtleGraphicsViewModel
    {
        /// <summary>
        /// 描画の間隔。
        /// </summary>
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// 亀の現在位置。
        /// </summary>
        public Cursor Cursor { get; } = new Cursor { X = 100, Y = 100 };

        /// <summary>
        /// 亀が歩いた奇跡。
        /// </summary>
        public ObservableCollection<Line> Lines { get; } = new ObservableCollection<Line>();

        /// <summary>
        /// 亀の速さ。
        /// </summary>
        public double Speed { get; set; } = 10;

        private ConcurrentQueue<Command> _queue = new ConcurrentQueue<Command>();

        public Commander Commander => _commander ?? (_commander = new Commander(_queue));
        private Commander _commander;

        /// <summary>
        /// 描画処理を開始する。
        /// 必ずUIスレッドから呼び出す必要あり。
        /// </summary>
        /// <param name="cancel">描画処理を止めるためのトークン。</param>
        /// <returns></returns>
        public async Task Start(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                await Task.Delay(Interval);

                if (_queue.IsEmpty)
                    continue;

                Command c;
                if (!_queue.TryDequeue(out c))
                    continue;

                switch (c.Type)
                {
                    case Command.CommandType.Walk:
                        await WalkAsync(c.Value);
                        break;
                    case Command.CommandType.Turn:
                        Cursor.Direction += c.Value;
                        break;
                    case Command.CommandType.Clear:
                        Lines.Clear();
                        break;
                    case Command.CommandType.Speed:
                        Speed = c.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task WalkAsync(double value)
        {
            var initX = Cursor.X;
            var initY = Cursor.Y;

            var line = new Line { X1 = initX, Y1 = initY, X2 = initX, Y2 = initY };
            Lines.Add(line);

            var dx = Math.Sin(Cursor.Direction / 180 * Math.PI);
            var dy = -Math.Cos(Cursor.Direction / 180 * Math.PI);

            var destX = initX + value * dx;
            var destY = initY + value * dy;

            var d = Speed * Interval.TotalSeconds;
            dx *= d;
            dy *= d;

            for (var rest = value; rest > 0; rest -= d)
            {
                await Task.Delay(Interval);
                Cursor.X += dx;
                Cursor.Y += dy;
                line.X2 = Cursor.X;
                line.Y2 = Cursor.Y;
            }

            Cursor.X = destX;
            Cursor.Y = destY;
            line.X2 = Cursor.X;
            line.Y2 = Cursor.Y;
        }
    }
}
