using System;

namespace Filter
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

			this.buff = new CircularBuffer(a.Length);
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
	}//class IirFilter

	/// <summary>
	/// 2次IIRを用いたピーキングイコライザ。
	/// </summary>
	public class PeakingEqualizer : IirFilter
	{
		/// <summary>
		/// ピーキングイコライザを作成。
		/// </summary>
		/// <param name="w">中心周波数</param>
		/// <param name="Q">Q値</param>
		/// <param name="A">増幅率(リニア値)</param>
		public PeakingEqualizer(double w, double Q, double A) : base(2)
		{
#if true
			double a = Math.Sqrt(A);
			double sn = Math.Sin(w);
			double cs = Math.Cos(w);
			double alpha = sn / (2 * Q);
			double a0 = 1 + alpha / a;
			double a1 = -2 * cs;
			double a2 = 1 - alpha / a;
			double b0 = 1 + alpha * a;
			double b1 = -2 * cs;
			double b2 = 1 - alpha * a;

			this.A[0] = -a1 / a0;
			this.A[1] = -a2 / a0;
			this.B[0] = b0 / a0;
			this.B[1] = b1 / a0;
			this.B[2] = b2 / a0;
#else
			double g = A;
			double Ft = 1 / Math.Tan(w / 2);

			double term1 = 1.0 + Ft*g/Q + Ft*Ft;
			double term2 = 1.0 + Ft/Q + Ft*Ft;

			//Peekを作る
			if(A > 1)
			{
				this.B[0] = term1 / term2;
				this.B[1] = 2.0*(1.0 - Ft*Ft)/term2;
				this.B[2] = (1.0 - Ft*g/Q + Ft*Ft) / term2;
				this.A[0] = - this.B[1];
				this.A[1] = - (1.0 - Ft/Q + Ft*Ft)/ term2;
			}
			//dipを作る
			else if(A < 1)
			{
				this.B[0] = term2 / term1;
				this.B[1] = 2.0*(1.0 - Ft*Ft)/term1;
				this.B[2] = (1.0 - Ft / Q + Ft*Ft) / term1;
				this.A[0] = - this.B[1];
				this.A[1] = - (1.0 - Ft*g/Q + Ft*Ft)/ term1;
			}
			else
			{
				this.B[0] = 1.0;
				this.B[1] = 0.0;
				this.B[2] = 0.0;
				this.A[0] = 0.0; 
				this.A[1] = 0.0;   
			}
#endif
		}
	}//class PeakingEqualizer


	/// <summary>
	/// 2次IIRを用いたピーキングイコライザ。
	/// </summary>
	public class ShelvingEqualizer : IirFilter
	{
		/// <summary>
		/// ピーキングイコライザを作成。
		/// </summary>
		/// <param name="w">中心周波数</param>
		/// <param name="A">増幅率(リニア値)</param>
		public ShelvingEqualizer(double w, double A) : base(1)
		{
			double tn = (Math.Sin(w) - 1) / Math.Cos(w);
			double g = 1/A;//Math.Sqrt(A);

			double term1 = g*(1.0+tn) - (1.0-tn);
			double term2 = g*(1.0+tn) + (1.0-tn);

			if(g > 1)
			{
				this.A[0]= term1/term2;
				this.B[0]= 2.0/term2;
				this.B[1]= tn;
			}
			else if(g < 1)
			{
				this.A[0]= tn;
				this.B[0]= term2/2.0;
				this.B[1]= term1/term2; 
			}
			else
			{
				this.A[0]= 0.0;
				this.B[0]= 1.0;
				this.B[1]= 0.0;
			}
		}
	}//class ShelvingEqualizer
}//namespace Filter
