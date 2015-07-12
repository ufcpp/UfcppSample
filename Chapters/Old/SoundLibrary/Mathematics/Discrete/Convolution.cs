using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Discrete
{
	using Type = System.Double;

	/// <summary>
	/// 配列 x と配列 y の畳込みを求める。
	/// </summary>
	public class Convolution : Function
	{
		Type[] x;
		Type[] y;

		public Convolution(Type[] x, Type[] y)
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
				return 2 * this.x.Length - 1;
			}
		}

		/// <summary>
		/// 畳込み C[n] = Σ_i x[i]y[n-i] を求める。
		/// </summary>
		/// <param name="n">C[n] の n</param>
		/// <returns>畳込み結果</returns>
		public override Type this[int n]
		{
			get
			{
				int len = this.x.Length;
				Type val = 0;
				if(n < len)
				{
					for(int i=0, j=n; i<=n; ++i, --j)
					{
						val += x[i] * y[j];
					}
				}
				else
				{
					for(int i=n-(len-1), j=len-1; i<len; ++i, --j)
					{
						val += x[i] * y[j];
					}
				}

				return val;
			}
		}
	}
}
