using System;
using System.IO;

namespace Wave
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
	public class FormatHeader
	{
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

		public FormatHeader(){}

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

		/// <summary>
		/// BinaryReader からヘッダ読み出し。
		/// </summary>
		/// <param name="reader"></param>
		public FormatHeader(BinaryReader reader)
		{
			ReadFromStream(reader);
		}

		/// <summary>
		/// BinaryReader からヘッダ読み出し。
		/// </summary>
		/// <param name="reader"></param>
		public void ReadFromStream(BinaryReader reader)
		{
			this.id         = reader.ReadInt16();
			this.ch         = reader.ReadInt16();
			this.sampleRate = reader.ReadInt32();
			this.dataRate   = reader.ReadInt32();
			this.blockSize  = reader.ReadInt16();
			this.sampleBit  = reader.ReadInt16();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(this.id        );
			writer.Write(this.ch        );
			writer.Write(this.sampleRate);
			writer.Write(this.dataRate  );
			writer.Write(this.blockSize );
			writer.Write(this.sampleBit );
		}
	}//class FormatHeader

	/// <summary>
	/// RIFF Wave 形式のファイルから音声データを読み出す。
	/// </summary>
	public class WaveReader : IDisposable
	{
		BinaryReader reader = null;
		FormatHeader header = null;
		uint dataLength = 0;

		public WaveReader(){}

		public WaveReader(string filename)
		{
			this.Open(filename);
		}

		public WaveReader(BinaryReader reader)
		{
			this.Open(reader);
		}

		public void Dispose()
		{
			this.Close();
		}

		/// <summary>
		/// Wave ファイルを開く。
		/// </summary>
		/// <param name="filename">Wave ファイル名</param>
		public void Open(string filename)
		{
			Open(new BinaryReader(File.OpenRead(filename)));
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
			byte[] buf;

			buf= this.reader.ReadBytes(4);
			if(buf[0] != 'R' || buf[1] != 'I' || buf[2] != 'F' || buf[3] != 'F')
			{
				throw new WaveException("このファイルはRIFF形式ではありません。");
			}

			this.reader.ReadBytes(4); //ファイルサイズ読み飛ばし。

			buf= this.reader.ReadBytes(4);
			if(buf[0] != 'W' || buf[1] != 'A' || buf[2] != 'V' || buf[3] != 'E')
			{
				throw new WaveException("このファイルはwave形式ではありません。");
			}

			buf= this.reader.ReadBytes(4);
			if(buf[0] != 'f' || buf[1] != 'm' || buf[2] != 't' || buf[3] != ' ')
			{
				throw new WaveException("fmtタグが見つかりませんでした。");
			}

			int headerLength = this.reader.ReadInt32();
			if(headerLength < 16)
			{
				throw new WaveException("ヘッダ長が短すぎます。");
			}

			this.header = new FormatHeader(this.reader);
			if(header.id != 0x0001)
			{
				throw new WaveException("対応していないフォーマットです。");
			}

			if(headerLength != 16)
			{
				this.reader.ReadBytes(headerLength - 16); // ヘッダーの残りの部分読み飛ばし。
			}

			buf= this.reader.ReadBytes(4);
			if(buf[0] != 'd' || buf[1] != 'a' || buf[2] != 't' || buf[3] != 'a')
			{
				throw new WaveException("dataタグが見つかりませんでした。");
			}

			this.dataLength = (uint)(this.reader.ReadUInt32() / this.header.blockSize);
		}//Open

		/// <summary>
		/// Wave ファイルを閉じる。
		/// </summary>
		public void Close()
		{
			if(reader == null) return;

			reader.Close();
			reader =null;
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
		/// <param name="length">読み出すサンプル数。</param>
		/// <param name="l">左チャネルのデータ格納先。</param>
		/// <param name="r">右チャネルのデータ格納先。</param>
		/// <returns>実際に読み出したサンプル数。</returns>
		public int Read(uint length, out double[] l, out double[] r)
		{
			l = null;
			r = null;
			if(this.reader == null) return 0;

			int i = 0;
			try
			{
				if(this.header.ch == 1) // モノラル
				{
					l = new double[length];

					if(this.header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)(this.reader.ReadByte() - 128);
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)this.reader.ReadInt16();
						}
					}
				}//モノラル
				else if(header.ch == 2) // ステレオ
				{
					l = new double[length];
					r = new double[length];

					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)(this.reader.ReadByte() - 128);
							r[i] = (double)(this.reader.ReadByte() - 128);
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (double)this.reader.ReadInt16();
							r[i] = (double)this.reader.ReadInt16();
						}
					}
				}//ステレオ
			}
			catch(EndOfStreamException){}
			catch(IOException){}

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

			int i = 0;
			try
			{
				if(this.header.ch == 1) // モノラル
				{
					l = new float[length];

					if(this.header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)(this.reader.ReadByte() - 128);
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)this.reader.ReadInt16();
						}
					}
				}//モノラル
				else if(header.ch == 2) // ステレオ
				{
					l = new float[length];
					r = new float[length];

					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)(this.reader.ReadByte() - 128);
							r[i] = (float)(this.reader.ReadByte() - 128);
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							l[i] = (float)this.reader.ReadInt16();
							r[i] = (float)this.reader.ReadInt16();
						}
					}
				}//ステレオ
			}
			catch(EndOfStreamException){}
			catch(IOException){}

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

			try
			{
				if(header.ch == 1) // モノラル
				{
					if(header.sampleBit == 8)
					{
						this.reader.ReadBytes(length);
					}

					if(header.sampleBit == 16)
					{
						this.reader.ReadBytes(length * 2);
					}
				}
				else if(header.ch == 2) // ステレオ
				{
					if(header.sampleBit == 8)
					{
						this.reader.ReadBytes(length * 2);
					}
					else if(header.sampleBit == 16)
					{
						this.reader.ReadBytes(length * 4);
					}
				}
			}
			catch(EndOfStreamException)
			{
				this.dataLength = 0;
				return false;
			}

			this.dataLength -= (uint)length;
			return true;
		}//Skip
	}//class WaveReader

	/// <summary>
	/// RIFF Wave 形式のファイルに音声データを書き込む。
	/// </summary>
	public class WaveWriter : IDisposable
	{
		BinaryWriter writer = null;
		FormatHeader header = null;
		uint dataLength = 0;

		public WaveWriter(){}

		public WaveWriter(string filename, FormatHeader header)
		{
			this.Open(filename, header);
		}

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
			Open(new BinaryWriter(File.OpenWrite(filename)), header);
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
			byte[] buf;

			buf = System.Text.Encoding.ASCII.GetBytes("RIFF");
			this.writer.Write(buf);

			this.writer.Write((uint)0); //データ長(仮の値を入れておく)

			buf = System.Text.Encoding.ASCII.GetBytes("WAVE");
			this.writer.Write(buf);

			buf = System.Text.Encoding.ASCII.GetBytes("fmt ");
			this.writer.Write(buf);

			this.writer.Write((uint)16);
			this.header.WriteToStream(writer);

			buf = System.Text.Encoding.ASCII.GetBytes("data");
			this.writer.Write(buf);

			this.writer.Write((uint)0); //データ長(仮の値を入れておく)
		}//Open

		/// <summary>
		/// Wave ファイルを閉じる。
		/// </summary>
		public void Close()
		{
			if(writer == null) return;

			uint length = (uint)(this.dataLength * this.header.blockSize);

			writer.Seek(40, SeekOrigin.Begin);
			writer.Write(length);

			writer.Seek(4, SeekOrigin.Begin);
			writer.Write(length + 36u);

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
			int i = 0;

			try
			{
				if(header.ch == 1) // モノラル
				{
					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToByte(l[i]));
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToShort(l[i]));
						}
					}
				}//モノラル
				else if(header.ch == 2) // ステレオ
				{
					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToByte(l[i]));
							writer.Write(this.DoubleToByte(r[i]));
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToShort(l[i]));
							writer.Write(this.DoubleToShort(r[i]));
						}
					}
				}//ステレオ
			}
			catch(IOException){return 0;}
	
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
			int i = 0;

			try
			{
				if(header.ch == 1) // モノラル
				{
					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToByte(l[i]));
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToShort(l[i]));
						}
					}
				}//モノラル
				else if(header.ch == 2) // ステレオ
				{
					if(header.sampleBit == 8)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToByte(l[i]));
							writer.Write(this.DoubleToByte(r[i]));
						}
					}
					else if(header.sampleBit == 16)
					{
						for(i=0; i<length; ++i)
						{
							writer.Write(this.DoubleToShort(l[i]));
							writer.Write(this.DoubleToShort(r[i]));
						}
					}
				}//ステレオ
			}
			catch(IOException){return 0;}
	
			this.dataLength += (uint)i;
			return i;
		}//Write

		/// <summary>
		/// double → byte の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		private byte DoubleToByte(double x)
		{
			x += 128;
			if(x < 0) x = 0;
			else if(x > 255) x = 255;
			return (byte)x;
		}

		/// <summary>
		/// double → short の変換。
		/// </summary>
		/// <param name="x">元</param>
		/// <returns>後</returns>
		private short DoubleToShort(double x)
		{
			if(x < short.MinValue) x = short.MinValue;
			else if(x > short.MaxValue) x = short.MaxValue;
			return (short)x;
		}
	}//class WaveWriter
}//namespace Wave
