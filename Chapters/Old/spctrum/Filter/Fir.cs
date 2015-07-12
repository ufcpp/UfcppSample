using System;

namespace Filter
{
	/// <summary>
	/// 窓関数用の delegate
	/// </summary>
	public delegate double WindowFunction(double x);

	/// <summary>
	/// FIR フィルタ関係の共通関数群
	/// </summary>
	class FirCommon
	{
		/// <summary>
		/// 線形位相ローパス/バンドパス/ハイパスフィルタのフィルタタイプ。
		/// </summary>
		public enum FirFilterType
		{
			LPF, BPF, HPF
		}

		/// <summary>
		/// 線形位相バンドパスフィルタの係数を計算する。
		/// 右側半分のみを計算。
		/// </summary>
		/// <param name="type">フィルタタイプ</param>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数(BPF の場合は遮断帯域幅、HPF の場合は π-遮断周波数)</param>
		/// <param name="w0">(BPF のみ) 中心周波数</param>
		/// <param name="window">窓関数</param>
		/// <returns>係数の右側半分を計算したもの</returns>
		public static void CalcLinearBPFCoefficient(FirFilterType type, double[] coef, double w, double w0, WindowFunction window)
		{
			int n = coef.Length;
			double sum;

			sum = coef[0] = window(0) * w;
			for(int i=1; i<n; ++i)
			{
				double tmp = window(i) * Math.Sin(w * i) / i;
				sum += tmp * 2;

				if(type == FirFilterType.LPF)
				{
					coef[i] = tmp;
				}
				else if(type == FirFilterType.HPF)
				{
					if(i%2 != 0)
						coef[i] = -tmp;
					else
						coef[i] = tmp;
				}
				else
				{
					coef[i] = 2 * Math.Cos(w0 * i) * tmp;
				}
			}

			for(int i=0; i<n; ++i)
			{
				coef[i] /= sum;
			}
		}//GetLinearBPFCoefficient

		/// <summary>
		/// 窓関数用。常に1を返す関数。
		/// </summary>
		public static double Constant1(double x)
		{
			return 1;
		}
	}//class FirCommon

	/// <summary>
	/// FIR フィルタクラス。
	/// </summary>
	public class FirFilter : IFilter
	{
		protected double[] coef; // 係数配列
		protected CircularBuffer buff; // 遅延バッファ

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public FirFilter() : this(null){}

		/// <summary>
		/// タップ数を指定して FIR を作る。
		/// </summary>
		/// <param name="taps">タップ数</param>
		public FirFilter(int taps) : this(new double[taps]){}

		/// <summary>
		/// 係数を指定して FIR を作る。
		/// </summary>
		/// <param name="coef">係数を格納した配列。</param>
		public FirFilter(double[] coef)
		{
			this.Coefficient = coef;
		}

		/// <summary>
		/// フィルタリングを行う。
		/// N: フィルタ次数 (= this.coef.Length - 1)
		/// x: 入力
		/// y: 出力
		/// c[i]: 係数配列
		/// d[i]: i+1 サンプル前の x の値
		/// とすると、
		/// y = x*c[N] + Σ_{i=0}^{N-1} d[i]*c[N-1-i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			int N = this.coef.Length - 1;
			double y = this.coef[N] * x;
			for(int i=0; i<N-1; ++i)
			{
				y += this.buff[i] * this.coef[i];
			}
			this.buff.PushBack(x);
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
		/// 係数の取得/設定
		/// </summary>
		public double[] Coefficient
		{
			set
			{
				this.coef = value;
				if(value == null)
					this.buff = null;
				else
					this.buff = new CircularBuffer(coef.Length - 1);
			}
			get
			{
				return this.coef;
			}
		}
	}//class FirFilter

	/// <summary>
	/// 線形位相 FIR フィルタクラス。
	/// 係数が実対象であることを利用して計算量/メモリ量削減。
	/// 奇数タップバージョン
	/// (タップ数 2n + 1 で、coef[n-i] == coef[n+i] foreach i)
	/// </summary>
	public class OddLinearFir : IFilter
	{
		protected double[] coef;
		protected CircularBuffer buff;

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public OddLinearFir() : this(null){}

		/// <summary>
		/// タップ数を指定して FIR を作る。
		/// </summary>
		/// <param name="n">タップ数 ＝ 2n + 1</param>
		public OddLinearFir(int n) : this(new double[n+1]){}

		/// <summary>
		/// 係数を指定して FIR を作る。
		/// </summary>
		/// <param name="coef">係数を格納した配列。</param>
		public OddLinearFir(double[] coef)
		{
			this.Coefficient = coef;
		}

		/// <summary>
		/// フィルタリングを行う。
		/// n: タップ数が 2n + 1 (n = this.coef.Length - 1)
		/// x: 入力
		/// y: 出力
		/// c[i]: 係数配列
		/// d[i]: i+1 サンプル前の x の値
		/// とすると、
		/// y = d[n]*c[0] + Σ_{i=1}^{n} (d[n+i] + d[n-i])*c[i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			this.buff.PushBack(x);

			int n = this.coef.Length - 1;
			double y = this.coef[0] * this.buff[n];
			for(int i=1; i<=n; ++i)
			{
				y += this.coef[i] * (this.buff[n+i] + this.buff[n-i]);
			}
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
		/// 係数の取得/設定
		/// </summary>
		public double[] Coefficient
		{
			set
			{
				this.coef = value;
				if(value == null)
					this.buff = null;
				else
					this.buff = new CircularBuffer(2*coef.Length - 1);
			}
			get
			{
				int n = this.coef.Length;
				double[] tmp = new double[this.coef.Length * 2 - 1];
				for(int i=0; i<n; ++i)
				{
					tmp[n-1 + i] = tmp [n-1 - i] = this.coef[i];
				}

				return tmp;
			}
		}
	}//class OddLinearFir

	/// <summary>
	/// ローパス FIR フィルタ。
	/// </summary>
	public class LowPassFir : OddLinearFir
	{
		/// <summary>
		/// ローパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		public LowPassFir(int n, double w) : this(n, w, new WindowFunction(FirCommon.Constant1)){}
		
		/// <summary>
		/// ローパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public LowPassFir(int n, double w, WindowFunction window) : base(n)
		{
			FirCommon.CalcLinearBPFCoefficient(FirCommon.FirFilterType.LPF, this.coef, w, 0, window);
		}
	}//class LowPassFir

	/// <summary>
	/// ハイパス FIR フィルタ。
	/// </summary>
	public class HighPassFir : OddLinearFir
	{
		/// <summary>
		/// ハイパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		public HighPassFir(int n, double w) : this(n, w, new WindowFunction(FirCommon.Constant1)){}
		
		/// <summary>
		/// ハイパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public HighPassFir(int n, double w, WindowFunction window) : base(n)
		{
			FirCommon.CalcLinearBPFCoefficient(FirCommon.FirFilterType.HPF, this.coef, Math.PI - w, 0, window);
		}
	}//class HighPassFir

	/// <summary>
	/// バンドパス FIR フィルタ。
	/// </summary>
	public class BandPassFir : OddLinearFir
	{
		/// <summary>
		/// バンドパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="wl">下限周波数</param>
		/// <param name="wh">上限周波数</param>
		public BandPassFir(int n, double wl, double wh) : this(n, wl, wh, new WindowFunction(FirCommon.Constant1)){}
		
		/// <summary>
		/// バンドパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="wl">下限周波数</param>
		/// <param name="wh">上限周波数</param>
		/// <param name="window">窓関数</param>
		public BandPassFir(int n, double wl, double wh, WindowFunction window) : base(n)
		{
			FirCommon.CalcLinearBPFCoefficient(FirCommon.FirFilterType.BPF, this.coef, (wl - wh)/2, (wl + wh)/2, window);
		}
	}//class BandPassFir

	public class Delay : IFilter
	{
		CircularBuffer buf;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="taps">遅延タップ数</param>
		public Delay(int taps)
		{
			this.buf = new CircularBuffer(taps);
		}

		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			double tmp = this.buf.Top;
			this.buf.PushBack(x);
			return tmp;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buf.Length; ++i)
			{
				this.buf[i] = 0;
			}
		}

		public int Taps{get{return this.buf.Length;}}
	}//class Delay
}//namespace Filter
