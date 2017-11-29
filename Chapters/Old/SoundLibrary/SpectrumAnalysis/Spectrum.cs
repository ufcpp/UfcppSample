using System;
using SoundLibrary.Mathematics;

namespace SoundLibrary.SpectrumAnalysis
{
	/// <summary>
	/// 周波数特性クラス。
	/// </summary>
	public class Spectrum : ICloneable
	{
		double[] x; // データ格納領域
		Fft fft;

		#region コンストラクタ・構築用メソッド

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public Spectrum() : this(null){}

		/// <summary>
		/// 時系列長のみを指定して構築。
		/// </summary>
		/// <param name="length">時系列長</param>
		public Spectrum(int length) : this(new double[length]){}

		/// <summary>
		/// 周波数領域データ(時系列データを Fft クラスで変換したもの)から作成。
		/// </summary>
		/// <param name="x">周波数領域データ</param>
		public Spectrum(double[] x)
		{
			this.x = x;
			this.fft = new Fft(x.Length);
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="X">時間領域データ</param>
		/// <returns>作成された周波数特性</returns>
		public static Spectrum FromTimeSequence(double[] x)
		{
			if(x == null) return null;
			return FromTimeSequence(x, x.Length, 0);
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="x">時間領域データ</param>
		/// <param name="length">データの長さ</param>
		/// <returns>作成された周波数特性</returns>
		public static Spectrum FromTimeSequence(double[] x, int length)
		{
			return FromTimeSequence(x, length, 0);
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="x">時間領域データ</param>
		/// <param name="length">データの長さ</param>
		/// <param name="skip">読み飛ばす長さ</param>
		/// <returns>作成された周波数特性</returns>
		public static Spectrum FromTimeSequence(double[] x, int length, int skip)
		{
			if(x == null) return null;

			int len = BitOperation.FloorPower2(length);

			double[] tmp = new double[len];
			for(int i=0; i<len; ++i) tmp[i] = x[skip + i];
			Fft fft = new Fft(len);
			fft.Transform(tmp);
			return new Spectrum(tmp);
		}

		/// <summary>
		/// 遅延 δ[i - delay] の周波数特性を作成する。
		/// </summary>
		/// <param name="delay">遅延サンプル数</param>
		/// <returns>作成された周波数特性</returns>
		public static Spectrum FromDelay(double delay, int length)
		{
			Spectrum s = new Spectrum(length);
			length /= 2;
			s.x[0] = 1;
			s.x[1] = Math.Cos(-Math.PI * delay);
			double dt = -Math.PI * delay / length;
			double t = dt;
			for(int i=1; i<length; ++i, t += dt)
			{
				s[i] = Complex.FromArg(t);
			}

			return s;
		}

		#endregion
		#region 値の取得・設定

		/// <summary>
		/// 周波数領域のデータ数。
		/// </summary>
		public int Count
		{
			get{return x.Length/2;}
		}

		/// <summary>
		/// i 番目のデータを複素数値で返す。
		/// </summary>
		public Complex this[int i]
		{
			set
			{
				if(i == 0) this.x[0] = value.Re;
				else if(i == x.Length/2) this.x[1] = value.Re;
				else
				{
					this.x[2*i]   = value.Re;
					this.x[2*i+1] = -value.Im;
				}
			}
			get
			{
				if(i == 0) return this.x[0];
				else if(i == x.Length/2) return this.x[1];
				return new Complex(this.x[2*i], -this.x[2*i+1]);
			}
		}

		/// <summary>
		/// 時系列を返す。
		/// </summary>
		public double[] TimeSequence
		{
			get
			{
				double[] x = (double[])this.x.Clone();
				this.fft.Invert(x);
				for(int i=0; i<x.Length; ++i) x[i] *= 2.0/x.Length;
				return x;
			}
		}

		/// <summary>
		/// 時系列を返す。
		/// あらかじめ時系列データの格納先を用意しておく。
		/// </summary>
		/// <param name="x">データ格納先</param>
		public void GetTimeSequence(double[] x)
		{
			double a = 2.0/x.Length;
			for(int i=0; i<x.Length; ++i) x[i] = a * this.x[i];
			this.fft.Invert(x);
		}

		/// <summary>
		/// 時間領域のデータ数。
		/// </summary>
		public int TimeLength
		{
			get{return x.Length;}
		}

		#endregion
		#region 演算子・変換メソッド

		/// <summary>
		/// 単項＋。元のままの値を返す。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>+a</returns>
		public static Spectrum operator+ (Spectrum a)
		{
			return a.Clone();
		}

		/// <summary>
		/// 単項－。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>-a</returns>
		public static Spectrum operator- (Spectrum a)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] = -a[i];
			return c;
		}

		/// <summary>
		/// スペクトル同士の和 A(ω) + B(ω)。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>和</returns>
		public static Spectrum operator+ (Spectrum a, Spectrum b)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] += b[i];
			return c;
		}

		/// <summary>
		/// スペクトル同士の差 A(ω) - B(ω)。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>差</returns>
		public static Spectrum operator- (Spectrum a, Spectrum b)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] -= b[i];
			return c;
		}

		/// <summary>
		/// スペクトル同士の積 A(ω) * B(ω)。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>積</returns>
		public static Spectrum operator* (Spectrum a, Spectrum b)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] *= b[i];
			return c;
		}

		/// <summary>
		/// スペクトル×実数の積 A(ω) * x。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="x">右オペランド</param>
		/// <returns>積</returns>
		public static Spectrum operator* (Spectrum a, double x)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] *= x;
			return c;
		}

		/// <summary>
		/// スペクトル×実数の積 x * A(ω)。
		/// </summary>
		/// <param name="x">左オペランド</param>
		/// <param name="a">右オペランド</param>
		/// <returns>積</returns>
		public static Spectrum operator* (double x, Spectrum a)
		{
			return a * x;
		}

		/// <summary>
		/// スペクトル同士の商 A(ω) / B(ω)。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>商</returns>
		public static Spectrum operator/ (Spectrum a, Spectrum b)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] /= b[i];
			return c;
		}

		public Spectrum Invert()
		{
			Spectrum c = this.Clone();
			for(int i=0; i<=c.Count; ++i)
				c[i] = this[i].Invert();
			return c;
		}

		#endregion
		#region 実部・虚部

		/// <summary>
		/// 実部を配列化して取得。
		/// </summary>
		/// <returns></returns>
		public double[] GetRe()
		{
			double[] tmp = new double[this.x.Length / 2];
			for(int i=0; i<tmp.Length; ++i) tmp[i] = this[i].Re;
			return tmp;
		}

		/// <summary>
		/// 虚部を配列化して取得。
		/// </summary>
		/// <returns></returns>
		public double[] GetIm()
		{
			double[] tmp = new double[this.x.Length / 2];
			for(int i=0; i<tmp.Length; ++i) tmp[i] = this[i].Im;
			return tmp;
		}

		#endregion
		#region 振幅特性

		/// <summary>
		/// パワースペクトル[dB]を取得。
		/// </summary>
		public double[] GetPower()
		{
			double[] tmp = new double[this.x.Length / 2];
			for(int i=0; i<tmp.Length; ++i) tmp[i] = this[i].Power;
			return tmp;
		}

		/// <summary>
		/// 振幅特性を取得。
		/// </summary>
		public double[] GetAmplitude()
		{
			double[] tmp = new double[this.x.Length / 2];
			for(int i=0; i<tmp.Length; ++i) tmp[i] = this[i].Abs;
			return tmp;
		}

		#endregion
		#region 位相特性

		/// <summary>
		/// 位相特性を取得。
		/// </summary>
		public double[] GetPhase()
		{
			double[] tmp = new double[this.x.Length / 2];
			tmp[0] = 0;
			for(int i=1; i<tmp.Length; ++i) tmp[i] = this[i].Arg;
			return tmp;
		}

		/// <summary>
		/// 位相特性(アンラップしたもの)を取得。
		/// </summary>
		public double[] GetUnwrapPhase()
		{
			double[] tmp = this.GetPhase();
			Spectrum.Unwrap(tmp);
			return tmp;
		}

		/// <summary>
		/// 位相特性(アンラップしたもの)を取得。
		/// </summary>
		/// <param name="skip">最初 skip サンプルはアンラップしない</param>
		public double[] GetUnwrapPhase(int skip)
		{
			double[] tmp = this.GetPhase();
			Spectrum.Unwrap(tmp, skip);
			return tmp;
		}

		/// <summary>
		/// 位相のアンラッピングを行う。
		/// </summary>
		/// <param name="phase">位相の入った配列</param>
		public static void Unwrap(double[] phase)
		{
			double tmp = 0;
			double prev = phase[0];

			for(int i=1; i<phase.Length; ++i)
			{
				double dif = phase[i] - prev;
				prev = phase[i];
				if(dif < -Math.PI) tmp += 2 * Math.PI;
				if(dif >  Math.PI) tmp -= 2 * Math.PI;
				phase[i] += tmp;
			}
		}

		/// <summary>
		/// 位相のアンラッピングを行う。
		/// </summary>
		/// <param name="phase">位相の入った配列</param>
		/// <param name="start">アンラッピングの開始地点</param>
		public static void Unwrap(double[] phase, int start)
		{
			double tmp = 0;
			double prev = phase[start];

			for(int i=start+1; i<phase.Length; ++i)
			{
				double dif = phase[i] - prev;
				prev = phase[i];
				if(dif < -Math.PI) tmp += 2 * Math.PI;
				if(dif >  Math.PI) tmp -= 2 * Math.PI;
				phase[i] += tmp;
			}
		}

		#endregion
		#region 最小位相・オールパス位相

		/// <summary>
		/// 最小位相を求める。
		/// </summary>
		public double[] GetMinimumPhase()
		{
			int N = this.x.Length / 2;
			double[] tmp = new double[2*N];
			for(int i=0; i<N; ++i)
			{
				tmp[2*i]         = Math.Log(this[i].LinearPower);
				tmp[2*i+1]       = 0;
			}

			CFft cfft = new CFft(2*N);
			cfft.Invert(tmp);

			tmp[0] /= 2; tmp[1] /= 2;
			tmp[N] /= 2; tmp[N+1] /= 2;
			for(int i=N+1; i<2*N; ++i)
				tmp[i] = 0;

			cfft.Transform(tmp);

			double[] y = new double[N];
			for(int i=0; i<N; ++i)
			{
				y[i] = -tmp[2*i+1] / N;
			}

			return y;
		}//GetMinimumPhase

		/// <summary>
		/// オールパス位相を求める。
		/// </summary>
		public double[] GetAllpassPhase()
		{
			double[] p  = GetPhase();
			Spectrum.Unwrap(p);
			double[] mp = GetMinimumPhase();
			int n = p.Length;

			for(int i=0; i<n; ++i) p[i] -= mp[i];

			return p;
		}//GetAllpassPhase

		#endregion
		#region 位相遅延・群遅延特性

		/// <summary>
		/// 位相遅延特性を取得。
		/// </summary>
		public static double[] GetPhaseDelay(double[] phase, double fs)
		{
			double df = fs / phase.Length;
			double dw = 2 * Math.PI * df;
			double[] tmp = new Double[phase.Length];

			tmp[0] = tmp[1] = -phase[1] / dw;
			for(int i=2; i<tmp.Length; ++i) tmp[i] = -phase[i] / (dw * i);
			return tmp;
		}

		public double[] GetPhaseDelay()
		{
			double[] phase = this.GetUnwrapPhase(this.Count / 100);

			double dw = 2 * Math.PI / phase.Length;
			double[] tmp = new Double[phase.Length];

			tmp[0] = tmp[1] = -phase[1] / dw;
			for(int i=2; i<tmp.Length; ++i) tmp[i] = -phase[i] / (dw * i);
			return tmp;
		}

		/// <summary>
		/// 群遅延特性を取得。
		/// F(ω) = x + j y, θ=∠F,
		/// gd = -dθ/dω = -(d/dω)Im[log F] = -Im[(d/dω)log F]
		/// = -Im[F'/F] = -Im[(x'+jy') / (x+jy) = (x'y - xy') / (x*x + y*y)
		/// </summary>
		public double[] GetGroupDelay()
		{
			double[] re = this.GetRe();
			double[] im = this.GetIm();
			double[] dre = SoundLibrary.Mathematics.Discrete.Differential.Derive(re);
			double[] dim = SoundLibrary.Mathematics.Discrete.Differential.Derive(im);
			double[] tmp = new Double[re.Length];
			double c = tmp.Length / Math.PI;

			for(int i=0; i<tmp.Length; ++i)
			{
				double d = re[i] * re[i] + im[i] * im[i];
				tmp[i] = im[i] * dre[i] - re[i] * dim[i];
				tmp[i] *= c / d;
			}
			return tmp;
		}

		/// <summary>
		/// 群遅延特性を取得。
		/// </summary>
		[System.Obsolete("GetGroupDelay の妥当性が確かめられ次第削除します")]
		public double[] GetGroupDelay0()
		{
			double[] tmp = this.GetUnwrapPhase(this.Count / 100);

			tmp = SoundLibrary.Mathematics.Discrete.Differential.Derive(tmp);
			double c = tmp.Length / Math.PI;

			for(int i=0; i<tmp.Length; ++i) tmp[i] *= -c;
			return tmp;
		}

		#endregion
		#region 遅延関係、非推奨メソッド(Obsolete にするなり、削除するなりしたい)

		/// <summary>
		/// 平均ディレイ(＝オールパス位相の平均勾配×-1)を求める。
		/// </summary>
		public double GetMeanDelay()
		{
			double[] p = this.GetUnwrapPhase(this.Count / 100);
			double[] mp = GetMinimumPhase();
			int n = p.Length;
			double delay = 0;

			for(int i=0; i<n; ++i) delay += p[i] - mp[i];
			delay *= -2.0 / (n * (n - 1));

			return delay;
		}

		/// <summary>
		/// 位相から平均ディレイを除いた部分を求める。
		/// </summary>
		/// <param name="delay">位相から平均ディレイを除いた部分</param>
		public double[] GetPhase0(double delay)
		{
			double[] p = this.GetUnwrapPhase(this.Count / 100);
			int n = p.Length;

			for(int i=0; i<n; ++i) p[i] += delay * i;

			return p;
		}

		/// <summary>
		/// 位相から平均ディレイを除いた部分を求める。
		/// </summary>
		public double[] GetPhase0()
		{
			double[] p = this.GetUnwrapPhase(this.Count / 100);
			double[] mp = GetMinimumPhase();
			int n = p.Length;
			double x = 0;

			for(int i=0; i<n; ++i) x += p[i] - mp[i];
			x *= -2.0 / (n * n);

			for(int i=0; i<n; ++i) p[i] += x * i;

			return p;
		}

		#endregion
		#region 形式の変換

		/// <summary>
		/// 振幅特性の平均値が0dBになるように正規化
		/// </summary>
		public void Normalize()
		{
			double level = 0;
			for(int i=0; i<this.Count; ++i) level += Math.Log(this[i].LinearPower);
			level /= 2 * this.Count;
			level = Math.Exp(-level);
			for(int i=0; i<this.Count; ++i) this[i] *= level;
		}

		/// <summary>
		/// 最小位相化する。
		/// </summary>
		public void ConvertToMinimumPhase()
		{
			double[] amp = this.GetAmplitude();
			double[] phase = this.GetMinimumPhase();
			int len = this.Count;

			for(int i=0; i<len; ++i)
			{
				this[i] = Complex.FromPolar(amp[i], phase[i]);
			}
		}

		/// <summary>
		/// 最小位相化した周波数特性を生成する。
		/// </summary>
		/// <returns>最小位相化した周波数特性</returns>
		public Spectrum GetMinimumPahsedSpectrum()
		{
			Spectrum s = this.Clone();
			s.ConvertToMinimumPhase();
			return s;
		}

		#endregion
		#region ICloneable メンバ

		/// <summary>
		/// インスタンスのコピーを作成。
		/// </summary>
		/// <returns></returns>
		public Spectrum Clone()
		{
			return new Spectrum((double[])this.x.Clone());
		}

		object System.ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
		#region static 関数

		/// <summary>
		/// x をヒルベルト変換する。
		/// </summary>
		/// <param name="x">変換元</param>
		/// <returns>変換後</returns>
		public static double[] HilbertTransform(double[] x)
		{
			int N = x.Length;
			double[] tmp = new double[2*N];
			for(int i=0; i<N; ++i)
			{
				tmp[2*i]         = x[i];
				tmp[2*i+1]       = 0;
			}

			CFft cfft = new CFft(2*N);
			cfft.Invert(tmp);

			tmp[0] /= 2; tmp[1] /= 2;
			tmp[N] /= 2; tmp[N+1] /= 2;
			for(int i=N+1; i<2*N; ++i)
				tmp[i] = 0;

			cfft.Transform(tmp);

			double[] y = new double[x.Length];
			for(int i=0; i<N; ++i)
			{
				x[i] = tmp[2*i]   * 2 / N;
				y[i] = tmp[2*i+1] * 2 / N;
			}

			return y;
		}//HilbertTransform

		/// <summary>
		/// データのスムージングを行う。
		/// </summary>
		/// <param name="data"></param>
		public static void Smooth(double[] data)
		{
			double sum = data[0] + data[1] + data[2];
			data[0] = sum / 3;
			sum += data[3];
			data[1] = sum / 4;
			sum += data[4];
			data[1] = sum / 5;
			int i=3;
			for(; i<data.Length-2; ++i)
			{
				sum -= data[i-3];
				sum += data[i+2];
				data[i] = sum / 5;
			}
			sum -= data[i-3];
			data[i] = sum / 4;
			++i;
			sum -= data[i-3];
			data[i] = sum / 3;
		}

		#endregion
	}//class Spectrum
}//namespace SpectrumAnalysis
