using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// エンベロープ生成用のパラメータ。
	/// </summary>
	public class EnvelopeParameter
	{
		public double attackLevel;  // アタックレベル(リニア値)
		public double sustainLevel; // サステインレベル(リニア値)
		public int attackTime;      // アタックタイム(ステップ数)
		public int decayTime;       // ディケイタイム(ステップ数)
		public int releaseTime;     // リリースタイム(ステップ数)

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="al">アタックレベル(リニア値)</param>
		/// <param name="sl">サステインレベル(リニア値)</param>
		/// <param name="at">アタックタイム(ステップ数)</param>
		/// <param name="dt">リリースタイム(ステップ数)</param>
		/// <param name="rt">リリースタイム(ステップ数)</param>
		public EnvelopeParameter(double al, double sl, int at, int dt, int rt)
		{
			this.attackLevel  = al;
			this.sustainLevel = sl;
			this.attackTime   = at;
			this.decayTime    = dt;
			this.releaseTime  = rt;
		}
	}

	/// <summary>
	/// 元となる Sound にエンベロープ曲線を掛けた Sound を生成する。
	/// 
	/// アタックタイム
	/// 　　　　ディケイタイム　　　リリースタイム
	/// ←──→←→　　　　　　　　←→
	/// 　　　／＼　←アタックレベル
	/// 　　／　　＼
	/// 　／　　　　￣￣￣￣￣￣￣￣＼ 　←サステインレベル
	/// ／　　　　　　　　　　　　　　＼
	/// </summary>
	public class Envelope : Sound
	{
		EnvelopeParameter parameter;
		Sound sound;

		/// <summary>
		/// エンベロープパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="al">アタックレベル(sound の振幅との相対値)</param>
		/// <param name="sl">サステインレベル(sound の振幅との相対値)</param>
		/// <param name="at">アタックタイム(ステップ数)</param>
		/// <param name="dt">リリースタイム(ステップ数)</param>
		/// <param name="rt">リリースタイム(ステップ数)</param>
		/// <param name="sound">元となる音</param>
		public Envelope(double al, double sl, int at, int dt, int rt, Sound sound)
			: this(new EnvelopeParameter(al, sl, at, dt, rt), sound)
		{
		}

		/// <summary>
		/// エンベロープパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="sound">元となる音</param>
		public Envelope(EnvelopeParameter parameter, Sound sound)
		{
			CheckParameter(parameter, sound.Length);
			this.parameter = parameter;
			this.sound = sound;
		}

		/// <summary>
		/// パラメータの正当性をチェック。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		/// <param name="length">音の長さ</param>
		static void CheckParameter(EnvelopeParameter parameter, int length)
		{
			if(parameter.attackTime < 0)
				throw new ArgumentException("アタックタイムが負");
			if(parameter.decayTime < 0)
				throw new ArgumentException("ディケイタイムが負");
			if(parameter.releaseTime < 0)
				throw new ArgumentException("リリースタイムが負");
			if(parameter.attackTime + parameter.decayTime + parameter.releaseTime > length)
				throw new ArgumentException("音が短すぎ");
		}

		public override int Length
		{
			get
			{
				return this.sound.Length;
			}
		}

		public override double[] ToArray()
		{
			double[] x = this.sound.ToArray();
			double grad;

			double al = this.parameter.attackLevel;
			double sl = this.parameter.sustainLevel;
			int    at = this.parameter.attackTime;
			int    dt = this.parameter.decayTime;
			int    rt = this.parameter.releaseTime;
			int   len = this.sound.Length;

			int i = 0;

			// アタック
			grad = al / at;
			for(; i <= at; ++i)
			{
				int n = i + 1;
				x[i] *= n * grad;
			}

			// ディケイ
			grad = grad = (al - sl) / dt;
			for(; i <= at + dt; ++i)
			{
				int n = at + dt - i;
				x[i] *= n * grad  + sl;
			}

			// サステイン
			for(; i < len - rt; ++i)
			{
				x[i] *= sl;
			}

			// リリース
			grad = sl / rt;
			for(; i<len; ++i)		
			{
				int n = len - i;
				x[i] *= n * grad;
			}

			return x;
		}
	}
}
