namespace VersionSample.Csharp6
{
    /// <summary>
    ///  null 条件演算子。
    /// これも、一度変数に受ける必要がなくなったり、? : 演算子のウザったさがなくなるだけの割とシンプルの構文糖衣。
    /// </summary>
    public class NullConditionalSample
    {
        public static int X(Entry x)
        {
            return x?.Name.Length ?? 0;
        }

        public static int SameAsX(Entry x)
        {
            if (x == null) return 0;
            return x.Name.Length;
        }

        public static int? Y(Entry x)
        {
            return x?.Name?.Length;
        }

        public static int? SameAsY(Entry x)
        {
            if (x == null) return null;

            var name = x.Name;
            if (name == null) return null;
            return name.Length;

            // 下3行は、
            //if (x.Name) return null;
            //return x.Name.Length;
            // ではなく、一度ローカル変数に受けたようなコードになる。
            //x.Name == null ? null : x.Name.Length;
            // みたいな、プロパティ呼び出しが2回にはならない。
            // これが、? : 演算子ではできない。
        }

        public static int? DifferentFromY(Entry x)
        {
            var name = x == null ? null : x.Name;
            // ?. は短絡評価なので、↑がnullだった時点でreturnして、↓の評価は走らなくなる。
            return name == null ? (int?)null : name.Length;

            // つまり、
            // (1)
            //return x?.Name?.Length;
            // と、
            // (2)
            //var name = x?.Name;
            //return name?.Length;
            // は意味が変わるので注意。
        }
    }

    public class Entry
    {
        public int Id { get; }

        public string Name { get; }

        public Entry(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
