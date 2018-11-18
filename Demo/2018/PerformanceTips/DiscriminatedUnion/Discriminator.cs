namespace DiscriminatedUnion
{
    /// <summary>
    /// 今回、例としてとりあえず、string or char[] な union を作ることにする。
    /// </summary>
    public enum Discriminator
    {
        /// <summary>
        /// 構造体で作ろうと思うんで、default(StringOrCharArray) を null 扱いするために、null にあたる値も用意。
        /// </summary>
        Null,

        /// <summary>
        /// string
        /// </summary>
        String,

        /// <summary>
        /// char[]
        /// </summary>
        CharArray,
    }
}
