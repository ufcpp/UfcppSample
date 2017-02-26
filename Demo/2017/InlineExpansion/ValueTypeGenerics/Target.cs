namespace ValueTypeGenerics
{
    // 個別実装
    public class Embedded
    {
        public static int Sum(int[] items)
        {
            var sum = 0;
            foreach (var x in items) sum += x;
            return sum;
        }

        public static int Prod(int[] items)
        {
            var sum = 1;
            foreach (var x in items) sum *= x;
            return sum;
        }
    }

    // IGroup インターフェイスをを引数で受け取って使う
    public class InterfaceParameter
    {
        public static int Sum(int[] items, IGroup op)
        {
            var sum = op.Zero;
            foreach (var x in items) sum = op.Op(sum, x);
            return sum;
        }
    }

    // 値型ジェネリックを使って IGroup インスタンスを作る
    public class TypeClass
    {
        public static int Sm<TGroup>(int[] items)
            where TGroup : struct, IGroup
        {
            var g = default(TGroup);
            var sum = g.Zero;
            foreach (var x in items) sum = g.Op(sum, x);
            return sum;
        }
    }
}
