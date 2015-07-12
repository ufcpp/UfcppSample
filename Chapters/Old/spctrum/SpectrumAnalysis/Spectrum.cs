using System;

using Wave;

namespace SpectrumAnalysis
{
	/// <summary>
	/// 周波数特性クラス。
	/// </summary>
	public class Spectrum
	{
		double sampleRate; // サンプリングレート
		double[] x; // データ格納領域

		public Spectrum(){}

		/// <summary>
		/// 周波数領域データから作成
		/// </summary>
		/// <param name="x">周波数領域データ</param>
		/// <param name="sampleRate">サンプリングレート</param>
		public Spectrum(double[] x, double sampleRate)
		{
			this.x = x;
			this.sampleRate = sampleRate;
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="X">時間領域データ</param>
		/// <param name="sampleRate">サンプリングレート</param>
		/// <returns>作成された</returns>
		public static Spectrum FromTimeSequence(double[] x, double sampleRate)
		{
			if(x == null) return null;
			return FromTimeSequence(x.Length, x, sampleRate);
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="length">データの長さ</param>
		/// <param name="X">時間領域データ</param>
		/// <param name="sampleRate">サンプリングレート</param>
		/// <returns>作成された</returns>
		public static Spectrum FromTimeSequence(int length, double[] x, double sampleRate)
		{
			if(x == null) return null;

			int len = Power2(length);

			double[] tmp = new double[len];
			for(int i=0; i<len; ++i) tmp[i] = x[i];
			Fft fft = new Fft(len);
			fft.Transform(tmp);
			return new Spectrum(tmp, sampleRate);
		}

		/// <summary>
		/// 時間領域データから作成。
		/// </summary>
		/// <param name="skip">読み飛ばす長さ</param>
		/// <param name="length">データの長さ</param>
		/// <param name="X">時間領域データ</param>
		/// <param name="sampleRate">サンプリングレート</param>
		/// <returns>作成された</returns>
		public static Spectrum FromTimeSequence(int skip, int length, double[] x, double sampleRate)
		{
			if(x == null) return null;

			int len = Power2(length);

			double[] tmp = new double[len];
			for(int i=0; i<len; ++i) tmp[i] = x[skip + i];
			Fft fft = new Fft(len);
			fft.Transform(tmp);
			return new Spectrum(tmp, sampleRate);
		}

		/// <summary>
		/// インスタンスのコピーを作成。
		/// </summary>
		/// <returns></returns>
		public Spectrum Clone()
		{
			return new Spectrum((double[])this.x.Clone(), this.sampleRate);
		}

		/// <summary>
		/// len を超えない最大の2のべきを求める。
		/// </summary>
		/// <param name="len"></param>
		/// <returns></returns>
		public static int Power2(int len)
		{
			int n = 1;
			for(; len!=1; len/=2, n*=2);
			return n;
		}

		/// <summary>
		/// 周波数領域のデータ数。
		/// </summary>
		public int Count
		{
			get{return x.Length/2;}
		}

		/// <summary>
		/// 時間領域のデータ数。
		/// </summary>
		public int TimeLength
		{
			get{return x.Length;}
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
		/// 時系列を返す。
		/// </summary>
		public double[] TimeSequence
		{
			get
			{
				double[] x = (double[])this.x.Clone();
				Fft fft = new Fft(x.Length);
				fft.Invert(x);
				for(int i=0; i<x.Length; ++i) x[i] *= 2.0/x.Length;
				return x;
			}
		}

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

			CFft fft = new CFft(2*N);
			fft.Invert(tmp);

			tmp[0] /= 2; tmp[1] /= 2;
			tmp[N] /= 2; tmp[N+1] /= 2;
			for(int i=N+1; i<2*N; ++i)
				tmp[i] = 0;

			fft.Transform(tmp);

			double[] y = new double[x.Length];
			for(int i=0; i<N; ++i)
			{
				x[i] = tmp[2*i]   * 2 / N;
				y[i] = tmp[2*i+1] * 2 / N;
			}

			return y;
		}//HilbertTransform

		/// <summary>
		/// スペクトル同士の和 A(ω) + B(ω)。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>和</returns>
		public static Spectrum operator+ (Spectrum a, Spectrum b)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<a.Count; ++i)
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
			for(int i=0; i<a.Count; ++i)
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
			for(int i=0; i<a.Count; ++i)
				c[i] *= b[i];
			return c;
		}
		public static Spectrum operator* (Spectrum a, double x)
		{
			Spectrum c = a.Clone();
			for(int i=0; i<a.Count; ++i)
				c[i] *= x;
			return c;
		}
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
			for(int i=0; i<a.Count; ++i)
				c[i] /= b[i];
			return c;
		}

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

		/// <summary>
		/// 位相特性を取得。
		/// </summary>
		public double[] GetPhase()
		{
			double[] tmp = new double[this.x.Length / 2];
			for(int i=0; i<tmp.Length; ++i) tmp[i] = this[i].Arg;
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
		/// 位相特性を取得。
		/// </summary>
		public static double[] GetPhaseDalay(double[] phase, double fs)
		{
			double df = fs / phase.Length;
			double dw = 2 * Math.PI * df;
			double[] tmp = new Double[phase.Length];

			tmp[0] = tmp[1] = -phase[1] / dw;
			for(int i=2; i<tmp.Length; ++i) tmp[i] = -phase[i] / (dw * i);
			return tmp;
		}

		/// <summary>
		/// 群特性を取得。
		/// </summary>
		public static double[] GetGroupDalay(double[] phase, double fs)
		{
			double df = fs / phase.Length;
			double dw = 2 * Math.PI * df;
			double[] tmp = new Double[phase.Length];

			tmp[0] = tmp[1] = (phase[1] - phase[0]) / dw;
			int i=2;
			for(; i<tmp.Length-1; ++i) tmp[i] = (phase[i-1] - phase[i+1]) / (2 * dw);
			tmp[i] = (phase[i-1] - phase[i]) / dw;
			return tmp;
		}

		/// <summary>
		/// 最小位相を求める。
		/// </summary>
		/// <returns>変換後</returns>
		public double[] GetMinimumPhase()
		{
			int N = this.x.Length / 2;
			double[] tmp = new double[2*N];
			for(int i=0; i<N; ++i)
			{
				tmp[2*i]         = Math.Log(this[i].LinearPower);
				tmp[2*i+1]       = 0;
			}

			CFft fft = new CFft(2*N);
			fft.Invert(tmp);

			tmp[0] /= 2; tmp[1] /= 2;
			tmp[N] /= 2; tmp[N+1] /= 2;
			for(int i=N+1; i<2*N; ++i)
				tmp[i] = 0;

			fft.Transform(tmp);

			double[] y = new double[N];
			for(int i=0; i<N; ++i)
			{
				y[i] = tmp[2*i+1] / N;
			}

			return y;
		}//HilbertTransform
	}//class Spectrum
}
