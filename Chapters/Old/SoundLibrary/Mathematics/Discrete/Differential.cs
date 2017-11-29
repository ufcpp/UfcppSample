using System;

namespace SoundLibrary.Mathematics.Discrete
{
	using Type = System.Double;

	/// <summary>
	/// 導関数。
	/// </summary>
	public class Differential : Function
	{
		Function primitive;

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="primitive">原始関数</param>
		public Differential(Function primitive)
		{
			this.primitive = primitive;
		}

		public override int Begin
		{
			get
			{
				return this.primitive.Begin;
			}
		}

		public override int End
		{
			get
			{
				return this.primitive.End;
			}
		}

		public override int Length
		{
			get
			{
				return this.primitive.Length;
			}
		}

		public override Type this[int n]
		{
			get
			{
				if(n == this.primitive.Begin    ) return ForwardDifference(this.primitive, n);
				if(n == this.primitive.Begin + 1) return Diffrential3(this.primitive, n);
				if(n == this.primitive.End - 2  ) return Diffrential3(this.primitive, n);
				if(n == this.primitive.End - 1  ) return BackwardDifference(this.primitive, n);
				return Diffrential5(this.primitive, n);
			}
		}

		/// <summary>
		/// 前進差分近似で微分。
		/// </summary>
		/// <param name="f">微分対象</param>
		/// <param name="i">位置</param>
		/// <returns>微分結果</returns>
		public static Type ForwardDifference(Function f, int i)
		{
			return f[i+1] - f[i];
		}

		/// <summary>
		/// 後退差分近似で微分。
		/// </summary>
		/// <param name="f">微分対象</param>
		/// <param name="i">位置</param>
		/// <returns>微分結果</returns>
		public static Type BackwardDifference(Function f, int i)
		{
			return f[i] - f[i-1];
		}

		/// <summary>
		/// 3点近似で微分。
		/// </summary>
		/// <param name="f">微分対象</param>
		/// <param name="i">位置</param>
		/// <returns>微分結果</returns>
		public static Type Diffrential3(Function f, int i)
		{
			return (f[i+1] - f[i-1]) / 2;
		}

		/// <summary>
		/// 5点近似で微分。
		/// </summary>
		/// <param name="f">微分対象</param>
		/// <param name="i">位置</param>
		/// <returns>微分結果</returns>
		public static Type Diffrential5(Function f, int i)
		{
			return (-f[i+2] + 8 * f[i+1] - 8 * f[i-1] + f[i-2]) / 12;
		}

		/// <summary>
		/// 配列を関数に見立てて微分。
		/// </summary>
		/// <param name="x">微分対象</param>
		/// <returns>微分結果</returns>
		public static Type[] Derive(Type[] x)
		{
			Type[] y = new Type[x.Length];
			Differential dx = new Differential(Function.FromArray(x));
			for(int i=0; i<y.Length; ++i)
			{
				y[i] = dx[i];
			}
			return y;
		}
	}
}
