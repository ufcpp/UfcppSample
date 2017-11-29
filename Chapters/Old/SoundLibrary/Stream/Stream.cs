using System;

namespace SoundLibrary.Stream
{
	/// <summary>
	/// 音声ストリーム用インタフェース。
	/// めんどくさいんで、16ビットPCM限定。
	/// モノラル版。
	/// </summary>
	public abstract class Stream
	{
		/// <summary>
		/// バッファにデータを読み込む。
		/// </summary>
		/// <param name="buffer">データ読み込み先のバッファ</param>
		/// <param name="offset">データを書き込み始める場所のオフセット</param>
		/// <param name="size">読み出したいデータ長</param>
		/// <returns>実際に読み込んだデータ長</returns>
		public abstract int FillBuffer(short[] buffer, int offset, int size);

		/// <summary>
		/// バッファにデータを読み込む。
		/// </summary>
		/// <param name="buffer">データ読み込み先のバッファ</param>
		/// <returns>実際に読み込んだデータ長</returns>
		public int FillBuffer(short[] buffer)
		{
			return this.FillBuffer(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// データを空読みする。
		/// </summary>
		/// <param name="size">空読みしたいサイズ</param>
		/// <returns>実際から読みしたサイズ</returns>
		public abstract bool Skip(int size);
	}

	/// <summary>
	/// 音声ストリームにバッファを付けたもの。
	/// </summary>
	public class BufferedStream
	{
		#region 定数・フィールド

		const int DEFAULT_BUFFER_SIZE = 1024;

		Stream stream;
		short[] buffer;
		int current;
		int last;

		#endregion
		#region 初期化

		/// <summary>
		/// デフォルトサイズのバッファを用意。
		/// </summary>
		/// <param name="stream">入力元のストリーム</param>
		public BufferedStream(Stream stream) : this(stream, DEFAULT_BUFFER_SIZE) {}

		/// <summary>
		/// バッファサイズを指定してバッファを用意。
		/// </summary>
		/// <param name="stream">入力元のストリーム</param>
		/// <param name="size">バッファサイズ</param>
		public BufferedStream(Stream stream, int size) : this(stream, new short[size]) {}

		/// <summary>
		/// バッファを直接指定。
		/// </summary>
		/// <param name="stream">入力元のストリーム</param>
		/// <param name="buffer">バッファ</param>
		public BufferedStream(Stream stream, short[] buffer)
		{
			this.stream = stream;
			this.buffer = buffer;
			this.current = -1;
			this.last = 0;
		}

		#endregion
		#region 値の取得など

		/// <summary>
		/// 次のデータに移動。
		/// </summary>
		/// <returns>まだデータがバッファ内に残っているかどうか</returns>
		public bool MoveNext()
		{
			++this.current;

			if(this.current >= this.last)
			{
				this.last = this.stream.FillBuffer(this.buffer);				
				this.current = 0;
			}

			return this.last != 0;
		}

		/// <summary>
		/// 値の取得。
		/// </summary>
		public short Value
		{
			get{return this.buffer[this.current];}
		}

		#endregion
	}
}
