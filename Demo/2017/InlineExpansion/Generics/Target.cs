using System;

namespace Generics
{
    interface ISample
    {
        int Value { get; }
    }

    class CInt : ISample
    {
        public CInt(int value) => Value = value;
        public int Value { get; }
        public static implicit operator CInt(int value) => new CInt(value);
    }

    class CString : ISample
    {
        public CString(string value) => _value = value;
        public int Value => _value.Length;
        public string _value;
        public static implicit operator CString(string value) => new CString(value);
    }

    struct SInt : ISample
    {
        public SInt(int value) => Value = value;
        public int Value { get; }
        public static implicit operator SInt(int value) => new SInt(value);
    }

    struct SString : ISample
    {
        public SString(string value) => _value = value;
        public int Value => _value.Length;
        public string _value;
        public static implicit operator SString(string value) => new SString(value);
    }

    /// <summary>
    /// 計測の対象。
    /// </summary>
    class Target
    {
        public static T Max<T>(T x, T y)
            where T : ISample
            => x.Value > y.Value ? x : y;
    }

    /// <summary>
    /// <see cref="Target.Max{T}(T, T)"/>を、string, Version, int, DateTime 辺りで呼んだ場合、どんな感じになるか手動で展開したもの。
    /// </summary>
    class Instantiated
    {
        // Max<CInt>, Max<CString> はこういう感じの単一のメソッド呼び出しに展開される。
        // 参照型に対しては同じメソッドが使いまわされる。
        // ジェネリックにするメリットは、キャストが消える程度(コスト的にはそこまで大きくない。どちらかというと型安全性の問題)。
        public static object Max(object x, object y)
            => ((ISample)x).Value > ((ISample)y).Value ? x : y;

        // 値型に対しては1個1個完全展開。
        // object 版に値型を渡してしまうと box 化(そこそこのコスト)を起こすので、JITがそれぞれの型専用のコードに展開してしまう。
        public static SInt MaxSInt(SInt x, SInt y) => x.Value > y.Value ? x : y;
        public static SString MaxSString(SString x, SString y) => x.Value > y.Value ? x : y;
    }
}
