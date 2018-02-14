using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

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

        private unsafe struct RowView
        {
            private float* _p;
            public RowView(float* p) => _p = p;
            public RowView(Matrix* p) => _p = (float*)p;

            public Vector128<float> this[int index]
            {
                get => Sse.LoadVector128(_p + (index << 2));
                set => Sse.Store(_p + (index << 2), value);
            }
        }

        public unsafe static Matrix operator +(in Matrix a, in Matrix b)
        {
            if (Sse.IsSupported)
            {
                fixed (float* pa = &a.M11)
                fixed (float* pb = &b.M11)
                {
                    var c = default(Matrix);
                    var pc = (float*)&c;
                    Sse.Store(pc, Sse.Add(Sse.LoadVector128(pa), Sse.LoadVector128(pb)));
                    Sse.Store(pc + 4, Sse.Add(Sse.LoadVector128(pa + 4), Sse.LoadVector128(pb + 4)));
                    Sse.Store(pc + 8, Sse.Add(Sse.LoadVector128(pa + 8), Sse.LoadVector128(pb + 8)));
                    Sse.Store(pc + 12, Sse.Add(Sse.LoadVector128(pa + 12), Sse.LoadVector128(pb + 12)));
                    return c;
                }
            }
            else
            {
                return new Matrix(
                    a.M11 + b.M11,
                    a.M21 + b.M21,
                    a.M31 + b.M31,
                    a.M41 + b.M41,
                    a.M12 + b.M12,
                    a.M22 + b.M22,
                    a.M32 + b.M32,
                    a.M42 + b.M42,
                    a.M13 + b.M13,
                    a.M23 + b.M23,
                    a.M33 + b.M33,
                    a.M43 + b.M43,
                    a.M14 + b.M14,
                    a.M24 + b.M24,
                    a.M34 + b.M34,
                    a.M44 + b.M44);
            }
        }

        // linear combination:
        // a[0] * B.row[0] + a[1] * B.row[1] + a[2] * B.row[2] + a[3] * B.row[3]
        private unsafe static Vector128<float> LinearCombination(in Vector128<float> a, float* pb)
        {
            Vector128<float> result;
            result = Sse.Multiply(Sse.Shuffle(a, a, 0x00), Sse.LoadVector128(pb));
            result = Sse.Add(result, Sse.Multiply(Sse.Shuffle(a, a, 0x55), Sse.LoadVector128(pb + 4)));
            result = Sse.Add(result, Sse.Multiply(Sse.Shuffle(a, a, 0xaa), Sse.LoadVector128(pb + 8)));
            result = Sse.Add(result, Sse.Multiply(Sse.Shuffle(a, a, 0xff), Sse.LoadVector128(pb + 12)));
            return result;
        }

        public unsafe static Matrix operator *(in Matrix a, in Matrix b)
        {
            if (Sse.IsSupported)
            {
                fixed (float* pa = &a.M11)
                fixed (float* pb = &b.M11)
                {
                    var c = default(Matrix);
                    var pc = (float*)&c;

                    Sse.Store(pc, LinearCombination(Sse.LoadVector128(pa), pb));
                    Sse.Store(pc + 4, LinearCombination(Sse.LoadVector128(pa + 4), pb));
                    Sse.Store(pc + 8, LinearCombination(Sse.LoadVector128(pa + 8), pb));
                    Sse.Store(pc + 12, LinearCombination(Sse.LoadVector128(pa + 12), pb));

                    return c;
                }
            }
            else
            {
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
            }
        }
    }
}
