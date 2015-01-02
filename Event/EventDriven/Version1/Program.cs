namespace EventDriven.Version1
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        // 時刻の表示形式
        const string FULL = "yyyy/dd/MM hh:mm:ss\n";
        const string DATE = "yyyy/dd/MM\n";
        const string TIME = "hh:mm:ss\n";

        static bool isSuspended = true;  // プログラムの一時停止フラグ。
        static string timeFormat = TIME; // 時刻の表示形式。

        static void Main()
        {
            WriteHelp();

            var cts = new CancellationTokenSource();

            Task.WhenAll(
                Task.Run(() => EventLoop(cts)),
                TimerLoop(cts.Token)
                ).Wait();
        }

        // 毎秒時刻表示のループ
        private static async Task TimerLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (!isSuspended)
                {
                    // 1秒おきに現在時刻を表示。
                    Console.Write(DateTime.Now.ToString(timeFormat));
                }
                await Task.Delay(1000);
            }
        }

        // キー受付のループ
        static void EventLoop(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                // 文字を読み込む
                // (「キーが押される」というイベントの発生を待つ)
                string line = Console.ReadLine();
                char eventCode = line.Length == 0 ? '\0' : line[0];

                // イベント処理
                switch (eventCode)
                {
                    case 'r': // run
                        isSuspended = false;
                        break;
                    case 's': // suspend
                        isSuspended = true;
                        break;
                    case 'f': // full
                        timeFormat = FULL;
                        break;
                    case 'd': // date
                        timeFormat = DATE;
                        break;
                    case 't': // time
                        timeFormat = TIME;
                        break;
                    case 'q': // quit
                        cts.Cancel();
                        break;
                    default: // ヘルプ
                        WriteHelp();
                        break;
                }
            }
        }

        private static void WriteHelp()
        {
            Console.Write(
              "使い方\n" +
              "r (run)    : 時刻表示を開始します。\n" +
              "s (suspend): 時刻表示を一時停止します。\n" +
              "f (full)   : 時刻の表示形式を“日付＋時刻”にします。\n" +
              "d (date)   : 時刻の表示形式を“日付のみ”にします。\n" +
              "t (time)   : 時刻の表示形式を“時刻のみ”にします。\n" +
              "q (quit)   : プログラムを終了します。\n"
              );
        }
    }
}
