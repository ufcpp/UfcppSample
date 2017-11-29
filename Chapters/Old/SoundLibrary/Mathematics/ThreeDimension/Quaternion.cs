using System;

namespace SoundLibrary.Mathematics.ThreeDimension
{
	/// <summary>
	/// ハミルトンの四減数。
	/// </summary>
	public struct Quaternion
	{
		#region フィールド

		/// <summary>
		/// 実部。
		/// </summary>
		public double a;

		/// <summary>
		/// 虚部。
		/// </summary>
		public Vector  u;

		#endregion
		#region 初期化

		/// <summary>
		/// 実部と虚部ベクトルを指定して初期化。
		/// a + ii・uu、ii=(i,j,k)、uu＝(p,q,r)。
		/// </summary>
		/// <param name="a">実部</param>
		/// <param name="u">虚部ベクトル</param>
		public Quaternion(double a, Vector u)
		{
			this.a = a;
			this.u = u;
		}

		/// <summary>
		/// 実部と虚部の要素を指定して初期化。
		/// a + i p + j q + k r。
		/// </summary>
		/// <param name="a">実部</param>
		/// <param name="p">虚部の i 要素</param>
		/// <param name="q">虚部の j 要素</param>
		/// <param name="r">虚部の k 要素</param>
		public Quaternion(double a, double p, double q, double r)
			: this(a, new Vector(p, q, r))
		{
		}

		#endregion
		#region 演算子

		/// <summary>
		/// x + y。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <returns>x + y</returns>
		public static Quaternion operator+ (Quaternion x, Quaternion y)
		{
			return new Quaternion(x.a + y.a, x.u + y.u);
		}

		/// <summary>
		/// x - y。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <returns>x - y</returns>
		public static Quaternion operator- (Quaternion x, Quaternion y)
		{
			return new Quaternion(x.a - y.a, x.u - y.u);
		}

		/// <summary>
		/// -x。
		/// </summary>
		/// <param name="x">x</param>
		/// <returns>-x</returns>
		public static Quaternion operator- (Quaternion x)
		{
			return new Quaternion(-x.a, -x.u);
		}

		/// <summary>
		/// x × y。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <returns>x × y</returns>
		public static Quaternion operator* (Quaternion x, Quaternion y)
		{
			double a = x.a * y.a - Vector.InnerProduct(x.u, y.u);
			Vector  u = x.a * y.u + y.a * x.u + Vector.OuterProduct(x.u, y.u);
			return new Quaternion(a, u);
		}

		/// <summary>
		/// x ÷ y。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <returns>x ÷ y</returns>
		public static Quaternion operator/ (Quaternion x, Quaternion y)
		{
			return x * y.Inverse();
		}

		/// <summary>
		/// 実数p × x。
		/// </summary>
		/// <param name="p">実数p</param>
		/// <param name="x">x</param>
		/// <returns>p × x</returns>
		public static Quaternion operator* (double p, Quaternion x)
		{
			return new Quaternion(p * x.a, p * x.u);
		}

		/// <summary>
		/// x × 実数p。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="p">実数p</param>
		/// <returns>p × x</returns>
		public static Quaternion operator* (Quaternion x, double p){return p * x;}

		/// <summary>
		/// x ÷ 実数p。
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="p">実数p</param>
		/// <returns>x ÷ p</returns>
		public static Quaternion operator/ (Quaternion x, double p){return (1/p) * x;}

		#endregion
		#region その他のメソッド

		/// <summary>
		/// 二乗ノルム。
		/// </summary>
		public double Norm
		{
			get{return this.a * this.a + this.u.Norm;}
		}

		/// <summary>
		/// 共役四元数を求める。
		/// </summary>
		/// <returns>共役四元数</returns>
		public Quaternion Conjunction()
		{
			return new Quaternion(this.a, -this.u);
		}

		/// <summary>
		/// 逆数を求める。
		/// </summary>
		/// <returns>逆数</returns>
		public Quaternion Inverse()
		{
			return this.Conjunction() / this.Norm;
		}

		#endregion
		#region 回転がらみの static メソッド

		/// <summary>
		/// 四元数を使って3次元空間上の回転。
		/// p × (0, x) × ~p を計算する(~p は p の共役)。
		/// </summary>
		/// <param name="p">回転軸/角を表す四元数</param>
		/// <param name="x">回転させたい点のベクトル</param>
		/// <returns>回転後の点のベクトル</returns>
		public static Vector Rotate(Quaternion p, Vector x)
		{
			Vector y = (p.a * p.a - p.u.Norm) * x;
			y += 2 * (Vector.InnerProduct(p.u, x) * p.u + p.a * Vector.OuterProduct(p.u, x));
			return y;
		}

		/// <summary>
		/// 四元数を使って3次元空間上の回転。
		/// p × x × ~q を計算する(~p は p の共役)。
		/// </summary>
		/// <param name="p">p</param>
		/// <param name="x">x</param>
		/// <returns>p × x × ~q</returns>
		public static Quaternion Rotate(Quaternion p, Quaternion x)
		{
			return new Quaternion(x.a, Rotate(p, x.u));
		}

		/// <summary>
		/// ベクトル(axis)を軸として、θ(theta)回転するための四元数を計算する。
		/// </summary>
		/// <param name="theta">回転角θ</param>
		/// <param name="axis">回転軸ベクトル</param>
		/// <returns>回転を表す四元数</returns>
		public static Quaternion Rotator(double theta, Vector axis)
		{
			theta *= 0.5;
			axis *= Math.Sin(theta) / axis.Abs;
			return new Quaternion(Math.Cos(theta), axis);
		}

		#endregion
		#region 文字列化

		public override string ToString()
		{
			return "(" + this.a.ToString() + ", " + this.u.ToString() + ")";
		}

	
		#endregion
	}
}
