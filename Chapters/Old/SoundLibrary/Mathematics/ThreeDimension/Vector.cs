using System;

namespace SoundLibrary.Mathematics.ThreeDimension
{
	/// <summary>
	/// 3次元ベクトル。
	/// </summary>
	public struct Vector
	{
		#region フィールド

		public double x, y, z;

		#endregion
		#region 初期化

		public Vector(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		#endregion
		#region ノルム・絶対値

		public double Norm
		{
			get{return this.x * this.x + this.y * this.y + this.z * this.z;}
		}

		public double Abs
		{
			get{return Math.Sqrt(this.Norm);}
		}

		#endregion
		#region 演算子

		public static Vector operator+ (Vector a, Vector b)
		{
			return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector operator- (Vector a, Vector b)
		{
			return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector operator- (Vector a)
		{
			return new Vector(-a.x, -a.y, -a.z);
		}

		public static Vector operator* (double p, Vector a)
		{
			return new Vector(p * a.x, p * a.y, p * a.z);
		}

		public static Vector operator* (Vector a, double p){return p * a;}
		public static Vector operator/ (Vector a, double p){return (1/p) * a;}

		#endregion
		#region 内積・外積

		public static double InnerProduct(Vector a, Vector b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public static Vector OuterProduct(Vector a, Vector b)
		{
			double x = a.y * b.z - a.z * b.y;
			double y = a.z * b.x - a.x * b.z;
			double z = a.x * b.y - a.y * b.x;
			return new Vector(x, y, z);
		}

		#endregion
		#region 文字列化

		public override string ToString()
		{
			return "(" + this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString() + ")";
		}

		#endregion
	}
}
