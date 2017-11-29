#define GAIN
//#define PHASE
//#define OUTPUT_COEF

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using Wave;
using Filter;
using SpectrumAnalysis;

namespace AppMain
{
	/// <summary>
	/// Class1 の概要の説明です。
	/// </summary>
	public class AppTest
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void _Main(string[] args)
		{
			const int N = 1024;
			double[] x = new double[N];

			IFilter filter = new ShelvingEqualizer(Common.Normalize(1000), 1);

			// インパルス応答取得
			filter.Clear();
			/*
			filter.GetValue(1);
			for(int i=0; i<16; ++i)
				filter.GetValue(0);
				*/
			filter.GetValue(1);
			for(int i=1; i<N; ++i)
				x[i] = filter.GetValue(0);

#if TIME_SEQUENCE
			int n = N;
			double[] t = new double[n];
			for(int i=0; i<n; ++i) t[i] = i;

			Graph.GraphForm gf = new Graph.GraphForm();
			gf.Graph.AddEntry(t, x, new Pen(Color.Crimson));
			gf.Graph.SetXAxis(0, n, 5, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Graph.SetYAxis(0, 2, 5, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Size = new Size(640, 480);
			gf.ShowDialog();
#elif PHASE
			Spectrum spec = Spectrum.FromTimeSequence(x, 1);

			int n = spec.Count;
			double[] t = new double[n];
			for(int i=0; i<n; ++i) t[i] = i;

			double[] y = spec.GetPhase();
			Spectrum.Unwrap(y);

			Graph.GraphForm gf = new Graph.GraphForm();
			gf.Graph.AddEntry(t, y, new Pen(Color.Crimson));
			gf.Graph.SetXAxis(0, n, 4, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Graph.SetYAxis(-50, 0, 4, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Size = new Size(640, 480);
			gf.ShowDialog();
#elif GAIN
			Spectrum spec = Spectrum.FromTimeSequence(x, 1);

			int n = spec.Count;
			double[] t = new double[n];
			for(int i=0; i<n; ++i) t[i] = i;

			double[] y = spec.GetAmplitude();

			Graph.GraphForm gf = new Graph.GraphForm();
			gf.Graph.AddEntry(t, y, new Pen(Color.Crimson));
			gf.Graph.SetXAxis(0, n, 4, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Graph.SetYAxis(0, 4, 4, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black));
			gf.Size = new Size(640, 480);
			gf.ShowDialog();
#endif

#if false
			const int N = 512;
			double[] x = new double[N];
			double[] x0 = new double[N];
			double[] y0 = new double[N];
/*
			Random rnd = new Random();
			x0[0] = 1;
			x0[1] = 1;
			y0[0] = 0;
			y0[1] = 0;
			for(int i=1; i<N/2; ++i)
			{
				x0[2*i]   = rnd.NextDouble();
				x0[2*i+1] = rnd.NextDouble();
				y0[2*i]   = -x0[2*i+1];
				y0[2*i+1] = x0[2*i];
			}
			Fft fft = new Fft(N);
			fft.Invert(x0);
			fft.Invert(y0);
			for(int i=0; i<N; ++i)
				x[i] = x0[i];
//*/
//*
			for(int i=0; i<N; ++i)
			{
				x[i] = 100;
				for(int k=1; k<200; ++k)
					x[i] += short.MaxValue / 4 / k * Math.Cos(Math.PI / 512 * 32 * i * k);
				x0[i] = x[i];
				y0[i] = 0;
				for(int k=1; k<200; ++k)
					y0[i] += short.MaxValue / 4 / k * Math.Sin(Math.PI / 512 * 32 * i * k);
			}
//*/

			double[] y = Spectrum.HilbertTransform(x);

			using(StreamWriter writer = new StreamWriter("z.csv", false, Encoding.Default))
			{
/*
				for(int i=0; i<x.Length; ++i)
					writer.Write("{0},{1},{2},{3},{4}\n", i, x0[i], x[i], y0[i], y[i]);
//*/					
//*
				Spectrum a0 = Spectrum.FromTimeSequence(x0);
				Spectrum a  = Spectrum.FromTimeSequence(x);
				Spectrum b0 = Spectrum.FromTimeSequence(y0);
				Spectrum b  = Spectrum.FromTimeSequence(y);

				for(int i=0; i<a.Count; ++i)
				{
					writer.Write("{0},{1},{2},{3},{4}\n", i, a0[i].Abs, a[i].Abs, b0[i].Abs, b[i].Abs);
//					writer.Write("{0},{1},{2},{3},{4}\n", i, a0[i].Arg, a[i].Arg, b0[i].Arg, b[i].Arg);
				}
//*/
			}

#elif false
			const int N = 1024;

			double[] lp = new double[N];
			IFilter lpf = new LowPassFir(16, Common.Normalize(2000));
			lp[0] = lpf.GetValue(1);
			for(int i=1; i<N; ++i) lp[i] = lpf.GetValue(0);

			double[] bp = new double[N];
			IFilter bpf = new BandPassFir(16, Common.Normalize(2000), Common.Normalize(8000));
			lp[0] = bpf.GetValue(1);
			for(int i=1; i<N; ++i) bp[i] = bpf.GetValue(0);

			double[] hp = new double[N];
			IFilter hpf = new HighPassFir(16, Common.Normalize(8000));
			lp[0] = hpf.GetValue(1);
			for(int i=1; i<N; ++i) hp[i] = hpf.GetValue(0);

			Fft fft = new Fft(N);
			fft.Transform(1, lp);
			fft.Transform(1, bp);
			fft.Transform(1, hp);

			using(StreamWriter writer = new StreamWriter("z.csv", false, Encoding.Default))
			{
				double la = Common.Amp(lp[0]);
				double ba = Common.Amp(bp[0]);
				double ha = Common.Amp(hp[0]);
				writer.Write("{0},{1},{2},{3}\n", 0, la, ba, ha);

				for(int i=1; i<N/2; ++i)
				{
					la = Common.Amp(lp[2*i] * lp[2*i] + lp[2*i+1] * lp[2*i+1]);
					ba = Common.Amp(bp[2*i] * bp[2*i] + bp[2*i+1] * bp[2*i+1]);
					ha = Common.Amp(hp[2*i] * hp[2*i] + hp[2*i+1] * hp[2*i+1]);

					writer.Write("{0},{1},{2},{3}\n", i, la, ba, ha);
				}

				la = Common.Amp(lp[1]);
				ba = Common.Amp(bp[1]);
				ha = Common.Amp(hp[1]);
				writer.Write("{0},{1},{2},{3}\n", N/2, la, ba, ha);
			}
#elif false
			uint length;
			double[] l;
			double[] r;
			FormatHeader header;

			using(WaveReader reader = new WaveReader(@"TD100V00H030.wav"))
			{
				header = reader.Header;
				length = reader.Length;
				int tmp = reader.Read(length, out l, out r);
			}

			IFilter lpf = new LowPassFir(16, Common.Normalize(4500));
			IFilter bpf = new BandPassFir(16, Common.Normalize(4500), Common.Normalize(10000));
			IFilter hpf = new HighPassFir(16, Common.Normalize(10000));

			for(int i=0; i<l.Length; ++i)
			{
				double lp = lpf.GetValue(l[i]);
				double bp = bpf.GetValue(l[i]);
				double hp = hpf.GetValue(l[i]);
				l[i] = lp + bp + hp;
			}
			if(r != null)
				for(int i=0; i<l.Length; ++i)
				{
					double lp = lpf.GetValue(r[i]);
					double bp = bpf.GetValue(r[i]);
					double hp = hpf.GetValue(r[i]);
					r[i] = lp + bp + hp;
				}

			using(WaveWriter writer = new WaveWriter("z.wav", header))
			{
				writer.Write(l, r);
			}

#elif false
			using(StreamWriter writer = new StreamWriter("z.csv", false, Encoding.Default))
			{
				for(int i=0; i<l.Length; ++i)
				{
					writer.Write("{0},{1},{2}\n", i, l[i], r[i]);
				}
			}
#endif
		}
		static void Write(StreamWriter writer, string title, Delay f)
		{
			writer.Write("{0}\n{1}\n", title, f.Taps);
		}
		static void Write(StreamWriter writer, string title, OddLinearFir f)
		{
			writer.Write("{0}\n", title);
			double[] c = f.Coefficient;
			writer.Write("{0}", f.Coefficient[0]);
			for(int i=1; i<c.Length; ++i)
				writer.Write(", {0}", f.Coefficient[i]);
			writer.Write("\n");
		}
		static void Write(StreamWriter writer, string title, IirFilter f)
		{
			writer.Write("{0}\n", title);
			writer.Write("{0}", f.A[0]);
			for(int i=1; i<f.A.Length; ++i)
				writer.Write(", {0}", f.A[i]);
			writer.Write("\n");
			writer.Write("{0}", f.B[0]);
			for(int i=1; i<f.B.Length; ++i)
				writer.Write(", {0}", f.B[i]);
			writer.Write("\n");
		}

		static IFilter GetFilter()
		{
			double Fl = Common.Normalize(5000);
			double Fh = Common.Normalize(15000);

			LowPassFir  lpf = new LowPassFir(16, Fl);
			BandPassFir bpf = new BandPassFir(16, Fl, Fh);
			HighPassFir hpf = new HighPassFir(16, Fh);
			PeakingEqualizer peql = new PeakingEqualizer(Common.Normalize(900), 3, 1.5);
			PeakingEqualizer peqm = new PeakingEqualizer(Common.Normalize(8000), 5, 2);
			PeakingEqualizer peqh = new PeakingEqualizer(Common.Normalize(20000), 2, 4);
			Delay dl = new Delay(4);
			Delay dm = new Delay(8);
			Delay dh = new Delay(16);

#if OUTPUT_COEF
			using(StreamWriter writer = new StreamWriter("coef.txt"))
			{
				Write(writer, "LPF", lpf);
				Write(writer, "BPF", bpf);
				Write(writer, "HPF", hpf);
				Write(writer, "PEQ L", peql);
				Write(writer, "PEQ M", peqm);
				Write(writer, "PEQ H", peqh);
				Write(writer, "DLY L", dl);
				Write(writer, "DLY M", dm);
				Write(writer, "DLY H", dh);
			}
#endif

			IFilter low = new SerialConnector(new IFilter[]{lpf, peql, dl});
			IFilter mid = new SerialConnector(new IFilter[]{bpf, peqm, dm});
			IFilter hig = new SerialConnector(new IFilter[]{hpf, peqh, dh});

			IFilter filter = new Mixer(
				new IFilter[]{low, mid, hig},
				new Double[]{2, 1, 0.5});

			return filter;
		}
	}
}
