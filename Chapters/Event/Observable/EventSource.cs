using System;
using System.Threading.Tasks;

class EventSource
{
    public event EventHandler<int> Progress; // 登録口

    private async Task RunAsync()
    {
        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(100);
            Progress(this, i); // イベントを起こす
        }
    }
}

class EventSubscriber : IDisposable
{
    EventSource _source;

    public EventSubscriber(EventSource source)
    {
        _source = source;
        source.Progress += OnProgress; // 購読開始
    }

    // イベントを受け取って処理
    private void OnProgress(object sender, int i)
    {
        Console.WriteLine("進捗 " + i + "%");
    }

    public void Dispose()
    {
        _source.Progress -= OnProgress; // 購読解除
    }
}
