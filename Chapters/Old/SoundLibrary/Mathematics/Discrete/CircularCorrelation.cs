using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Discrete
{
	using Type = System.Double;

	/// <summary>
	/// 配列 x と配列 y の循環相互相関値を求める。
	/// </summary>
	public class CircularCorrelation : Function
	{
		Type[] x;
		Type[] y;

		public CircularCorrelation(Type[] x, Type[] y)
		{
			if(x.Length != y.Length)
				throw new ArgumentException("x と y の長さは等しくなければなりません。");

			this.x = x;
			this.y = y;
		}

		public override int Begin
		{
			get
			{
				return 0;
			}
		}

		public override int End
		{
			get
			{
				return this.x.Length;
			}
		}

		public override int Length
		{
			get
			{
				return this.x.Length;
			}
		}

		/// <summary>
		/// 相互相関値 C[n] = Σ_i x[i]y[i-n] を求める。
		/// </summary>
		/// <param name="n">C[n] の n</param>
		/// <returns>相互相関値</returns>
		public override Type this[int n]
		{
			get
			{
				int len = this.x.Length;
				Type val = 0;

				int i=0;
				for(int j=n; j<len; ++i, ++j)
				{
					val += x[i] * y[j];
				}
				for(int j=0; i<len; ++i, ++j)
				{
					val += x[i] * y[j];
				}

				return val;
			}
		}
	}
}
