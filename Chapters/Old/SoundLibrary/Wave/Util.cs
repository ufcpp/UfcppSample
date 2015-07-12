using System;

namespace SoundLibrary.Wave
{
	/// <summary>
	/// ステレオ音を1サンプル単位でフィルタリングする関数。
	/// </summary>
	public delegate void Stereo1SampleFilter(ref short lVal, ref short rVal);

	/// <summary>
	/// モノラル音を1サンプル単位でフィルタリングする関数。
	/// </summary>
	public delegate void Monaural1SampleFilter(ref short val);

	/// <summary>
	/// SoundLibrary.Wave 内で使うユーティリティ関数の定義。
	/// </summary>
	public class Util
	{
		#region RIFF/WAV ファイルヘッダ中の定数

		public static readonly byte[] RIFF = System.Text.Encoding.ASCII.GetBytes("RIFF");
		public static readonly byte[] WAVE = System.Text.Encoding.ASCII.GetBytes("WAVE");
		public static readonly byte[] FMT  = System.Text.Encoding.ASCII.GetBytes("fmt ");
		public static readonly byte[] DATA = System.Text.Encoding.ASCII.GetBytes("data");

		#endregion
		#region 2つの配列の比較

		public static bool Equal(byte[] a, byte[] b)
		{
			return a[0] == b[0] && a[1] == b[1] && a[2] == b[2] && a[3] == b[3];

			/*
			unsafe
			{
				fixed(byte* pa = a, pb = b)
				{
					return *(int*)pa == *(int*)pb;
				}
			}
			//*/
		}

		#endregion
		#region MemCopy

		public static void MemCopy(byte[] orig, short[] dest)
		{
			MemCopy(orig, dest, orig.Length);
		}

		public static void MemCopy(byte[] orig, short[] dest, int length)
		{
			unsafe
			{
				fixed(void* po = orig, pd = dest)
				{
					MemCopy(po, pd, length);
				}
			}
		}

		public static void MemCopy(short[] orig, byte[] dest)
		{
			MemCopy(orig, dest, orig.Length * 2);
		}

		public static void MemCopy(short[] orig, byte[] dest, int length)
		{
			unsafe
			{
				fixed(void* po = orig, pd = dest)
				{
					MemCopy(po, pd, length);
				}
			}
		}

		public static void MemCopy(byte[] orig, byte[] dest)
		{
			unsafe
			{
				fixed(void* po = orig, pd = dest)
				{
					MemCopy(po, pd, orig.Length);
				}
			}
		}

		unsafe public static void MemCopy(void* orig, void* dest, int length)
		{
			int length16 = length - (length % (sizeof(int) * 16));
			int* end = ((int*)orig) + length16 / sizeof(int);
			int* p1 = (int*)orig;
			int* p2 = (int*)dest;
			while(p1!=end)
			{
				*p2 = *p1; ++p1; ++p2;
				*p2 = *p1; ++p1; ++p2;
				*p2 = *p1; ++p1; ++p2;
				*p2 = *p1; ++p1; ++p2;
			}

			byte* bend = (byte*)orig + length;
			byte* bp1 = (byte*)p1;
			byte* bp2 = (byte*)p2;
			for(; bp1!=bend; ++bp1, ++bp2)
				*bp2 = *bp1;
		}

		public static void MemCopy(short[] orig, int origOffset, short[] dest, int destOffset, int length)
		{
			unsafe
			{
				fixed(short* po = orig)
				{
					fixed(short* pd = dest)
					{
						MemCopy(po + origOffset, pd + destOffset, sizeof(short) * length);
					}
				}
			}
		}

		public static void MemCopy(byte[] orig, int origOffset, short[] dest, int destOffset, int length)
		{
			unsafe
			{
				fixed(byte* po = orig)
				{
					fixed(short* pd = dest)
					{
						MemCopy(po + origOffset, pd + destOffset, sizeof(byte) * length);
					}
				}
			}
		}

		public static void MemCopy(short[] orig, int origOffset, byte[] dest, int destOffset, int length)
		{
			unsafe
			{
				fixed(short* po = orig)
				{
					fixed(byte* pd = dest)
					{
						MemCopy(po + origOffset, pd + destOffset, sizeof(short) * length);
					}
				}
			}
		}

		public static void MemCopy(byte[] orig, int origOffset, byte[] dest, int destOffset, int length)
		{
			unsafe
			{
				fixed(byte* po = orig)
				{
					fixed(byte* pd = dest)
					{
						MemCopy(po + origOffset, pd + destOffset, sizeof(byte) * length);
					}
				}
			}
		}

		#endregion
		#region 型変換（Wave 格納形式 → double 等）

		/// <summary>
		/// wave 格納形式（128バイアス表現）→ short
		/// </summary>
		/// <param name="x">128バイアス表現のデータ</param>
		/// <returns>short 値</returns>
		internal static short BiasedByteToShort(byte x)
		{
			return (short)((short)x - 128);
		}

		#endregion
		#region 型変換（double 等 → Wave 格納形式、short, byte の範囲に収まるように値をクリップ）

		/// <summary>
		/// → byte の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static byte ClipToByte(double x)
		{
			x += 128;
			if(x < 0) x = 0;
			else if(x > 255) x = 255;
			return (byte)x;
		}

		/// <summary>
		/// → byte の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static byte ClipToByte(float x)
		{
			x += 128;
			if(x < 0) x = 0;
			else if(x > 255) x = 255;
			return (byte)x;
		}

		/// <summary>
		/// → byte の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static byte ClipToByte(short x)
		{
			x += 128;
			if(x < 0) x = 0;
			else if(x > 255) x = 255;
			return (byte)x;
		}

		/// <summary>
		/// → short の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static short ClipToShort(double x)
		{
			if(x < short.MinValue) x = short.MinValue;
			else if(x > short.MaxValue) x = short.MaxValue;
			return (short)x;
		}

		/// <summary>
		/// → short の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static short ClipToShort(float x)
		{
			if(x < short.MinValue) x = short.MinValue;
			else if(x > short.MaxValue) x = short.MaxValue;
			return (short)x;
		}

		/// <summary>
		/// → short の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		internal static short ClipToShort(short x)
		{
			return x;
		}

		#endregion
		#region Raw Data に対するフィルタリング

		/// <summary>
		/// Raw Wav Data に対してフィルタリングする。
		/// ステレオ版。
		/// </summary>
		/// <param name="data">Raw Data</param>
		/// <param name="filter">フィルタ</param>
		/// <param name="is16bit">Raw Data の形式が16ビットか8ビットか。</param>
		public static void FilteringRawData(byte[] data, Stereo1SampleFilter filter, bool is16bit)
		{
			unsafe
			{
				fixed(byte* begin = data)
				{
					if(is16bit)
					{
						short* end = (short*)(begin + (data.Length & (~3)));
						for(short* p = (short*)begin; p != (short*)end; p += 2)
						{
							filter(ref *p, ref *(p + 1));
						}
					}
					else
					{
						byte* end = begin + (data.Length & (~1));
						for(byte* p = begin; p != end; p += 2)
						{
							short l = BiasedByteToShort(*p);
							short r = BiasedByteToShort(*(p + 1));
							filter(ref l, ref r);
							*p = ClipToByte(l);
							*(p + 1) = ClipToByte(r);
						}
					}
				}
			}
		}

		/// <summary>
		/// Raw Wav Data に対してフィルタリングする。
		/// モノラル版。
		/// </summary>
		/// <param name="data">Raw Data</param>
		/// <param name="filter">フィルタ</param>
		/// <param name="is16bit">Raw Data の形式が16ビットか8ビットか。</param>
		public static void FilteringRawData(byte[] data, Monaural1SampleFilter filter, bool is16bit)
		{
			unsafe
			{
				fixed(byte* begin = data)
				{
					if(is16bit)
					{
						short* end = (short*)(begin + (data.Length & (~1)));
						for(short* p = (short*)begin; p != (short*)end; ++p)
						{
							filter(ref *p);
						}
					}
					else
					{
						byte* end = begin + data.Length;
						for(byte* p = begin; p != end; ++p)
						{
							short l = BiasedByteToShort(*p);
							filter(ref l);
							*p = ClipToByte(l);
						}
					}
				}
			}
		}

		#endregion
	}
}
