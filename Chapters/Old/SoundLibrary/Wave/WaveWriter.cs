using System;
using System.IO;

namespace SoundLibrary.Wave
{
	/// <summary>
	/// RIFF Wave 形式のファイルに音声データを書き込む。
	/// </summary>
	public class WaveWriter : IDisposable
	{
		BinaryWriter writer;
		FormatHeader header;
		uint dataLength = 0;

		public WaveWriter(){}

		/// <summary>
		/// ファイル名を指定して開く。
		/// </summary>
		/// <param name="filename">書き込み先 Wave ファイル名</param>
		/// <param name="header">Wave のヘッダ</param>
		public WaveWriter(string filename, FormatHeader header)
		{
			this.Open(filename, header);
		}

		/// <summary>
		/// ストリームに書き込む。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="header">Wave のヘッダ</param>
		public WaveWriter(BinaryWriter writer, FormatHeader header)
		{
			this.Open(writer, header);
		}

		public void Dispose()
		{
			this.Close();
		}

		/// <summary>
		/// Wave ファイルを開く。
		/// </summary>
		/// <param name="filename">Wave ファイル名</param>
		public void Open(string filename, FormatHeader header)
		{
			Open(new BinaryWriter(new BufferedStream(File.Create(filename))), header);
		}

		/// <summary>
		/// Wave ヘッダ(RIFF, fmt chunk, data chunk のデータ長まで)をストリームに書き出す。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="header">Wave のヘッダ</param>
		/// <param name="length">データ長(サンプル数)</param>
		public static void WriteHeader(BinaryWriter writer, FormatHeader header, int length)
		{
			byte[] buf;

			length *= header.blockSize;

			writer.Write(Util.RIFF);
			writer.Write((uint)length + 36u);
			writer.Write(Util.WAVE);
			writer.Write(Util.FMT);
			writer.Write((uint)16);

			unsafe
			{
				buf = new byte[16];
				fixed(byte* p = buf)
				{
					*(FormatHeader*)p = header;
				}
			}
			writer.Write(buf);
			writer.Write(Util.DATA);
			writer.Write((uint)length);
		}

		/// <summary>
		/// ヘッダのデータ長の部分を修正する。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="length">修正後のデータ長(サンプル数)</param>
		/// <param name="blockSize">ブロックサイズ</param>
		public static void ModifyHeader(BinaryWriter writer, int length, int blockSize)
		{
			length = length * blockSize;

			writer.Seek(4, SeekOrigin.Begin);
			writer.Write((uint)length + 36u);

			writer.Seek(40, SeekOrigin.Begin);
			writer.Write((uint)length);
		}

		/// <summary>
		/// データ書き出し。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="l">書き込みたいデータ(L ch)</param>
		/// <param name="r">書き込みたいデータ(R ch)</param>
		/// <returns></returns>
		public static int Write(BinaryWriter writer, FormatHeader header, double[] l, double[] r)
		{
			int i = 0;

			try
			{
				int length = l.Length;

				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
						}
					}
				}
				else // ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
							writer.Write(Util.ClipToByte(r[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
							writer.Write(Util.ClipToShort(r[i]));
						}
					}
				}//ステレオ
			}
			catch(IOException){return 0;}
			return i;
		}

		/// <summary>
		/// データ書き出し。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="l">書き込みたいデータ(L ch)</param>
		/// <param name="r">書き込みたいデータ(R ch)</param>
		/// <returns></returns>
		public static int Write(BinaryWriter writer, FormatHeader header, float[] l, float[] r)
		{
			int i = 0;

			try
			{
				int length = l.Length;

				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
						}
					}
				}
				else // ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
							writer.Write(Util.ClipToByte(r[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
							writer.Write(Util.ClipToShort(r[i]));
						}
					}
				}//ステレオ
			}
			catch(IOException){return 0;}
			return i;
		}

		/// <summary>
		/// データ書き出し。
		/// </summary>
		/// <param name="writer">書き込み先ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="l">書き込みたいデータ(L ch)</param>
		/// <param name="r">書き込みたいデータ(R ch)</param>
		/// <returns></returns>
		public static int Write(BinaryWriter writer, FormatHeader header, short[] l, short[] r)
		{
			int i = 0;

			try
			{
				int length = l.Length;

				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
						}
					}
				}
				else // ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToByte(l[i]));
							writer.Write(Util.ClipToByte(r[i]));
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(Util.ClipToShort(l[i]));
							writer.Write(Util.ClipToShort(r[i]));
						}
					}
				}//ステレオ
			}
			catch(IOException){return 0;}
			return i;
		}

		/// <summary>
		/// Wave ファイルを開く。
		/// </summary>
		/// <param name="reader">Wave ファイルを格納したストリーム</param>
		public void Open(BinaryWriter writer, FormatHeader header)
		{
			if(this.writer != null)
			{
				this.writer.Close();
			}

			this.writer = writer;
			this.header = header;

			WaveWriter.WriteHeader(writer, header, 0); // データ長は仮に0を入れておく。
		}//Open

		/// <summary>
		/// Wave ファイルを閉じる。
		/// </summary>
		public void Close()
		{
			if(writer == null) return;

			WaveWriter.ModifyHeader(this.writer, (int)this.dataLength, this.header.blockSize);

			writer.Close();
			writer = null;
		}

		/// <summary>
		/// データの書き込み。
		/// </summary>
		/// <param name="length">書き込むサンプル数。</param>
		/// <param name="l">左チャネルのデータ。</param>
		/// <param name="r">右チャネルのデータ。</param>
		/// <returns>実際に書き込んだサンプル数。</returns>
		public int Write(double[] l, double[] r)
		{
			if(this.writer == null) return 0;

			uint length = (uint)l.Length;
			int i = WaveWriter.Write(this.writer, this.header, l, r);

			this.dataLength += (uint)i;
			return i;
		}//Write

		/// <summary>
		/// データの書き込み。
		/// </summary>
		/// <param name="length">書き込むサンプル数。</param>
		/// <param name="l">左チャネルのデータ。</param>
		/// <param name="r">右チャネルのデータ。</param>
		/// <returns>実際に書き込んだサンプル数。</returns>
		public int Write(float[] l, float[] r)
		{
			if(this.writer == null) return 0;

			uint length = (uint)l.Length;
			int i = WaveWriter.Write(this.writer, this.header, l, r);
	
			this.dataLength += (uint)i;
			return i;
		}//Write

		/// <summary>
		/// データの書き込み。
		/// </summary>
		/// <param name="length">書き込むサンプル数。</param>
		/// <param name="l">左チャネルのデータ。</param>
		/// <param name="r">右チャネルのデータ。</param>
		/// <returns>実際に書き込んだサンプル数。</returns>
		public int Write(short[] l, short[] r)
		{
			if(this.writer == null) return 0;

			uint length = (uint)l.Length;
			int i = WaveWriter.Write(this.writer, this.header, l, r);
	
			this.dataLength += (uint)i;
			return i;
		}//Write

		/// <summary>
		/// Wave の生データをそのまま書き込む。
		/// </summary>
		/// <param name="writer">書き込み先</param>
		/// <param name="data">書き込むデータ</param>
		public static void WriteRawData(BinaryWriter writer, byte[] data)
		{
			writer.Write(data);
		}

		/// <summary>
		/// Wave の生データをそのまま書き込む。
		/// </summary>
		/// <param name="writer">書き込み先</param>
		/// <param name="data">書き込むデータ</param>
		/// <param name="length">書き込む長さ(バイト数)</param>
		public static void WriteRawData(BinaryWriter writer, byte[] data, int length)
		{
			writer.Write(data, 0, length);
		}

		/// <summary>
		/// Wave の生データをそのまま書き込む。
		/// </summary>
		/// <param name="data">書き込むデータ</param>
		public void WriteRawData(byte[] data)
		{
			this.WriteRawData(data, data.Length);
		}

		/// <summary>
		/// Wave の生データをそのまま書き込む。
		/// </summary>
		/// <param name="data">書き込むデータ</param>
		/// <param name="length">書き込む長さ(バイト数)</param>
		public void WriteRawData(byte[] data, int length)
		{
			WaveWriter.WriteRawData(this.writer, data, length);
			this.dataLength += (uint)(length / this.header.blockSize);
		}

		public uint Length
		{
			get{return this.dataLength;}
		}

		/// <summary>
		/// 1サンプル読み出す。
		/// モノラル16ビット以外の場合、サポート対象外。
		/// </summary>
		/// <param name="data">1サンプル分のデータ</param>
		public void WriteShort(short data)
		{
			if(!this.header.Is16Bit || this.header.IsStereo)
				return;

			this.writer.Write(data);
			++this.dataLength;
		}

		public void WriteShort(short l, short r)
		{
			System.Diagnostics.Debug.Assert(this.header.Is16Bit);

			if(this.header.IsStereo)
			{
				this.writer.Write(l);
				this.writer.Write(r);
				++this.dataLength;
			}
			else
			{
				this.writer.Write(l);
				++this.dataLength;
			}
		}
	}//class WaveWriter
}
