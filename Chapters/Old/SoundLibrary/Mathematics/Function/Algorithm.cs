using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// Algorithm の概要の説明です。
	/// </summary>
	/* static */
	public class Algorithm
	{
		/// <summary>
		/// 数値積分や探索のための変数の変化幅。
		/// 領域 [min, max] の間を刻み幅 step で探索する。
		/// </summary>
		public class Range
		{
			public Variable variable;
			public ValueType min;
			public ValueType max;
			public ValueType step;

			/// <summary>
			/// 最大値、最小値と、刻み幅を指定して初期化。
			/// </summary>
			/// <param name="variable">変数</param>
			/// <param name="min">最小値</param>
			/// <param name="max">最大値</param>
			/// <param name="step">刻み幅</param>
			public Range(Variable variable, ValueType min, ValueType max, ValueType step)
			{
				this.variable = variable;
				this.min = min;
				this.max = max;
				this.step = step;
			}

			/// <summary>
			/// 分割数を指定して初期化。
			/// </summary>
			/// <param name="variable">変数</param>
			/// <param name="min">最小値</param>
			/// <param name="max">最大値</param>
			/// <param name="n">分割数</param>
			public Range(Variable variable, ValueType min, ValueType max, int n)
			{
				this.variable = variable;
				this.min = min;
				this.max = max;
				this.step = (max - min) / (ValueType)n;
			}
		}

		/// <summary>
		/// 指定した範囲内/刻み幅で、関数 f の値が最小になるような引数を探す。
		/// (総当り)
		/// </summary>
		/// <param name="f">関数 f</param>
		/// <param name="rangeList">探索範囲/刻み幅</param>
		/// <returns>argmin f</returns>
		public static VariableTable Argmin(Function f, params Range[] rangeList)
		{
			VariableTable vars = new VariableTable();
			foreach(Range range in rangeList)
			{
				vars[range.variable] = range.min;
			}

			ValueType min = ValueType.MaxValue;
			VariableTable argmin = null;

			for(;;)
			{
				ValueType val = f[vars];
				if(val < min)
				{
					min = val;
					argmin = vars.Clone();
				}

				int i=0;
				for(; i<rangeList.Length; ++i)
				{
					ValueType x = vars[rangeList[i].variable];
					x += rangeList[i].step;

					if(x > rangeList[i].max)
					{
						vars[rangeList[i].variable] = rangeList[i].min;
					}
					else
					{
						vars[rangeList[i].variable] = x;
						break;
					}
				}
				if(i >= rangeList.Length) break;
			}

			return argmin;
		}

		/// <summary>
		/// 数値積分。(台形公式を使用。)
		/// </summary>
		/// <param name="f">被積分関数</param>
		/// <param name="range">積分範囲</param>
		/// <returns>数値積分結果</returns>
		public static Function Integral(Function f, Range range)
		{
			Variable x = range.variable;
			ValueType min = range.min;
			ValueType max = range.max;
			ValueType step = range.step;

			Function g = f.Bind(new Function.Parameter(x, min));
			g += f.Bind(new Function.Parameter(x, max));
			g /= 2;

			for(; min < max; min += step)
				g += f.Bind(new Function.Parameter(x, min));

			g *= step;
			return g;
		}
	}
}
