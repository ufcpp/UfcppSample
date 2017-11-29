using System;
using System.IO;

namespace SoundLibrary.Wave
{
	/// <summary>
	/// RIFF Wave 形式のファイルから音声データを読み出す。
	/// </summary>
	public class WaveReader : IDisposable
	{
		BinaryReader reader;
		FormatHeader header;
		uint dataLength = 0;

		public WaveReader(){}

		/// <summary>
		/// ファイル名を指定して開く。
		/// </summary>
		/// <param name="filename">Wave ファイル名</param>
		public WaveReader(string filename)
		{
			this.Open(filename);
		}

		/// <summary>
		/// ストリームから開く。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		public WaveReader(BinaryReader reader)
		{
			this.Open(reader);
		}

		public void Dispose()
		{
			this.Close();
		}

		/// <summary>
		/// Wave ファイルストリームからヘッダを読み出す。
		/// </summary>
		/// <param name="reader">読み出し元のストリーム</param>
		/// <returns>読み出したヘッダ</returns>
		public static FormatHeader ReadHeader(BinaryReader reader)
		{
			byte[] buf;

			buf= reader.ReadBytes(4);

			if(!Util.Equal(buf, Util.RIFF))
			{
				throw new WaveException("このファイルはRIFF形式ではありません。");
			}

			reader.ReadBytes(4); //ファイルサイズ読み飛ばし。

			buf= reader.ReadBytes(4);
			if(!Util.Equal(buf, Util.WAVE))
			{
				throw new WaveException("このファイルはwave形式ではありません。");
			}

			// fmt chunk 読み出し
			FormatHeader header;
			while(true)
			{
				buf = reader.ReadBytes(4);
				int length = reader.ReadInt32();
				byte[] data = reader.ReadBytes(length);

				if(length < 16)
				{
					throw new WaveException("ヘッダ長が短すぎます。");
				}
				if(Util.Equal(buf, Util.FMT))
				{
					unsafe
					{
						fixed(byte* p= data)
						{
							header = *(FormatHeader*)p;
						}
					}
					break;
				}
			}
			return header;
		}

		/// <summary>
		/// Wave ファイルストリームからデータチャンクを探す。
		/// fmt chunk よりも data chunk が後ろにあると言う前提で、
		/// ReadHeader の後に呼び出す。
		/// </summary>
		/// <param name="reader">読み出し元のストリーム</param>
		/// <returns>データチャンクサイズ(バイト数)</returns>
		public static int ReadDataChunk(BinaryReader reader)
		{
			int length = 0;

			while(true)
			{
				byte[] buf = reader.ReadBytes(4);
				length = reader.ReadInt32();

				if(length < 16)
				{
					throw new WaveException("ヘッダ長が短すぎます。");
				}
				if(Util.Equal(buf, Util.DATA))
				{
					break;
				}
				reader.ReadBytes(length);
			}
			return length;
		}

		/// <summary>
		/// Wave の生データをそのまま読み出す。
		/// </summary>
		/// <param name="reader">読み出し元</param>
		/// <param name="data">読み出し先</param>
		/// <returns>読み出したデータ</returns>
		public static int ReadRawData(BinaryReader reader, byte[] data)
		{
			return ReadRawData(reader, data, 0, data.Length);
		}

		/// <summary>
		/// Wave の生データをそのまま読み出す。
		/// </summary>
		/// <param name="reader">読み出し元</param>
		/// <param name="data">読み出し先</param>
		/// <param name="offset">読み出し先の開始地点</param>
		/// <returns>実際読み出したデータの長さ</returns>
		public static int ReadRawData(BinaryReader reader, byte[] data, int offset)
		{
			return ReadRawData(reader, data, offset, data.Length - offset);
		}

		/// <summary>
		/// Wave の生データをそのまま読み出す。
		/// </summary>
		/// <param name="reader">読み出し元</param>
		/// <param name="data">読み出し先</param>
		/// <param name="offset">読み出し先の開始地点</param>
		/// <param name="length">読み出す長さ(バイト数)</param>
		/// <returns>実際読み出したデータの長さ</returns>
		public static int ReadRawData(BinaryReader reader, byte[] data, int offset, int length)
		{
			return reader.Read(data, offset, length);
		}

		/// <summary>
		/// Wave の生データをそのまま読み出す。
		/// </summary>
		/// <param name="data">読込先</param>
		/// <param name="offset">読込先の開始オフセット(バイト数)</param>
		/// <param name="length">読み出す長さ(バイト数)</param>
		/// <returns>実際に読み込んだ長さ(バイト数)</returns>
		public int ReadRawData(byte[] data, int offset, int length)
		{
			if(this.dataLength * this.header.blockSize < length)
				length = (int)(this.dataLength * this.header.blockSize);

			length = WaveReader.ReadRawData(this.reader, data, offset, length);
			this.dataLength -= (uint)(length / this.header.blockSize);
			return length;
		}

		/// <summary>
		/// Wave の生データをそのまま読み出す。
		/// </summary>
		/// <param name="data">データ格納先</param>
		/// <returns>読み込んだ長さ(サンプル数)</returns>
		public int ReadRawData(byte[] data)
		{
			return this.ReadRawData(data, 0, data.Length);
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, double[]l, double[] r)
		{
			uint length = (uint)l.Length;

			int i = 0;
			try
			{
				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)reader.ReadInt16();
						}
					}
				}
				else //ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)(reader.ReadByte() - 128);
							r[i] = (double)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)reader.ReadInt16();
							r[i] = (double)reader.ReadInt16();
						}
					}
				}//ステレオ
			}
			catch(EndOfStreamException){}
			catch(IOException){}

			return i;
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, uint length, out double[]l, out double[] r)
		{
			if(header.IsStereo)
			{
				l = new double[length];
				r = new double[length];
			}
			else
			{
				l = new double[length];
				r = null;
			}
			return WaveReader.Read(reader, header, l, r);
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, float[]l, float[] r)
		{
			uint length = (uint)l.Length;

			int i = 0;
			try
			{
				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)reader.ReadInt16();
						}
					}
				}
				else //ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)(reader.ReadByte() - 128);
							r[i] = (float)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)reader.ReadInt16();
							r[i] = (float)reader.ReadInt16();
						}
					}
				}//ステレオ
			}
			catch(EndOfStreamException){}
			catch(IOException){}

			return i;
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, uint length, out float[]l, out float[] r)
		{
			if(header.IsStereo)
			{
				l = new float[length];
				r = new float[length];
			}
			else
			{
				l = new float[length];
				r = null;
			}
			return WaveReader.Read(reader, header, l, r);
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, short[]l, short[] r)
		{
			uint length = (uint)l.Length;

			int i = 0;
			try
			{
				if(!header.IsStereo) // モノラル
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (short)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (short)reader.ReadInt16();
						}
					}
				}
				else //ステレオ
				{
					if(!header.Is16Bit)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (short)(reader.ReadByte() - 128);
							r[i] = (short)(reader.ReadByte() - 128);
						}
					}
					else
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (short)reader.ReadInt16();
							r[i] = (short)reader.ReadInt16();
						}
					}
				}//ステレオ
			}
			catch(EndOfStreamException){}
			catch(IOException){}

			return i;
		}

		/// <summary>
		/// データ読み出し。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <param name="l">読み出し先配列(L ch)</param>
		/// <param name="r">読み出し先配列(R ch)</param>
		/// <returns>読み出したデータサンプル数</returns>
		public static int Read(BinaryReader reader, FormatHeader header, uint length, out short[]l, out short[] r)
		{
			if(header.IsStereo)
			{
				l = new short[length];
				r = new short[length];
			}
			else
			{
				l = new short[length];
				r = null;
			}
			return WaveReader.Read(reader, header, l, r);
		}

		/// <summary>
		/// データ読み飛ばし。
		/// </summary>
		/// <param name="reader">読み出し元ストリーム</param>
		/// <param name="header">Wave ヘッダ</param>
		/// <param name="length">読み出したい長さ</param>
		/// <returns>ストリームの最後まで達した場合は false</returns>
		public static bool Skip(BinaryReader reader, FormatHeader header, int length)
		{
			try
			{
				int readSize;
				if(!header.IsStereo) // モノラル
					if(!header.Is16Bit) readSize = length;
					else         readSize = length * 2;
				else          // ステレオ
					if(!header.Is16Bit) readSize = length * 2;
				else         readSize = length * 4;
				reader.ReadBytes(readSize);
			}
			catch(EndOfStreamException)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Wave ファイルを開く。
		/// </summary>
		/// <param name="filename">Wave ファイル名</param>
		public void Open(string filename)
		{
			Open(new BinaryReader(new BufferedStream(File.OpenRead(filename))));
		}

		/// <summary>
		/// Wave ファイルを開く。
		/// </summary>
		/// <param name="reader">Wave ファイルを格納したストリーム</param>
		public void Open(BinaryReader reader)
		{
			if(this.reader != null)
			{
				this.reader.Close();
			}

			this.reader = reader;

			// ヘッダ読み出し
			this.header = WaveReader.ReadHeader(reader);

			if(this.header.id != 0x0001)
			{
				throw new WaveException("対応していないフォーマットです。");
			}

			// data chunk 読み出し
			int length = ReadDataChunk(reader);
			this.dataLength = (uint)(length / this.header.blockSize);
		}//Open

		/// <summary>
		/// Wave ファイルを閉じる。
		/// </summary>
		public void Close()
		{
			if(this.reader != null)
			{
				this.reader.Close();
				this.reader = null;
			}
		}

		/// <summary>
		/// ヘッダ情報の取得。
		/// </summary>
		public FormatHeader Header
		{
			get{return this.header;}
		}

		/// <summary>
		/// 残りのデータ長を帰す。
		/// </summary>
		public uint Length
		{
			get{return this.dataLength;}
		}

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(double[] l, double[] r)
		{
			uint length = (uint)l.Length;
			int i = WaveReader.Read(this.reader, this.header, l, r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(float[] l, float[] r)
		{
			uint length = (uint)l.Length;
			int i = WaveReader.Read(this.reader, this.header, l, r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(short[] l, short[] r)
		{
			uint length = (uint)l.Length;
			int i = WaveReader.Read(this.reader, this.header, l, r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="length">読み出すサンプル数。</param>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(uint length, out double[] l, out double[] r)
		{
			l = null;
			r = null;
			if(this.reader == null) return 0;

			int i = WaveReader.Read(this.reader, this.header, length, out l, out r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="length">読み出すサンプル数。</param>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(uint length, out float[] l, out float[] r)
		{
			l = null;
			r = null;
			if(this.reader == null) return 0;

			int i = WaveReader.Read(this.reader, this.header, length, out l, out r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データの読み出し。
		/// </summary>
		/// <param name="length">読み出すサンプル数。</param>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(uint length, out short[] l, out short[] r)
		{
			l = null;
			r = null;
			if(this.reader == null) return 0;

			int i = WaveReader.Read(this.reader, this.header, length, out l, out r);
			this.dataLength -= (uint)i;
			return i;
		}//Read

		/// <summary>
		/// データを読み飛ばす。
		/// </summary>
		/// <param name="length">読み飛ばす長さ</param>
		/// <returns>ファイルの末尾まで到達したら false を返す</returns>
		public bool Skip(int length)
		{
			if(this.reader == null) return false;

			if(!WaveReader.Skip(this.reader, this.header, length))
			{
				this.dataLength = 0;
				return false;
			}

			this.dataLength -= (uint)length;
			return true;
		}//Skip

		/// <summary>
		/// ウェーブデータの先頭に戻る。
		/// </summary>
		public void Restart()
		{
			this.reader.BaseStream.Seek(0, SeekOrigin.Begin);
			WaveReader.ReadDataChunk(this.reader);
		}

		/// <summary>
		/// 1サンプル読み出す。
		/// モノラル16ビット以外の場合、サポート対象外。
		/// </summary>
		/// <returns>1サンプル分のデータ</returns>
		public short ReadShort()
		{
			if(!this.header.Is16Bit || this.header.IsStereo)
				return 0;

			short data = this.reader.ReadInt16();
			--this.dataLength;
			return data;
		}

		/// <summary>
		/// 1サンプル読み出す。
		/// モノラル16ビット以外の場合、サポート対象外。
		/// </summary>
		/// <returns>1サンプル分のデータ</returns>
		public void ReadShort(out short l, out short r)
		{
			System.Diagnostics.Debug.Assert(this.header.Is16Bit);

			if(this.header.IsStereo)
			{
				l = this.reader.ReadInt16();
				r = this.reader.ReadInt16();
				--this.dataLength;
			}
			else
			{
				l = this.reader.ReadInt16();
				r = 0;
				--this.dataLength;
			}
		}
	}//class WaveReader
}
