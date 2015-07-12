using System;

namespace SoundLibrary.Filter.Equalizer
{
	using Function = SoundLibrary.Mathematics.Function.Function;
	using Cached = SoundLibrary.Mathematics.Function.CachedFunction;

	/// <summary>
	/// パライコの IIR フィルタ設計クラス。
	/// </summary>
	/// <remarks>
	/// フィルタの零/極計算。
	/// 零/極 → アナログプロトタイプフィルタ係数計算。
	/// AP フィルタ →[双一次変換]→ ディジタルフィルタ化。
	/// ディジタルフィルタ係数を ParametricEqualizer クラスの形式に変換。
	/// 
	/// 零/極                 : AP フィルタの零/極。共役複素数根、実根×2、もしくは実根×1。
	/// AP フィルタ係数       : a[3], b[3]。∑b[i]s^i / ∑a[i]s^i。
	/// ディジタルフィルタ係数: a[3], b[3]。∑b[i]z^-i / ∑a[i]z^-i。
	/// パライコクラスの係数  : a[2], b[2], c。c * (1 + ∑b[i]z^-i) / (1 - ∑a[i]z^-i)。
	/// </remarks>
	public abstract class FilterDesigner
	{
		#region フィールド

		protected int order;

		#endregion
		#region 初期化

		protected FilterDesigner() : this(0) {}
		protected FilterDesigner(int order){this.order = order;}

		#endregion
		#region プロパティ

		/// <summary>
		/// 次数。
		/// </summary>
		public int Order
		{
			get{return this.order;}
			set{this.order = value;}
		}

		/// <summary>
		/// 零/極ペアの数。
		/// </summary>
		public virtual int Length
		{
			get{return (this.order + 1) / 2;}
		}

		#endregion
		#region 抽象メソッド

		/// <summary>
		/// フィルタの零点/極を計算。
		/// </summary>
		/// <param name="roots">零点/極一覧の格納先</param>
		public abstract void GetZeroPole(ZeroPole[] roots);

		#endregion
		#region 零点/極の計算

		/// <summary>
		/// フィルタの零点/極を計算。
		/// 結果格納用の配列を関数内で確保。
		/// </summary>
		/// <returns>フィルタの零点/極一覧</returns>
		public virtual ZeroPole[] GetZeroPole()
		{
			ZeroPole[] roots = new ZeroPole[this.Length];
			for(int i=0; i<roots.Length; ++i) roots[i] = new ZeroPole();

			this.GetZeroPole(roots);
			return roots;
		}

		#endregion
		#region アナログプロトタイプフィルタ係数設計

		/// <summary>
		/// アナログプロトタイプフィルタの係数を計算。
		/// </summary>
		/// <returns>AP フィルタ係数</returns>
		public Coefficient[] GetAnalogPrototype()
		{
			Coefficient[] coefs = new Coefficient[this.Length];
			for(int i=0; i<coefs.Length; ++i) coefs[i] = new Coefficient();

			this.GetAnalogPrototype(coefs);

			return coefs;
		}

		/// <summary>
		/// アナログプロトタイプフィルタの係数を計算。
		/// </summary>
		/// <param name="coefs">計算結果の格納先</param>
		public virtual void GetAnalogPrototype(Coefficient[] coefs)
		{
			ZeroPole[] roots = this.GetZeroPole();
			ZeroPoleToAnalogPrototype(roots, coefs);
		}

		#endregion
		#region ディジタルフィルタ係数設計

		/// <summary>
		/// ディジタル LPF 係数を計算。
		/// </summary>
		/// <param name="w">カットオフ周波数</param>
		/// <returns>ディジタル LPF 係数</returns>
		public Coefficient[] GetDigitalLPF(double w)
		{
			Coefficient[] coefs = new Coefficient[this.Length];
			for(int i=0; i<coefs.Length; ++i) coefs[i] = new Coefficient();

			this.GetDigitalLPF(w, coefs);

			return coefs;
		}

		/// <summary>
		/// ディジタル LPF 係数を計算。
		/// </summary>
		/// <param name="w">カットオフ周波数</param>
		/// <param name="coefs">計算結果の格納先</param>
		public virtual void GetDigitalLPF(double w, Coefficient[] coefs)
		{
			this.GetAnalogPrototype(coefs);
			BilinearTransform(coefs, coefs, w);
		}

		#endregion
		#region PEQ 係数

		/// <summary>
		/// PEQ 係数(LPF)を計算。
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		public virtual ParametricEqualizer.Parameter[] GetLPF(double w)
		{
			ParametricEqualizer.Parameter[] peq = new ParametricEqualizer.Parameter[this.Length];
			for(int i=0; i<peq.Length; ++i) peq[i] = new ParametricEqualizer.Parameter();

			ToPeqCoefficient(this.GetDigitalLPF(w), peq);
			return peq;
		}

		#endregion
		#region 変換
		#region 零点/極配置→アナログプロトタイプフィルタ係数

		public static void RootToAnalogPrototype(Root root, double[] c)
		{
			switch(root.type)
			{
				case Root.Type.Complex:
					c[2] = 1;
					c[1] = -2 * root.a;
					c[0] = root.a * root.a + root.b * root.b;
					break;
				case Root.Type.Real:
					c[2] = 1;
					c[1] = -(root.a + root.b);
					c[0] = root.a * root.b;
					break;
				case Root.Type.None:
					c[2] = 0;
					c[1] = 0;
					c[0] = 1;
					break;
				default:
					c[2] = 0;
					c[1] = 1;
					c[0] = -root.a;
          break;
			}
		}

		public static void ZeroPoleToAnalogPrototype(ZeroPole zeropole, Coefficient coef)
		{
			RootToAnalogPrototype(zeropole.zero, coef.b);
			RootToAnalogPrototype(zeropole.pole, coef.a);
		}

		public static void ZeroPoleToAnalogPrototype(ZeroPole[] roots, Coefficient[] coefs)
		{
			for(int i=0; i<roots.Length; ++i)
			{
				ZeroPoleToAnalogPrototype(roots[i], coefs[i]);
			}
		}

		#endregion
		#region 双一次変換

		/// <summary>
		/// 1次の伝達関数を双1時変換する。
		/// </summary>
		/// <remarks>
		/// (b0 + b1 s)/(a0 + a1 s) を、双1次変換した結果を
		/// (b0' + b1' z^-1)/(a0' + a1' z^-1)とするとき、
		/// a0, a1 → a0', a1' を求める。
		/// (b に関しても同様の手順で変換可能。)
		/// </remarks>
		/// <param name="a0">a0(a0'の値に上書きされる)</param>
		/// <param name="a1">a1(a1'の値に上書きされる)</param>
		/// <param name="sin">sin ωs</param>
		/// <param name="cos">cos ωs</param>
		public static void BilinearTransform(
			double a0, double a1,
			out double d0, out double d1,
			double sin, double cos)
		{
			a0 = a0 * sin;
			a1 = a1 * (1 + cos);

			d0 = a0 + a1;
			d1 = a0 - a1;
		}

		/// <summary>
		/// 2次の伝達関数を双1時変換する。
		/// </summary>
		/// <remarks>
		/// (b0 + b1 s + b2 s^2)/(a0 + a1 s + a2 s^2) を、双1次変換した結果を
		/// (b0' + b1' z^-1 + b2' z^-2)/(a0' + a1' z^-1 + a2' z^-2)とするとき、
		/// a0, a1, a2 → a0', a1', a2' を求める。
		/// (b に関しても同様の手順で変換可能。)
		/// </remarks>
		/// <param name="a0">a0(a0'の値に上書きされる)</param>
		/// <param name="a1">a1(a1'の値に上書きされる)</param>
		/// <param name="a2">a2(a2'の値に上書きされる)</param>
		/// <param name="sin">sin ωs</param>
		/// <param name="cos">cos ωs</param>
		public static void BilinearTransform(
			double a0, double a1, double a2,
			out double d0, out double d1, out double d2,
			double sin, double cos)
		{
			a0 = a0 * (1 - cos);
			a1 = a1 * sin;
			a2 = a2 * (1 + cos);

			d0 = d2 = a0 + a2;
			d1 = 2 * (a0 - a2);

			d0 += a1;
			d2 -= a1;
		}

		public static void BilinearTransform(Coefficient ap, Coefficient digital, double sin, double cos)
		{
			if(ap.a[2] != 0 || ap.b[2] != 0)
			{
				BilinearTransform(
					ap.a[0], ap.a[1], ap.a[2],
					out digital.a[0], out digital.a[1], out digital.a[2],
					sin, cos);
				BilinearTransform(
					ap.b[0], ap.b[1], ap.b[2],
					out digital.b[0], out digital.b[1], out digital.b[2],
					sin, cos);
				return;
			}

			digital.a[2] = 0;
			digital.b[2] = 0;

			if(ap.a[1] != 0 || ap.b[1] != 0)
			{
				BilinearTransform(
					ap.a[0], ap.a[1],
					out digital.a[0], out digital.a[1],
					sin, cos);
				BilinearTransform(
					ap.b[0], ap.b[1],
					out digital.b[0], out digital.b[1],
					sin, cos);
				return;
			}

			digital.a[1] = 0;
			digital.b[1] = 0;

			digital.a[0] = ap.a[0];
			digital.b[0] = ap.b[0];
		}

		public static void BilinearTransform(Coefficient[] ap, Coefficient[] digital, double w)
		{
			double sin, cos;
			GetSinCos(w, out sin, out cos);

			for(int i=0; i<ap.Length; ++i)
			{
				BilinearTransform(ap[i], digital[i], sin, cos);
			}
		}

		protected static void GetSinCos(double w, out double sin, out double cos)
		{
			cos = Math.Cos(w);
			sin = Math.Sqrt(1 - cos * cos); //Math.Sin(w);
		}

		#endregion
		#region ディジタルフィルタ係数→PEQの係数

		public static void ToPeqCoefficient(Coefficient digital, ParametricEqualizer.Parameter peq)
		{
			double[] a = digital.a;
			double[] b = digital.b;

			peq.c  =  b[0] / a[0];
			peq.a1 = -a[1] / a[0];
			peq.a2 = -a[2] / a[0];
			peq.b1 =  b[1] / b[0];
			peq.b2 =  b[2] / b[0];
		}

		public static void ToPeqCoefficient(Coefficient[] digital, ParametricEqualizer.Parameter[] peq)
		{
			for(int i=0; i<digital.Length; ++i)
			{
				ToPeqCoefficient(digital[i], peq[i]);
			}
		}

		#endregion
		#endregion
		#region Function クラスとの連携

		/// <summary>
		/// z^-1 = exp(-jω)
		/// </summary>
		/// <param name="w">周波数ω</param>
		/// <returns>z^-1</returns>
		public static Function ZInv(Function w)
		{
			return new Cached(
				Function.Exp(-Function.I(w))
				);
		}

		/// <summary>
		/// 双1次変換
		/// s = 1/tan(ωs/2) * (z^-1 + 1) / (z^-1 - 1)
		///   = j tan(ω/2) / tan(ωs/2)
		/// </summary>
		/// <param name="w">ω</param>
		/// <param name="ws">ωs</param>
		/// <returns>ディジタル s</returns>
		public static Function DigitalS(Function w, Function ws)
		{
			return new Cached(
				Function.I(
				Function.Tan(w / 2) / Function.Tan(ws / 2)
				));
		}

		/// <summary>
		/// 双1次変換
		/// s = 1/tan(ωs/2) * (z^-1 + 1) / (z^-1 - 1)
		///   = j tan(ω/2) / tan(ωs/2)
		/// </summary>
		/// <param name="w">ω</param>
		/// <param name="ws">ωs</param>
		/// <returns>ディジタル s</returns>
		public static Function DigitalS(Function w, double ws)
		{
			return new Cached(
				Function.I(
				Function.Tan(w / 2) / Math.Tan(ws / 2)
				));
		}

		/// <summary>
		/// 双1次変換(ωs＝π/2)
		/// s = (z^-1 + 1) / (z^-1 - 1)
		///   = j tan(ω/2)
		/// </summary>
		/// <param name="w">ω</param>
		/// <returns>ディジタル s</returns>
		public static Function DigitalS(Function w)
		{
			return new Cached(
				Function.I(
				Function.Tan(w / 2)
				));
		}

		/// <summary>
		/// アナログ伝達関数を取得。
		/// </summary>
		/// <param name="s">s 領域変数</param>
		/// <returns>伝達関数</returns>
		public Function GetTransferFunction(Function s)
		{
			Coefficient[] coefs = this.GetAnalogPrototype();
			Function f = (Function)1;

			foreach(Coefficient coef in coefs)
			{
				f *=
					(coef.b[0] + coef.b[1] * s + coef.b[2] * s * s) /
					(coef.a[0] + coef.a[1] * s + coef.a[2] * s * s);
			}

			return f;
		}

		/// <summary>
		/// ディジタル伝達関数の周波数特性を取得。
		/// </summary>
		/// <param name="w">周波数変数ω(正規化角周波数)</param>
		/// <param name="ws">カットオフ周波数ωs(正規化角周波数)</param>
		/// <returns>周波数特性</returns>
		public Function GetTransferFunction(Function w, Function ws)
		{
			return this.GetTransferFunction(DigitalS(w, ws));
		}

		#endregion
	}
}
