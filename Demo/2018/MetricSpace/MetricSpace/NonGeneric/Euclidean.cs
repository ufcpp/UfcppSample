namespace MetricSpace.NonGeneric
{
    class Euclidean
    {
        // a と b の長さが同じとか、いくつか前提を置いちゃってるけども、最低限のコード
        // a, b を N 次元空間上の点とみなして、その間の距離の2乗、
        // 要するに「差の2乗和」を求める。
        public static float DistanceSquared(float[] a, float[] b)
        {
            var d = 0f;
            for (int i = 0; i < a.Length; i++)
            {
                var dif = b[i] - a[i];
                d += dif * dif;
            }
            return d;
        }
    }

#if Uncompilable
    // int や double でも使いたいからと言って、以下のようには書けない。
    // ジェネリックな型 T には +, -, * が定義されていない。
    class Euclidean<T>
    {
        public static T DistanceSquared(T[] a, T[] b)
        {
            T d = 0;
            for (int i = 0; i < a.Length; i++)
            {
                var dif = b[i] - a[i];
                d += dif * dif;
            }
            return d;
        }
    }
#endif
}
