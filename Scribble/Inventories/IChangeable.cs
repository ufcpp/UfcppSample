namespace Inventories
{
    /// <summary>
    /// An interface for changing items in a repository.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public interface IChangeable<T>
    {
        /// <summary>
        /// Changes an item.
        /// </summary>
        /// <param name="args">変更状況。</param>
        void Change(ItemChangedArgs<T> args);
    }
}
