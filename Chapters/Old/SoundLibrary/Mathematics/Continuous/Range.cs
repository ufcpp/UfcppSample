using System;

namespace SoundLibrary.Mathematics.Continuous
{
	/// <summary>
	/// 定義域の範囲を表す構造体。
	/// </summary>
	public struct Range
	{
		bool hasMin;
		bool hasMax;
		double inf;
		double sup;

		/// <summary>
		/// 最小・最大値の有無、上限・下限を指定して初期化。
		/// 例:
		/// (1, 2] → new Range(false, true, 1, 2)、
		/// [0, 1] → new Range(true, true, 0, 1)
		/// </summary>
		/// <param name="hasMin"></param>
		/// <param name="hasMax"></param>
		/// <param name="inf"></param>
		/// <param name="sup"></param>
		public Range(bool hasMin, bool hasMax, double inf, double sup)
		{
			this.hasMax = hasMax;
			this.hasMin = hasMin;
			this.sup = sup;
			this.inf = inf;
		}

		/// <summary>
		/// 最大値を持つかどうか。
		/// </summary>
		public bool HasMaximum
		{
			get{return this.hasMax;}
			set{this.hasMax = value;}
		}

		/// <summary>
		/// 最小値を持つかどうか。
		/// </summary>
		public bool HasMinimum
		{
			get{return this.hasMin;}
			set{this.hasMin = value;}
		}

		/// <summary>
		/// 上限。
		/// </summary>
		public double Supremum
		{
			get{return this.sup;}
			set{this.sup = value;}
		}

		/// <summary>
		/// 下限。
		/// </summary>
		public double Infimum
		{
			get{return this.inf;}
			set{this.inf = value;}
		}
	}
}
