namespace DataLib
{
    /// <summary>
    /// データ例
    /// </summary>
    public class Class1
#if NotiryPropertyChanged
        : BindableBase
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
