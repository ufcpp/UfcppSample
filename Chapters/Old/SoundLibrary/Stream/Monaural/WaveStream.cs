using System;
using SoundLibrary.Wave;

namespace SoundLibrary.Stream.Monaural
{
	/// <summary>
	/// WaveReader からデータを読み出し続けるだけのストリーム。
	/// </summary>
	public class WaveStream : Stream, IDisposable
	{
		#region フィールド

		WaveReader reader;
		byte[] buffer;

		#endregion
		#region 初期化

		public WaveStream(WaveReader reader)
		{
			this.reader = reader;
			this.buffer = null;
		}

		#endregion
		#region Stream メンバ

		public override int FillBuffer(short[] buffer, int offset, int size)
		{
			if(this.buffer == null || this.buffer.Length < size * 2)
			{
				this.buffer = new byte[size * 2];
			}

			size = this.reader.ReadRawData(this.buffer, 0, size * 2);

			SoundLibrary.Wave.Util.MemCopy(this.buffer, 0, buffer, offset, size);

			return size / 2;
		}

		public override bool Skip(int size)
		{
			return this.reader.Skip(size);
		}

		#endregion
		#region IDisposable メンバ

		public void Close()
		{
			this.reader.Close();
		}

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
