using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Discrete
{
	using Type = System.Double;

	/// <summary>
	/// 配列 x と配列 y の循環畳込みを求める。
	/// </summary>
	public class CircularConvolution : Function
	{
		Type[] x;
		Type[] y;

		public CircularConvolution(Type[] x, Type[] y)
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
				return this.Length;
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
		/// 循環畳込み C[n] = Σ_i x[i]y[n-i] を求める。
		/// </summary>
		/// <param name="n">C[n] の n</param>
		/// <returns>循環畳込み結果</returns>
		public override Type this[int n]
		{
			get
			{
				int len = this.x.Length;
				Type val = 0;

				int i=0;
				for(int j=n-1; i<n; ++i, --j)
				{
					val += x[i] * y[j];
				}
				for(int j=len-1; i<len; ++i, --j)
				{
					val += x[i] * y[j];
				}

				return val;
			}
		}
	}
}
