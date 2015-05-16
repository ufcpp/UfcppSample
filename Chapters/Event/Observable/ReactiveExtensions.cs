namespace Observable
{
    using System;
    using System.Reactive.Subjects;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// <see cref="Button"/> が、というか、event 構文がこうだったらよかったのに。
    /// という例の Reactive Extensions 版。
    /// </summary>
    class PreferredButtonByRx
    {
        public IObservable<RoutedEventArgs> Click => _click;
        private Subject<RoutedEventArgs> _click = new Subject<RoutedEventArgs>();

        public IObservable<MouseButtonEventArgs> MouseDoubleClick => _mouseDoubleClick;
        private Subject<MouseButtonEventArgs> _mouseDoubleClick = new Subject<MouseButtonEventArgs>();
    }
}
