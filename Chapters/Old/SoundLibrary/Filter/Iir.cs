using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// IIR フィルタクラス。
	/// </summary>
	public class IirFilter : IFilter
	{
		double[] a; // 分母係数配列
		double[] b; // 分子係数配列
		CircularBuffer buff; // 遅延バッファ

		/// <summary>
		/// デフォルトコンストラクタ。
		/// </summary>
		public IirFilter() : this(null, null) {}

		/// <summary>
		/// 次数を指定して IIR を作る。
		/// </summary>
		/// <param name="order">IIR の次数</param>
		public IirFilter(int order) : this(new double[order], new double[order + 1]) {}

		/// <summary>
		/// 係数を指定して IIR を作る。
		/// 作りたい IIR の伝達関数が、
		///      Σ_0^N B_i z^i
		/// Y = ---------------- X
		///      Σ_0^N A_i z^i
		/// であるとき、
		/// a[i] = - A_(i+1) / A_0    (i = 1～N)
		/// b[i] = B_i / A_0        (i = 0～N)
		/// </summary>
		/// <param name="a">分母係数配列</param>
		/// <param name="b">分子係数配列</param>
		public IirFilter(double[] a, double[] b)
		{
			this.SetCoefficient(a, b);

			this.Clear();
		}

		/// <summary>
		/// フィルタリングを行う。
		/// N: フィルタ次数 (= this.a.Length = this.b.Length - 1)
		/// x: 入力
		/// t: 中間データ
		/// y: 出力
		/// a[i]: 分母係数配列
		/// b[i]: 分子係数配列
		/// d[i]: i+1 サンプル前の t の値
		/// とすると、
		/// t = x + Σ_{i=0}^{N-1} a[i]d[i]
		/// y = t * b[0] + Σ_{i=0}^{N} b[i+1]d[i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			int N = this.a.Length;
			double t = x;
			for(int i=0; i<N; ++i)
			{
				t += this.buff[i] * this.a[i];
			}
			double y = t * this.b[0];
			for(int i=0; i<N; ++i)
			{
				y += this.buff[i] * this.b[i+1];
			}
			this.buff.PushFront(t);
			return y;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buff.Length; ++i)
			{
				this.buff[i] = 0;
			}
		}

		/// <summary>
		/// 係数の設定
		/// </summary>
		/// <param name="a">分母係数配列</param>
		/// <param name="b">分子係数配列</param>
		public void SetCoefficient(double[] a, double[]b)
		{
			if(a == null || b == null ||
				a.Length + 1 != b.Length)
			{
				this.buff = null;
				return;
			}

			if(this.buff == null)
				this.buff = new CircularBuffer(a.Length);
			else if(this.buff.Length < a.Length)
				this.buff.Resize(a.Length);

			this.a = a;
			this.b = b;
		}

		/// <summary>
		/// 分母係数配列の取得
		/// </summary>
		public double[] A
		{
			get{return this.a;}
		}

		/// <summary>
		/// 分母係数配列の取得
		/// </summary>
		public double[] B
		{
			get{return this.b;}
		}

		public object Clone()
		{
			return new IirFilter(this.a, this.b);
		}
	}//class IirFilter
}//namespace Filter
