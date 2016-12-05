using System.ComponentModel;

namespace CardBattle.Lib.ComponentModel
{
    /// <summary>
    /// データバインディング用共通基底。
    /// </summary>
    /// <remarks>
    /// System.Threading.Tasks 版もある。
    /// あっちは、[CallerMemberName] を使ってるのと、boxing 回避用オーバーロードがない。
    /// </remarks>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged { add { _PropertyChanged += value; } remove { _PropertyChanged -= value; } }

        /// <summary>
        /// 派生クラスから直接 Invoke したいことがあるので。
        /// </summary>
        protected PropertyChangedEventHandler _PropertyChanged { get; private set; }

        /// <summary>
        /// フィールドの値を書き替え＆通知。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, string propertyName) => SetProperty(ref storage, value, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// 変更通知。
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// フィールドの値を書き替え＆通知。
        /// </summary>
        protected bool SetProperty<T>(ref T storage, T value, PropertyChangedEventArgs e)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(e);
            return true;

        }

        /// <summary>
        /// 変更通知。
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => _PropertyChanged?.Invoke(this, e);
    }
}
