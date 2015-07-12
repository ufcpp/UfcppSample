using System;

namespace SoundLibrary.Mathematics.ThreeDimension
{
	/// <summary>
	/// 1次変換用の3×3行列。
	/// 回転などは右手系を仮定。
	/// </summary>
	public class Matrix
	{
		#region フィールド

		double ax, bx, cx;
		double ay, by, cy;
		double az, bz, cz;

		#endregion
		#region 初期化

		public Matrix()
			:this(0, 0, 0, 0, 0, 0, 0, 0, 0) {}

		public Matrix(Vector x, Vector y, Vector z)
			:this(
			x.x, y.x, z.x,
			x.y, y.y, z.y,
			x.z, y.z, z.z){}

		public Matrix(
			double ax, double bx, double cx,
			double ay, double by, double cy,
			double az, double bz, double cz)
		{
			this.Set(
				ax, bx, cx,
				ay, by, cy,
				az, bz, cz);
		}

		public void Set(
			double ax, double bx, double cx,
			double ay, double by, double cy,
			double az, double bz, double cz)
		{
			this.ax = ax;
			this.bx = bx;
			this.cx = cx;

			this.ay = ay;
			this.by = by;
			this.cy = cy;

			this.az = az;
			this.bz = bz;
			this.cz = cz;
		}

		public void Set(Matrix a)
		{
			this.ax = a.ax;
			this.bx = a.bx;
			this.cx = a.cx;

			this.ay = a.ay;
			this.by = a.by;
			this.cy = a.cy;

			this.az = a.az;
			this.bz = a.bz;
			this.cz = a.cz;
		}

		#endregion
		#region 要素の参照

		#region 行
		
		public class Row
		{
			Matrix a;
			int i;
			internal Row(Matrix a, int i){ this.a = a; this.i = i;}

			public double X
			{
				get
				{
					switch(i)
					{
						case 0: return a.ax;
						case 1: return a.ay;
						case 2: return a.az;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.ax = value; break;
						case 1: a.ay = value; break;
						case 2: a.az = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}

			public double Y
			{
				get
				{
					switch(i)
					{
						case 0: return a.bx;
						case 1: return a.by;
						case 2: return a.bz;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.bx = value; break;
						case 1: a.by = value; break;
						case 2: a.bz = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}

			public double Z
			{
				get
				{
					switch(i)
					{
						case 0: return a.cx;
						case 1: return a.cy;
						case 2: return a.cz;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.cx = value; break;
						case 1: a.cy = value; break;
						case 2: a.cz = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}


			public static explicit operator Vector (Row r)
			{
				return new Vector(r.X, r.Y, r.Z);
			}
		}

		public Row Rows(int i)
		{
			return new Row(this, i);
		}

		#endregion
		#region 列
		
		public class Column
		{
			Matrix a;
			int i;
			internal Column(Matrix a, int i){ this.a = a; this.i = i;}

			public double X
			{
				get
				{
					switch(i)
					{
						case 0: return a.ax;
						case 1: return a.bx;
						case 2: return a.cx;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.ax = value; break;
						case 1: a.bx = value; break;
						case 2: a.cx = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}

			public double Y
			{
				get
				{
					switch(i)
					{
						case 0: return a.ay;
						case 1: return a.by;
						case 2: return a.cy;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.ay = value; break;
						case 1: a.by = value; break;
						case 2: a.cy = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}

			public double Z
			{
				get
				{
					switch(i)
					{
						case 0: return a.az;
						case 1: return a.bz;
						case 2: return a.cz;
						default: throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch(i)
					{
						case 0: a.az = value; break;
						case 1: a.bz = value; break;
						case 2: a.cz = value; break;
						default: throw new IndexOutOfRangeException();
					}
				}
			}


			public static explicit operator Vector (Column r)
			{
				return new Vector(r.X, r.Y, r.Z);
			}
		}

		public Column Columns(int i)
		{
			return new Column(this, i);
		}

		#endregion

		public double this[int i, int j]
		{
			get
			{
				switch(i)
				{
					case 0:
					switch(j)
					{
						case 0: return this.ax;
						case 1: return this.bx;
						case 2: return this.cx;
						default: throw new IndexOutOfRangeException();
					}
					case 1:
					switch(j)
					{
						case 0: return this.ay;
						case 1: return this.by;
						case 2: return this.cy;
						default: throw new IndexOutOfRangeException();
					}
					case 2:
					switch(j)
					{
						case 0: return this.az;
						case 1: return this.bz;
						case 2: return this.cz;
						default: throw new IndexOutOfRangeException();
					}
					default: throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch(i)
				{
					case 0:
					switch(j)
					{
						case 0: this.ax = value; break;
						case 1: this.bx = value; break;
						case 2: this.cx = value; break;
						default: throw new IndexOutOfRangeException();
					}
						break;
					case 1:
					switch(j)
					{
						case 0: this.ay = value; break;
						case 1: this.by = value; break;
						case 2: this.cy = value; break;
						default: throw new IndexOutOfRangeException();
					}
						break;
					case 2:
					switch(j)
					{
						case 0: this.az = value; break;
						case 1: this.bz = value; break;
						case 2: this.cz = value; break;
						default: throw new IndexOutOfRangeException();
					}
						break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		#endregion
		#region 演算子

		public static Matrix operator+ (Matrix a, Matrix b)
		{
			return new Matrix(
				a.ax + b.ax, a.bx + b.bx, a.cx + b.cx,
				a.ay + b.ay, a.by + b.by, a.cy + b.cy,
				a.az + b.az, a.bz + b.bz, a.cz + b.cz);
		}

		public static Matrix operator- (Matrix a, Matrix b)
		{
			return new Matrix(
				a.ax - b.ax, a.bx - b.bx, a.cx - b.cx,
				a.ay - b.ay, a.by - b.by, a.cy - b.cy,
				a.az - b.az, a.bz - b.bz, a.cz - b.cz);
		}

		public static Matrix operator* (Matrix a, Matrix b)
		{
			double ax = a.ax * b.ax + a.bx * b.ay + a.cx * b.az;
			double ay = a.ay * b.ax + a.by * b.ay + a.cy * b.az;
			double az = a.az * b.ax + a.bz * b.ay + a.cz * b.az;

			double bx = a.ax * b.bx + a.bx * b.by + a.cx * b.bz;
			double by = a.ay * b.bx + a.by * b.by + a.cy * b.bz;
			double bz = a.az * b.bx + a.bz * b.by + a.cz * b.bz;

			double cx = a.ax * b.cx + a.bx * b.cy + a.cx * b.cz;
			double cy = a.ay * b.cx + a.by * b.cy + a.cy * b.cz;
			double cz = a.az * b.cx + a.bz * b.cy + a.cz * b.cz;

			return new Matrix(
				ax, bx, cx,
				ay, by, cy,
				az, bz, cz);
		}

		public static Matrix operator* (double x, Matrix a)
		{
			return new Matrix(
				x * a.ax, x * a.bx, x * a.cx,
				x * a.ay, x * a.by, x * a.cy,
				x * a.az, x * a.bz, x * a.cz);
		}

		public static Matrix operator* (Matrix a, double x){return x * a;}
		public static Matrix operator/ (Matrix a, double x){return (1/x) * a;}

		/// <summary>
		/// 行列とベクトルの積 A・x を計算。
		/// </summary>
		/// <param name="a">A</param>
		/// <param name="x">x</param>
		/// <returns>A・x</returns>
		public static Vector operator* (Matrix a, Vector v)
		{
			double x = a.ax * v.x + a.bx * v.y + a.cx * v.z;
			double y = a.ay * v.x + a.by * v.y + a.cy * v.z;
			double z = a.az * v.x + a.bz * v.y + a.cz * v.z;
			return new Vector(x, y, z);
		}

		/// <summary>
		/// 行列とベクトルの積 x^t・A を計算(x^t は x の転置)。
		/// </summary>
		/// <param name="a">A</param>
		/// <param name="x">x</param>
		/// <returns>x^t・A</returns>
		public static Vector operator* (Vector v, Matrix a)
		{
			double x = a.ax * v.x + a.ay * v.y + a.az * v.z;
			double y = a.bx * v.x + a.by * v.y + a.bz * v.z;
			double z = a.cx * v.x + a.cy * v.y + a.cz * v.z;
			return new Vector(x, y, z);
		}

		#endregion
		#region 逆行列・余因子・行列式

		/// <summary>
		/// 逆行列を求める。
		/// </summary>
		/// <returns>逆行列</returns>
		public Matrix Inverse()
		{
			double ax = this.by * this.cz - this.bz * this.cy;
			double ay = this.bz * this.cx - this.bx * this.cz;
			double az = this.bx * this.cy - this.by * this.cx;

			double bx = this.cy * this.az - this.cz * this.ay;
			double by = this.cz * this.ax - this.cx * this.az;
			double bz = this.cx * this.ay - this.cy * this.ax;

			double cx = this.ay * this.bz - this.az * this.by;
			double cy = this.az * this.bx - this.ax * this.bz;
			double cz = this.ax * this.by - this.ay * this.bx;

			double det = this.ax * ax + this.ay * ay + this.az * az;

			return new Matrix(
				ax / det, bx / det, cx / det,
				ay / det, by / det, cy / det,
				az / det, bz / det, cz / det);
		}

		/// <summary>
		/// 余因子行列を求める。
		/// </summary>
		/// <returns>余因子行列</returns>
		public Matrix Adjugate()
		{
			double ax = this.by * this.cz - this.bz * this.cy;
			double ay = this.bz * this.cx - this.bx * this.cz;
			double az = this.bx * this.cy - this.by * this.cx;

			double bx = this.cy * this.az - this.cz * this.ay;
			double by = this.cz * this.ax - this.cx * this.az;
			double bz = this.cx * this.ay - this.cy * this.ax;

			double cx = this.ay * this.bz - this.az * this.by;
			double cy = this.az * this.bx - this.ax * this.bz;
			double cz = this.ax * this.by - this.ay * this.bx;

			return new Matrix(
				ax, bx, cx,
				ay, by, cy,
				az, bz, cz);
		}

		/// <summary>
		/// 行列式を求める。
		/// </summary>
		/// <returns>行列式</returns>
		public double Determinant()
		{
			double ax = this.by * this.cz - this.bz * this.cy;
			double ay = this.bz * this.cx - this.bx * this.cz;
			double az = this.bx * this.cy - this.by * this.cx;

			return this.ax * ax + this.ay * ay + this.az * az;
		}

		#endregion
		#region 特殊な行列を作成

		#region 単位行列・零行列

		/// <summary>
		/// 単位行列。
		/// </summary>
		public static Matrix I
		{
			get
			{
				return new Matrix(
					1, 0, 0,
					0, 1, 0,
					0, 0, 1);
			}
		}

		/// <summary>
		/// 零行列。
		/// </summary>
		public static Matrix O
		{
			get
			{
				return new Matrix(
					0, 0, 0,
					0, 0, 0,
					0, 0, 0);
			}
		}

		#endregion
		#region x, y, z 軸を中心とする回転

		/// <summary>
		/// X 軸を中心に回転する行列を求める。
		/// </summary>
		/// <param name="theta">回転角</param>
		public void RotateX(double theta)
		{
			double cos = Math.Cos(theta);
			double sin = Math.Sin(theta);
			this.Set(
				1,   0,    0,
				0, cos, -sin,
				0, sin,  cos);
		}

		public static Matrix GetRotateX(double theta)
		{
			Matrix a = new Matrix();
			a.RotateX(theta);
			return a;
		}

		/// <summary>
		/// Y 軸を中心に回転する行列を求める。
		/// </summary>
		/// <param name="theta">回転角</param>
		public void RotateY(double theta)
		{
			double cos = Math.Cos(theta);
			double sin = Math.Sin(theta);
			this.Set(
				 cos, 0, sin,
				   0, 1,   0,
				-sin, 0, cos);
		}

		public static Matrix GetRotateY(double theta)
		{
			Matrix a = new Matrix();
			a.RotateY(theta);
			return a;
		}

		/// <summary>
		/// Z 軸を中心に回転する行列を求める。
		/// </summary>
		/// <param name="theta">回転角</param>
		public void RotateZ(double theta)
		{
			double cos = Math.Cos(theta);
			double sin = Math.Sin(theta);
			this.Set(
				cos, -sin, 0,
				sin,  cos, 0,
				  0,    0, 1);
		}

		public static Matrix GetRotateZ(double theta)
		{
			Matrix a = new Matrix();
			a.RotateZ(theta);
			return a;
		}

		#endregion
		#region 任意の軸を中心とする回転

		/// <summary>
		/// 軸ベクトル(axis)を中心にθ(theta)回転する行列を求める。
		/// </summary>
		/// <param name="theta">回転角</param>
		/// <param name="axis">回転軸ベクトル</param>
		public void Rotate(double theta, Vector axis)
		{
			axis /= axis.Abs;

			double cos = Math.Cos(theta);
			double sin = Math.Sin(theta);
			double c1  = 1 - cos;

			double yz = axis.y * axis.z * c1;
			double zx = axis.z * axis.x * c1;
			double xy = axis.x * axis.y * c1;

			double x =axis.x * sin;
			double y =axis.y * sin;
			double z =axis.z * sin;

			double ax = cos + c1 * axis.x * axis.x;
			double ay = xy + z;
			double az = zx - y;

			double bx = xy - z;
			double by = cos + c1 * axis.y * axis.y;
			double bz = yz + x;

			double cx = zx + y;
			double cy = yz - x;
			double cz = cos + c1 * axis.z * axis.z;

			this.Set(
				ax, bx, cx,
				ay, by, cy,
				az, bz, cz);
		}

		public static Matrix GetRotate(double theta, Vector axis)
		{
			Matrix a = new Matrix();
			a.Rotate(theta, axis);
			return a;
		}

		#endregion

		#endregion
	}
}
