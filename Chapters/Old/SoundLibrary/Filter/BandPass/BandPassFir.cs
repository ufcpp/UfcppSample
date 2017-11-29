using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// 窓関数用のデリゲート。
	/// </summary>
	public delegate double WindowFunction(double i);

	/// <summary>
	/// 窓関数用のインターフェース。
	/// </summary>
	public interface IWindow
	{
		/// <summary>
		/// 窓関数の値を取得。
		/// </summary>
		/// <param name="i">i サンプル目の値を取得する</param>
		/// <returns>窓関数の値</returns>
		double Get(double i);

		/// <summary>
		/// 窓関数の次数を設定する。
		/// </summary>
		int Order{get; set;}
	}

	/// <summary>
	/// 線形位相ローパス/バンドパス/ハイパスフィルタのフィルタタイプ。
	/// </summary>
	public enum FirFilterType
	{
		LPF, BPF, HPF
	}

	/// <summary>
	/// 窓関数タイプ。
	/// </summary>
	public enum WindowType
	{
		Rectangular, // 矩形窓
		Hamming,     // ハミング窓
		Hanning,     // ハニング窓
		Blackman,    // ブラックマン窓
		Keiser,      // カイザー窓
	}

	/// <summary>
	/// FIR フィルタ関係の共通関数群
	/// </summary>
	public class FirCommon
	{
		public static IFirFilter GetLowPassFilter(int taps, double w, WindowType type)
		{
			return CalcLinearBPFCoefficient(FirFilterType.LPF, w, 0, taps, type);
		}

		public static IFirFilter GetBandPassFilter(int taps, double wl, double wh, WindowType type)
		{
			return CalcLinearBPFCoefficient(FirFilterType.BPF, (wl - wh)/2, (wl + wh)/2, taps, type);
		}

		public static IFirFilter GetHighPassFilter(int taps, double w, WindowType type)
		{
			return CalcLinearBPFCoefficient(FirFilterType.HPF, Math.PI - w, 0, taps, type);
		}

		public static IFirFilter CalcLinearBPFCoefficient(FirFilterType type, double w, double w0, int taps, WindowType window)
		{
			double[] coef;

			if(taps % 2 == 1)
			{
				coef = new double[(taps + 1) / 2];
				CalcOddLinearBPFCoefficient(type, coef, w, w0, GetWindow(window, taps));
				return new OddLinearFir(coef);
			}
			else
			{
				coef = new double[taps / 2];
				CalcEvenLinearBPFCoefficient(type, coef, w, w0, GetWindow(window, taps));
				return new EvenLinearFir(coef);
			}
		}

		/// <summary>
		/// 線形位相バンドパスフィルタ(奇数タップ)の係数を計算する。
		/// 右側半分のみを計算。
		/// </summary>
		/// <param name="type">フィルタタイプ</param>
		/// <param name="coef">係数の格納先</param>
		/// <param name="w">遮断周波数(BPF の場合は遮断帯域幅、HPF の場合は π-遮断周波数)</param>
		/// <param name="w0">(BPF のみ) 中心周波数</param>
		/// <param name="window">窓関数</param>
		public static void CalcOddLinearBPFCoefficient(FirFilterType type, double[] coef, double w, double w0, WindowFunction window)
		{
			int n = coef.Length - 1;
			double sum;

			sum = coef[n] = window(0) * w;
			for(int i=1; i<=n; ++i)
			{
				double tmp = window(i) * Math.Sin(w * i) / i;
				sum += tmp * 2;

				if(type == FirFilterType.LPF)
				{
					coef[n - i] = tmp;
				}
				else if(type == FirFilterType.HPF)
				{
					if(i%2 != 0)
						coef[n - i] = -tmp;
					else
						coef[n - i] = tmp;
				}
				else
				{
					coef[n - i] = 2 * Math.Cos(w0 * i) * tmp;
				}
			}

			for(int i=0; i<=n; ++i)
			{
				coef[i] /= sum;
			}
		}//GetOddLinearBPFCoefficient


		/// <summary>
		/// 線形位相バンドパスフィルタ(奇数タップ)の係数を計算する。
		/// 右側半分のみを計算。
		/// </summary>
		/// <param name="type">フィルタタイプ</param>
		/// <param name="coef">係数の格納先</param>
		/// <param name="w">遮断周波数(BPF の場合は遮断帯域幅、HPF の場合は π-遮断周波数)</param>
		/// <param name="w0">(BPF のみ) 中心周波数</param>
		/// <param name="window">窓関数</param>
		public static void CalcEvenLinearBPFCoefficient(FirFilterType type, double[] coef, double w, double w0, WindowFunction window)
		{
			int n = coef.Length - 1;
			double sum;

			sum = 0;
			for(int i=0; i<=n; ++i)
			{
				double x = i + 0.5;

				double tmp = window(x) * Math.Sin(w * x) / x;
				sum += tmp * 2;

				if(type == FirFilterType.LPF)
				{
					coef[n - i] = tmp;
				}
				else if(type == FirFilterType.HPF)
				{
					if(i%2 != 0)
						coef[n - i] = -tmp;
					else
						coef[n - i] = tmp;
				}
				else
				{
					coef[n - i] = 2 * Math.Cos(w0 * x) * tmp;
				}
			}

			for(int i=0; i<n; ++i)
			{
				coef[i] /= sum;
			}
		}//GetEvenLinearBPFCoefficient

		/// <summary>
		/// 窓関数用。常に1を返す関数。
		/// </summary>
		public static double Constant1(double i)
		{
			return 1;
		}

		public static WindowFunction GetWindow(WindowType type, int order)
		{
			switch(type)
			{
				case WindowType.Hamming:
					return new WindowFunction(new Window.Hamming(order).Get);
				case WindowType.Hanning:
					return new WindowFunction(new Window.Hanning(order).Get);
				case WindowType.Blackman:
					return new WindowFunction(new Window.Blackman(order).Get);
				case WindowType.Keiser:
					return new WindowFunction(new Window.Keiser(order, 20).Get);
				default:
					return new WindowFunction(Constant1);
			}
		}
	}//class FirCommon

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
		public LowPassFir(int n, double w, IWindow window) : this(n, w, new WindowFunction(window.Get)){}

		/// <summary>
		/// ローパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public LowPassFir(int n, double w, WindowFunction window) : base(n)
		{
			this.SetParameter(w, window);
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public void SetParameter(double w, WindowFunction window)
		{
			FirCommon.CalcOddLinearBPFCoefficient(FirFilterType.LPF, this.coef, w, 0, window);
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
		public HighPassFir(int n, double w, IWindow window) : this(n, w, new WindowFunction(window.Get)){}
		
		/// <summary>
		/// ハイパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public HighPassFir(int n, double w, WindowFunction window) : base(n)
		{
			this.SetParameter(w, window);
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="w">遮断周波数</param>
		/// <param name="window">窓関数</param>
		public void SetParameter(double w, WindowFunction window)
		{
			FirCommon.CalcOddLinearBPFCoefficient(FirFilterType.HPF, this.coef, Math.PI - w, 0, window);
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
		public BandPassFir(int n, double wl, double wh, IWindow window) : this(n, wl, wh, new WindowFunction(window.Get)){}

		/// <summary>
		/// バンドパスフィルタを作成
		/// </summary>
		/// <param name="n">タップ数＝2n+1</param>
		/// <param name="wl">下限周波数</param>
		/// <param name="wh">上限周波数</param>
		/// <param name="window">窓関数</param>
		public BandPassFir(int n, double wl, double wh, WindowFunction window) : base(n)
		{
			this.SetParameter(wl, wh, window);
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="wl">下限周波数</param>
		/// <param name="wh">上限周波数</param>
		/// <param name="window">窓関数</param>
		public void SetParameter(double wl, double wh, WindowFunction window)
		{
			FirCommon.CalcOddLinearBPFCoefficient(FirFilterType.BPF, this.coef, (wl - wh)/2, (wl + wh)/2, window);
		}
	}//class BandPassFir

	namespace Window
	{
		/// <summary>
		/// ハニング窓
		/// </summary>
		public class Hanning : IWindow
		{
			int order;
			public Hanning(int order){this.order = order;}

			public double Get(double i)
			{
				return 0.5 + 0.5 * Math.Cos(2 * Math.PI * i / (double)this.order) ;
			}

			public int Order
			{
				get{return this.order;}
				set{this.order = value;}
			}
		}

		/// <summary>
		/// ハミング窓
		/// </summary>
		public class Hamming : IWindow
		{
			int order;
			public Hamming(int order){this.order = order;}

			public double Get(double i)
			{
				return 0.54 + 0.46 * Math.Cos(2 * Math.PI * i / (double)this.order) ;
			}

			public int Order
			{
				get{return this.order;}
				set{this.order = value;}
			}
		}

		/// <summary>
		/// ブラックマン窓
		/// </summary>
		public class Blackman : IWindow
		{
			int order;
			public Blackman(int order){this.order = order;}

			public double Get(double i)
			{
				return 0.42 + 0.5 * Math.Cos(2 * Math.PI * i / (double)this.order) 
					+ 0.08 * Math.Cos(4 * Math.PI * i / (double)this.order);
			}

			public int Order
			{
				get{return this.order;}
				set{this.order = value;}
			}
		}

		public class Keiser : IWindow
		{
			/// <summary>
			/// 0次第1種変形ベッセル関数
			/// </summary>
			static double I0(double x)
			{
				double sum = 1.0;
				double xj = 1.0;

				for(int j=1; j<20 ; j++)
				{
					xj = 0.5*xj*x/j;
					sum = sum + Pow2(xj);
				}
				return sum;
			}

			static double Pow2(double x){return x * x;}

			int order2; // 次数/2
			double alpha;

			public Keiser(int order, double attenuate)
			{
				this.order2 = order / 2;

				if(attenuate >= 50.0)
					this.alpha = 0.1102 * (attenuate - 8.7);
				else if(attenuate > 21.0)
					this.alpha = 0.5842 * Math.Pow(attenuate - 21.0 , 0.4)
						+ 0.07886 * (attenuate - 21.0);
				else
					this.alpha = 0.0;  //方形窓となる
			}

			public double Get(double i)
			{
				double fm = this.alpha * Math.Sqrt(1.0-Pow2((double)i / this.order2));
				return I0(fm) / I0(this.alpha);
			}

			public int Order
			{
				get{return this.order2 * 2;}
				set{this.order2 = value / 2;}
			}
		}

		/// <summary>
		/// 矩形窓
		/// </summary>
		public class Rectangular : IWindow
		{
			public double Get(double i)
			{
				return 1;
			}

			public int Order
			{
				get{return 0;}
				set{}
			}
		}
	}//namespace Window
}//namespace Filter
