using System;

namespace SoundLibrary.Mathematics.Discrete
{
	using Type = System.Double;

	/// <summary>
	/// 離散関数を表すクラス。
	/// </summary>
	abstract public class Function
	{
		/// <summary>
		/// 定義域の下限。
		/// </summary>
		public abstract int Begin{get;}

		/// <summary>
		/// 定義域の上限 + 1。
		/// </summary>
		public abstract int End{get;}

		/// <summary>
		/// 定義域の長さ。
		/// </summary>
		public abstract int Length{get;}

		/// <summary>
		/// f[n] を求める。
		/// </summary>
		public abstract Type this[int n]{get;}

		/// <summary>
		/// 配列化。
		/// </summary>
		/// <returns>配列化したもの</returns>
		public virtual Type[] ToArray()
		{
			Type[] x = new Type[this.Length];

			for(int i=this.Begin; i<this.End; ++i)
			{
				x[i] = this[i];
			}
			return x;
		}

		/// <summary>
		/// func の値を最大にする引数の値を返す。
		/// </summary>
		/// <param name="func">argmax を求めたい関数。</param>
		/// <returns>argmax 値</returns>
		public static int Argmax(Function func)
		{
			return Argmax(func, func.Begin, func.End);
		}

		/// <summary>
		/// func の値を最大にする引数の値を返す。
		/// </summary>
		/// <param name="func">argmax を求めたい関数。</param>
		/// <param name="min">引数の範囲の最小値</param>
		/// <param name="max">引数の範囲の最大値</param>
		/// <returns>argmax 値</returns>
		public static int Argmax(Function func, int min, int max)
		{
			double maxVal = func[min];
			int argmax = min;

			for(int i=min+1; i<max; ++i)
			{
				double val = func[i];
				if(val > maxVal)
				{
					maxVal = val;
					argmax = i;
				}
			}

			return argmax;
		}

		#region Type[] を Function として扱うためのラッパー
		/// <summary>
		/// 配列の値をそのまま返す関数。
		/// </summary>
		class Array : Function
		{
			Type[] array;
			internal Array(Type[] array){this.array = array;}
			public override int Begin{get{return 0;}}
			public override int End{get{return this.array.Length;}}
			public override int Length{get{return this.array.Length;}}
			public override Double this[int n]{get{return this.array[n];}}
			public override Double[] ToArray(){return this.array;}
		}

		/// <summary>
		/// 配列の値をそのまま返す関数を作る。
		/// </summary>
		/// <param name="array">配列</param>
		/// <returns>配列の値をそのまま返す関数</returns>
		public static Function FromArray(Type[] array)
		{
			return new Array(array);
		}
		#endregion
	}
}
