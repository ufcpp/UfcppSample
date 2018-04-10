using System;

/*
todo:
- generic constraints
  - [x] unmanaged
  - [x] Enum
  - [x] Delegate
- [x] Support == and != for tuples
- [?] strongname ←署名入りのDLLをRoslynが読めなかったという不具合修正っぽいんだけども。署名したことないからわかんない…
- [x] Attribute on backing field
- [x] Ref Reassignment
- [x] Stackalloc initializers
- [ ] Custom fixed
- [ ] Indexing movable fixed buffers
- Improved overload candidates
  - [x] generic constraints
  - [x] static/instance
  - [x] method group return type
- [x] Expression variables

custom fixed, movable fixed buffers は、Preview 3時点でも未実装だった。
Roslyn の、dev15.7-preview3 ブランチには入ってなくて、dev15.7.x ブランチには入ってること確認済み。
 */

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
