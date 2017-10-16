using System;

namespace Lib
{
#if NotiryPropertyChanged
    public class BindableBase : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
#endif

    public class Class1
#if NotiryPropertyChanged
        : BindableBase, CommonLib.CommonInterface
#else
        : CommonLib.CommonInterface
#endif
    {
        /// <summary>
        /// 全環境で使える想定のプロパティ
        /// </summary>
        public int Id
#if NotiryPropertyChanged
        { get => _id; set => SetProperty(ref _id, value); }
        private int _id;
#else
        { get; set; }
#endif

#if Admin
        /// <summary>
        /// クライアントには見せたくない想定のプロパティ
        /// </summary>
        public string Secret
#if NotiryPropertyChanged
        { get => _secret; set => SetProperty(ref _secret, value); }
        private string _secret;
#else
        { get; set; }
#endif
#endif
    }
}
