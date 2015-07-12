using System;
using SoundLibrary.Filter;

namespace SoundLibrary.Wave
{
	public interface IStereoFilter
	{
		void Filter(ref short l, ref short r);
		void Clear();
	}

	/// <summary>
	/// IFilter → Stereo1SampleFilter 用アダプタ。
	/// 入力 (Xl, Xr), 出力 (Yl, Yr)、
	/// Yl = Hll Xl + Hrl Xr
	/// Yr = Hlr Xl + Hrr Xr
	/// </summary>
	public class CrossStereoFilter : IStereoFilter
	{
		#region フィールド

		IFilter ll; // 特性 Hll を持つフィルタ。
		IFilter lr; // 特性 Hlr を持つフィルタ。
		IFilter rl; // 特性 Hrl を持つフィルタ。
		IFilter rr; // 特性 Hrr を持つフィルタ。

		#endregion
		#region コンストラクタ

		/// <summary>
		/// Hll, Hlr, Hrl, Hrr を別個に指定。
		/// </summary>
		/// <param name="ll">特性 Hll を持つフィルタ</param>
		/// <param name="lr">特性 Hlr を持つフィルタ</param>
		/// <param name="rl">特性 Hrl を持つフィルタ</param>
		/// <param name="rr">特性 Hrr を持つフィルタ</param>
		public CrossStereoFilter(IFilter ll, IFilter lr, IFilter rl, IFilter rr)
		{
			this.ll = ll;
			this.lr = lr;
			this.rl = rl;
			this.rr = rr;
		}

		/// <summary>
		/// Hll ＝ Hrr, Hlr ＝ Hrlの場合。
		/// </summary>
		/// <param name="ll">特性 Hll を持つフィルタ</param>
		/// <param name="lr">特性 Hlr を持つフィルタ</param>
		public CrossStereoFilter(IFilter ll, IFilter lr)
		{
			this.ll = ll;
			this.lr = lr;
			this.rl = (IFilter)lr.Clone();
			this.rr = (IFilter)ll.Clone();
		}

		#endregion

		public void Filter(ref short l, ref short r)
		{
			double xl = this.ll.GetValue(l) + this.rl.GetValue(r);
			double xr = this.lr.GetValue(l) + this.rr.GetValue(r);
			l = SoundLibrary.Wave.Util.ClipToShort(xl);
			r = SoundLibrary.Wave.Util.ClipToShort(xr);
		}

		/// <summary>
		/// 状態の初期化。
		/// </summary>
		public void Clear()
		{
			this.ll.Clear();
			this.lr.Clear();
			this.rl.Clear();
			this.rr.Clear();
		}
	}

	/// <summary>
	/// IFilter → Stereo1SampleFilter 用アダプタ。
	/// L/R それぞれに別個のフィルタを適用。
	/// 入力 (Xl, Xr), 出力 (Yl, Yr)、
	/// Yl = Hl Xl
	/// Yr = Hr Xr
	/// </summary>
	public class StereoFilter : IStereoFilter
	{
		#region フィールド

		IFilter l; // 特性 Hl を持つフィルタ。
		IFilter r; // 特性 Hr を持つフィルタ。

		#endregion
		#region コンストラクタ

		/// <summary>
		/// Hl, Hr を別個に指定。
		/// </summary>
		/// <param name="l">特性 Hl を持つフィルタ</param>
		/// <param name="r">特性 Hr を持つフィルタ</param>
		public StereoFilter(IFilter l, IFilter r)
		{
			this.l = l;
			this.r = r;
		}

		/// <summary>
		/// Hl ＝ Hr の場合。
		/// </summary>
		/// <param name="l">特性 Hl を持つフィルタ</param>
		public StereoFilter(IFilter l)
		{
			this.l = l;
			this.r = (IFilter)l.Clone();
		}

		#endregion

		public void Filter(ref short l, ref short r)
		{
			double xl = this.l.GetValue(l);
			double xr = this.r.GetValue(r);
			l = SoundLibrary.Wave.Util.ClipToShort(xl);
			r = SoundLibrary.Wave.Util.ClipToShort(xr);
		}

		/// <summary>
		/// 状態の初期化。
		/// </summary>
		public void Clear()
		{
			this.l.Clear();
			this.r.Clear();
		}
	}

	/// <summary>
	/// IFilter → Stereo1SampleFilter 用アダプタ。
	/// ステレオ音をモノラル化してからフィルタリング。
	/// 入力 (Xl, Xr), 出力 (Yl, Yr)、
	/// X = (Xl * Xr) / 2
	/// Yl = Yr = H X
	/// </summary>
	public class StereoToMonauralFilter : IStereoFilter
	{
		#region フィールド

		IFilter f; // 特性 H を持つフィルタ。

		#endregion
		#region コンストラクタ

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="f">特性 H を持つフィルタ</param>
		public StereoToMonauralFilter(IFilter f)
		{
			this.f = f;
		}

		#endregion

		public void Filter(ref short l, ref short r)
		{
			double x = this.f.GetValue(0.5 * ((int)l + r));
			l = r = SoundLibrary.Wave.Util.ClipToShort(x);
		}

		/// <summary>
		/// 状態の初期化。
		/// </summary>
		public void Clear()
		{
			this.f.Clear();
		}
	}

	/// <summary>
	/// IFilter → Monaural1SampleFilter 用アダプタ。
	/// Y = H X
	/// </summary>
	public class MonauralFilter
	{
		#region フィールド

		IFilter f; // 特性 H を持つフィルタ。

		#endregion
		#region コンストラクタ

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="f">特性 H を持つフィルタ</param>
		public MonauralFilter(IFilter f)
		{
			this.f = f;
		}

		#endregion

		public void Filter(ref short l)
		{
			double x = this.f.GetValue(l);
			l = SoundLibrary.Wave.Util.ClipToShort(x);
		}

		/// <summary>
		/// 状態の初期化。
		/// </summary>
		public void Clear()
		{
			this.f.Clear();
		}
	}
}
