namespace EventDriven.Version2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    // イベント処理用のデリゲート
    delegate void KeyboadEventHandler(char eventCode);

    /// <summary>
    /// キーボードからの入力イベント待受けクラス。
    /// </summary>
    class KeyboardEventLoop
    {
        KeyboadEventHandler _onKeyDown;

        public KeyboardEventLoop(KeyboadEventHandler onKeyDown)
        {
            _onKeyDown = onKeyDown;
        }

        /// <summary>
        /// 待受け開始。
        /// </summary>
        /// <param name="ct">待ち受けを終了したいときにキャンセルする。</param>
        public Task Start(CancellationToken ct)
        {
            return Task.Run(() => EventLoop(ct));
        }

        /// <summary>
        /// イベント待受けループ。
        /// </summary>
        void EventLoop(CancellationToken ct)
        {
            // イベントループ
            while (!ct.IsCancellationRequested)
            {
                // 文字を読み込む
                // (「キーが押される」というイベントの発生を待つ)
                string line = Console.ReadLine();
                char eventCode = (line == null || line.Length == 0) ? '\0' : line[0];

                // イベント処理はデリゲートを通して他のメソッドに任せる。
                _onKeyDown(eventCode);
            }
        }
    }
}
