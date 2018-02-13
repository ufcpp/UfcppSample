using System.Numerics;

namespace Simd
{
    public struct Matrix
    {
        public float M11;
        public float M21;
        public float M31;
        public float M41;
        public float M12;
        public float M22;
        public float M32;
        public float M42;
        public float M13;
        public float M23;
        public float M33;
        public float M43;
        public float M14;
        public float M24;
        public float M34;
        public float M44;

        public Matrix(float m11, float m21, float m31, float m41, float m12, float m22, float m32, float m42, float m13, float m23, float m33, float m43, float m14, float m24, float m34, float m44)
        {
            M11 = m11;
            M21 = m21;
            M31 = m31;
            M41 = m41;
            M12 = m12;
            M22 = m22;
            M32 = m32;
            M42 = m42;
            M13 = m13;
            M23 = m23;
            M33 = m33;
            M43 = m43;
            M14 = m14;
            M24 = m24;
            M34 = m34;
            M44 = m44;
        }

        public unsafe static Matrix operator +(in Matrix a, in Matrix b)
        {
            fixed (float* pfa = &a.M11)
            fixed (float* pfb = &b.M11)
            {
                var m = default(Matrix);
                var p = (Vector4*)&m;
                var pa = (Vector4*)pfa;
                var pb = (Vector4*)pfb;
                p[0] = pa[0] + pb[0];
                p[1] = pa[1] + pb[1];
                p[2] = pa[2] + pb[2];
                p[3] = pa[3] + pb[3];

                // こんな感じのコード書きたいんだけど、まだ対応してるランタイムがなさげ
                //Sse.Store(p, Sse.Add(Sse.LoadVector128(pa), Sse.LoadVector128(pb)));
                //Sse.Store(p + 4, Sse.Add(Sse.LoadVector128(pa + 4), Sse.LoadVector128(pb + 4)));
                //Sse.Store(p + 8, Sse.Add(Sse.LoadVector128(pa + 8), Sse.LoadVector128(pb + 8)));
                //Sse.Store(p + 12, Sse.Add(Sse.LoadVector128(pa + 12), Sse.LoadVector128(pb + 12)));

                return m;
            }
        }

        public unsafe static Matrix operator *(in Matrix a, in Matrix b)
        {
            // Vector4 を介して速くする方法がなかなか思いつかなくて頓挫中
            // SSE が使えるなら https://gist.github.com/rygorous/4172889 こんな感じで計算できるはずなんだけども
            // System.Numerics.Vector4 には Sse.Shuffle 相当の機能がなくて、Vector4 を使って速くする方法なさそう。
            // 下手に Vector4 を使うより、↓みたいなベタ書きの方が速かった。

            float m11 = a.M11 * b.M11 + a.M21 * b.M12 + a.M31 * b.M13 + a.M41 * b.M14;
            float m21 = a.M11 * b.M21 + a.M21 * b.M22 + a.M31 * b.M23 + a.M41 * b.M24;
            float m31 = a.M11 * b.M31 + a.M21 * b.M32 + a.M31 * b.M33 + a.M41 * b.M34;
            float m41 = a.M11 * b.M41 + a.M21 * b.M42 + a.M31 * b.M43 + a.M41 * b.M44;
            float m12 = a.M12 * b.M11 + a.M22 * b.M12 + a.M32 * b.M13 + a.M42 * b.M14;
            float m22 = a.M12 * b.M21 + a.M22 * b.M22 + a.M32 * b.M23 + a.M42 * b.M24;
            float m32 = a.M12 * b.M31 + a.M22 * b.M32 + a.M32 * b.M33 + a.M42 * b.M34;
            float m42 = a.M12 * b.M41 + a.M22 * b.M42 + a.M32 * b.M43 + a.M42 * b.M44;
            float m13 = a.M13 * b.M11 + a.M23 * b.M12 + a.M33 * b.M13 + a.M43 * b.M14;
            float m23 = a.M13 * b.M21 + a.M23 * b.M22 + a.M33 * b.M23 + a.M43 * b.M24;
            float m33 = a.M13 * b.M31 + a.M23 * b.M32 + a.M33 * b.M33 + a.M43 * b.M34;
            float m43 = a.M13 * b.M41 + a.M23 * b.M42 + a.M33 * b.M43 + a.M43 * b.M44;
            float m14 = a.M14 * b.M11 + a.M24 * b.M12 + a.M34 * b.M13 + a.M44 * b.M14;
            float m24 = a.M14 * b.M21 + a.M24 * b.M22 + a.M34 * b.M23 + a.M44 * b.M24;
            float m34 = a.M14 * b.M31 + a.M24 * b.M32 + a.M34 * b.M33 + a.M44 * b.M34;
            float m44 = a.M14 * b.M41 + a.M24 * b.M42 + a.M34 * b.M43 + a.M44 * b.M44;

            return new Matrix(
                m11, m21, m31, m41,
                m12, m22, m32, m42,
                m13, m23, m33, m43,
                m14, m24, m34, m44);

            // ↓みたいなのは精度悪すぎてダメだった。
#if false
            var r1 = new Vector4(b.M11, b.M12, b.M13, b.M14);
            var r2 = new Vector4(b.M21, b.M22, b.M23, b.M24);
            var r3 = new Vector4(b.M31, b.M32, b.M33, b.M34);
            var r4 = new Vector4(b.M41, b.M42, b.M43, b.M44);

            fixed (float* pfa = &a.M11)
            {
                var pa = (Vector4*)pfa;

                float m11 = Vector4.Dot(pa[0], r1);
                float m21 = Vector4.Dot(pa[0], r2);
                float m31 = Vector4.Dot(pa[0], r3);
                float m41 = Vector4.Dot(pa[0], r4);
                float m12 = Vector4.Dot(pa[1], r1);
                float m22 = Vector4.Dot(pa[1], r2);
                float m32 = Vector4.Dot(pa[1], r3);
                float m42 = Vector4.Dot(pa[1], r4);
                float m13 = Vector4.Dot(pa[2], r1);
                float m23 = Vector4.Dot(pa[2], r2);
                float m33 = Vector4.Dot(pa[2], r3);
                float m43 = Vector4.Dot(pa[2], r4);
                float m14 = Vector4.Dot(pa[3], r1);
                float m24 = Vector4.Dot(pa[3], r2);
                float m34 = Vector4.Dot(pa[3], r3);
                float m44 = Vector4.Dot(pa[3], r4);

                return new Matrix(
                    m11, m21, m31, m41,
                    m12, m22, m32, m42,
                    m13, m23, m33, m43,
                    m14, m24, m34, m44);
            }
#endif
        }
    }
}
