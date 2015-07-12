using System;
using System.IO;
using System.Text;
using System.Collections;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// データリストの項目。
	/// </summary>
	public class DataItem
	{
		public string title;
		public double[] data;

		public DataItem(string title, double[] data)
		{
			this.title = title;
			this.data = data;
		}
	}

	/// <summary>
	/// 特性データのリスト。
	/// </summary>
	public class DataList
	{
		ArrayList list = new ArrayList();
		int count = -1;

		/// <summary>
		/// 項目の追加
		/// </summary>
		/// <param name="title">項目名</param>
		/// <param name="data">データ</param>
		public void Add(string title, double[] data)
		{
			if(this.count == -1) this.count = data.Length;
			else if(this.count != data.Length) return;

			list.Add(new DataItem(title, data));
		}

		/// <summary>
		/// データリストを CSV 形式でファイルに保存。
		/// </summary>
		/// <param name="writer">書き込み先</param>
		/// <param name="delim">区切り文字</param>
		/// <param name="outputTitle">ヘッダ行を出力するかどうか</param>
		public void Save(StreamWriter writer, char delim, bool outputTitle)
		{
			if(this.count == -1) return;

			if(outputTitle)
			{
				int i=0;
				for(; i<list.Count-1; ++i)
				{
					writer.Write("{0}{1}", ((DataItem)this.list[i]).title, delim);
				}
				writer.Write("{0}\n", ((DataItem)this.list[i]).title);
			}

			for(int j=0; j<this.count; ++j)
			{
				int i=0;
				for(; i<list.Count-1; ++i)
				{
					writer.Write("{0}{1}", ((DataItem)this.list[i]).data[j], delim);
				}
				writer.Write("{0}\n", ((DataItem)this.list[i]).data[j]);
			}
		}//Save

		/// <summary>
		/// データリストを CSV 形式でファイルに保存。
		/// </summary>
		/// <param name="writer">書き込み先</param>
		/// <param name="delim">区切り文字</param>
		public void Save(StreamWriter writer, char delim)
		{
			this.Save(writer, delim, false);
		}

		/// <summary>
		/// データリストを CSV 形式でファイルに保存。
		/// </summary>
		/// <param name="filename">出力ファイル名</param>
		/// <param name="delim">区切り文字</param>
		/// <param name="outputTitle">ヘッダ行を出力するかどうか</param>
		public void Save(string filename, char delim, bool outputHeader)
		{
			using(StreamWriter writer = new StreamWriter(filename, false, Encoding.Default))
			{
				this.Save(writer, delim, outputHeader);
			}
		}

		/// <summary>
		/// データリストを CSV 形式でファイルに保存。
		/// </summary>
		/// <param name="filename">出力ファイル名</param>
		/// <param name="delim">区切り文字</param>
		public void Save(string filename, char delim)
		{
			this.Save(filename, delim, false);
		}
	}//class DataList
}//namespace WaveAnalysis
