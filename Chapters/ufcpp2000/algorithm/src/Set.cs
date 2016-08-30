using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// セット。
  /// 数学で「集合」と呼ぶ奴。
  /// 要素の順序には意味がなくて、要素が含まれているかどうかだけが問題。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  interface ISet<T> : IEnumerable<T>
  {
    void Insert(T elem);
    void Erase(T elem);
    bool Contains(T elem);
  }
}
