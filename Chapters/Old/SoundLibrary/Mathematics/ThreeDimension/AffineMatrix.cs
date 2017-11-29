using System;

namespace SoundLibrary.Mathematics.ThreeDimension
{
	/// <summary>
	/// アフィン変換用の4×4行列。
	/// </summary>
	/// <remarks>
	/// 4列目は (0, 0, 0, 1) 固定。
	/// アフィン行列 B は1次変換行列 A と ベクトル t を用いて、
	/// B = (A t)
	///     (0 1)
	/// と表される。
	/// </remarks>
	public class AffineMatrix
	{
		#region フィールド

		Matrix a;
		Vector t;

		#endregion
		#region 初期化

		public AffineMatrix() : this(new Matrix(), new Vector()) {}

		public AffineMatrix(Matrix a) : this(a, new Vector()) {}

		public AffineMatrix(Vector t) : this(Matrix.I, t) {}

		public AffineMatrix(Matrix a, Vector t)
		{
			this.a = a;
			this.t = t;
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// アフィン変換の1次変換行列部分。
		/// </summary>
		public Matrix A
		{
			get{return this.a;}
			set{this.a = value;}
		}

		/// <summary>
		/// アフィン変換の平行移動部分。
		/// </summary>
		public Vector T
		{
			get{return this.t;}
			set{this.t = value;}
		}

		#endregion
		#region 演算子

		public static AffineMatrix operator+ (AffineMatrix a, AffineMatrix b)
		{
			return new AffineMatrix(a.a + b.a, a.t + b.t);
		}

		public static AffineMatrix operator- (AffineMatrix a, AffineMatrix b)
		{
			return new AffineMatrix(a.a - b.a, a.t - b.t);
		}

		public static AffineMatrix operator* (AffineMatrix a, AffineMatrix b)
		{
			return new AffineMatrix(a.a * b.a, a.a * b.t + a.t);
		}

		public static AffineMatrix operator/ (AffineMatrix a, AffineMatrix b)
		{
			return a * b.Inverse();
		}

		public static Vector operator* (AffineMatrix a, Vector v)
		{
			return a.a * v + a.t;
		}

		#endregion
		#region 逆元

		public AffineMatrix Inverse()
		{
			Matrix ai = this.a.Inverse();
			return new AffineMatrix(ai, -(ai * this.t));
		}

		#endregion
	}
}
