using System;
using System.Runtime.InteropServices;

namespace SoundLibrary.Wave
{
	/// <summary>
	/// WaveReader/WaveWriter で使う例外クラス。
	/// </summary>
	public class WaveException : Exception
	{
		public WaveException(){}
		public WaveException(string message) : base(message) {}
		public WaveException(string message, Exception innerException) : base(message, innerException) {}
	}

	/// <summary>
	/// Wave ファイルのフォーマットヘッダ。
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public struct FormatHeader
	{
		#region フィールド

		public short id;         // データ形式
		public short ch;         // チャネル数
		public int   sampleRate; // サンプリングレート
		public int   dataRate;   // データレート(＝チャネル数×ブロックサイズ)
		public short blockSize;  // ブロックサイズ(＝チャネル数×バイト/チャネル)
		public short sampleBit;  // 1サンプル辺りのビット数

		public const bool Stereo = true;
		public const bool Mono   = false;
		public const bool Bit16 = true;
		public const bool Bit8  = false;

		#endregion
		#region コンストラクタ

		/// <summary>
		/// サンプリングレート等のパラメータからヘッダ作成。
		/// </summary>
		/// <param name="rate">サンプリングレート</param>
		/// <param name="stereo">true ならステレオ、false ならモノラル</param>
		/// <param name="type">true なら16bit/sample、false なら8bit/sample</param>
		public FormatHeader(int rate, bool stereo, bool type)
		{
			this.id         = 1;
			this.ch         = stereo ? (short)2 : (short)1;
			this.sampleRate = rate;
			this.blockSize  = (short)(this.ch * (type ? 2 : 1));
			this.dataRate   = rate * this.blockSize;
			this.sampleBit  = type ? (short)16 : (short)8;
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// サンプルレート。
		/// </summary>
		public int Rate
		{
			set
			{
				this.sampleRate = value;
				this.dataRate = value * this.blockSize;
			}
			get
			{
				return this.sampleRate;
			}
		}

		/// <summary>
		/// サンプルビットが16ビットかどうか。
		/// </summary>
		public bool Is16Bit
		{
			set
			{
				this.blockSize  = (short)(this.ch * (value ? 2 : 1));
				this.sampleBit  = value ? (short)16 : (short)8;
			}
			get
			{
				return this.sampleBit == 16;
			}
		}

		/// <summary>
		/// チャネルがステレオかどうか。
		/// </summary>
		public bool IsStereo
		{
			set
			{
				this.ch = value ? (short)2 : (short)1;
				this.blockSize = (short)(this.sampleBit / 8 * this.ch);
			}
			get
			{
				return this.ch == 2;
			}
		}

		#endregion
	}//class FormatHeader
}//namespace Wave
