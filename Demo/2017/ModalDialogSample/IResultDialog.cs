using System;
using System.Threading.Tasks;
using System.Windows;

namespace ModalDialogSample
{
    public interface IResultDialog<T>
    {
        T Result { get; }
    }

    public static class DialogExtensions
    {
        public static Task<TResult> ShowDialogAsync<TWindow, TResult>()
            where TWindow : Window, IResultDialog<TResult>, new()
            => new TWindow().ShowDialogAsync<TWindow, TResult>();

        public static Task<TResult> ShowDialogAsync<TWindow, TResult>(this TWindow window)
            where TWindow : Window, IResultDialog<TResult>
        {
            var tcs = new TaskCompletionSource<TResult>();
            EventHandler closed = null;
            closed = (sender, e) =>
            {
                window.Closed -= closed;
                tcs.TrySetResult(window.Result);
            };

            window.Closed += closed;
            window.ShowDialog();

            return tcs.Task;
        }
    }
}
