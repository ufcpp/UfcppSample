#define CHECK_RANGE

using System;

using SoundLibrary.Filter;

namespace SoundLibrary.Stream.Monaural
{
	/// <summary>
	/// Stream にフィルタを掛ける。
	/// </summary>
	public class FilteredStream : Stream
	{
		#region フィールド

		Stream stream; // inner stream
		IFilter filter;

		#endregion
		#region 初期化

		/// <summary>
		/// stream に filter を掛ける。
		/// </summary>
		/// <param name="stream">内部ストリーム</param>
		/// <param name="filter">掛けたいフィルター</param>
		public FilteredStream(Stream stream, IFilter filter)
		{
			this.stream = stream;
			this.filter = filter;
		}

		#endregion
		#region Stream メンバ

		public override int FillBuffer(short[] buffer, int offset, int size)
		{
			size = this.stream.FillBuffer(buffer, offset, size);

			for(int i=0; i<size; ++i)
			{
				double val= this.filter.GetValue(buffer[i]);
#if CHECK_RANGE
			if(val > short.MaxValue) val = short.MaxValue;
			if(val < short.MinValue) val = short.MinValue;
#endif
				buffer[i] = (short)val;
			}

			return size;
		}

		public override bool Skip(int size)
		{
			return this.stream.Skip(size);
		}

		#endregion
	}
}
