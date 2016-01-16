using System.Collections.Concurrent;

namespace TurtleGraphics.ViewModels
{
    /// <summary>
    /// コマンド発行クラス。
    /// C# スクリプトのglobalsとして渡して、スクリプトからコマンドを発行するのに使う。
    /// </summary>
    public class Commander
    {
        private ConcurrentQueue<Command> _queue;

        public Commander(ConcurrentQueue<Command> queue)
        {
            _queue = queue;
        }

        public void walk(double distance) => _queue.Enqueue(Command.Walk(distance));
        public void turn(double angle) => _queue.Enqueue(Command.Turn(angle));
        public void speed(double speedDotPerSecond) => _queue.Enqueue(Command.Speed(speedDotPerSecond));
        public void clear() => _queue.Enqueue(Command.Clear());
    }
}
