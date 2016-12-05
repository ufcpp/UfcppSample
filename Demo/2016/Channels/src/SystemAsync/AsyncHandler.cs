using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// <see cref="System.EventHandler{TEventArgs}"/> の非同期版。
    /// </summary>
    /// <typeparam name="TEventArgs">イベント引数の型。</typeparam>
    /// <param name="sender">イベント送信元。</param>
    /// <param name="args">イベント引数。</param>
    public delegate Task AsyncHandler<TEventArgs>(object sender, TEventArgs args);
}
