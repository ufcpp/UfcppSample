using System;
using System.Collections;
using System.Xml;
using System.Reflection;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// フィルタの構成情報。
	/// </summary>
	public class FilterProperty
	{
		string name;
		object obj;
		Type type;

		internal FilterProperty(string name, object obj)
		{
			this.name = name;
			this.obj = obj;
			this.type = obj.GetType();
		}

		/// <summary>
		/// プロパティの値を取得・設定。
		/// </summary>
		public object Value
		{
			set
			{
				if(value == null)
					throw new ArgumentNullException("null は設定できません");

				if(!this.type.IsAssignableFrom(value.GetType()))
					throw new ArgumentException("型が一致していません");

				this.obj = value;
			}
			get
			{
				return this.obj;
			}
		}

		/// <summary>
		/// プロパティ名を取得。
		/// </summary>
		public string Name()
		{
			return this.name;
		}

		/// <summary>
		/// プロパティの型を取得。
		/// </summary>
		public Type Type()
		{
			return this.type;
		}
	}

	/// <summary>
	/// フィルタの配列型構成情報
	/// </summary>
	public class FilterArrayProperty
	{
		public struct Tuple
		{
			public string name;
			public Type type;

			public Tuple(string name, Type type)
			{
				this.name = name;
				this.type = type;
			}
		}

		Tuple[] informations;
		ArrayList propertyList;

		public FilterArrayProperty(params Tuple[] informations)
		{
			this.informations = informations;
			this.propertyList = new ArrayList();
		}

		/// <summary>
		/// i 番目の配列属性の j 番目の属性を取得/設定する。
		/// </summary>
		public object this[int i, int j]
		{
			set
			{
				if(value == null)
					throw new ArgumentNullException("null は設定できません");

				if(!this.informations[i].type.IsAssignableFrom(value.GetType()))
					throw new ArgumentException("型が一致していません");

				((object[])this.propertyList[i])[j] = value;
			}
			get
			{
				return ((object[])this.propertyList[i])[j];
			}
		}

		/// <summary>
		/// 構成情報を追加する。
		/// </summary>
		/// <param name="objs">項目の属性</param>
		public void Add(params object[] objs)
		{
			if(objs == null)
				throw new ArgumentNullException("null は設定できません");

			if(this.informations.Length != objs.Length)
				throw new ArgumentException("長さが一致していません");

			for(int i=0; i<objs.Length; ++i)
			{
				if(!this.informations[i].type.IsAssignableFrom(objs[i].GetType()))
					throw new ArgumentException("型が一致していません");
			}

			this.propertyList.Add(objs);
		}

		/// <summary>
		/// 構成情報を削除する。
		/// </summary>
		/// <param name="i">削除位置</param>
		public void Remove(int i)
		{
			this.propertyList.RemoveAt(i);
		}

		/// <summary>
		/// リストの長さ。
		/// </summary>
		public int ListLength
		{
			get{return this.propertyList.Count;}
		}

		/// <summary>
		/// 構成情報の数を取得する。
		/// </summary>
		public int Count
		{
			get{return this.informations.Length;}
		}

		/// <summary>
		/// プロパティ名を取得。
		/// </summary>
		public string Name(int i)
		{
			return this.informations[i].name;
		}

		/// <summary>
		/// プロパティの型を取得。
		/// </summary>
		public Type Type(int i)
		{
			return this.informations[i].type;
		}
	}

	/// <summary>
	/// フィルタ作成クラスの共通部分を集めた抽象基底クラス。
	/// プロパティの管理部分はこのクラスでする。
	/// </summary>
	public abstract class FilterGenerator
	{
		/// <summary>
		/// フィルタの構成情報。
		/// Amplifier の gain とか、FirFilter の coef とか、
		/// 普通の構成情報。
		/// </summary>
		protected FilterProperty[] properties = null;

		/// <summary>
		/// リストになっている構成情報(配列型構成情報と呼ぶことにする)。
		/// SerialConnector の (filter) とか Mixer の (gain, filter)とか、
		/// 可変長のもの構成情報。
		/// </summary>
		protected FilterArrayProperty[] arrayProperties = null;

		/// <summary>
		/// 数値変換用。
		/// </summary>
		protected Converter converter = new Converter();

		/// <summary>
		/// 構成情報の数を取得する。
		/// </summary>
		public int Count
		{
			get
			{
				if(this.properties == null)
					return 0;
				else
					return this.properties.Length;
			}
		}

		/// <summary>
		/// フィルタの構成情報を取得する。
		/// </summary>
		[System.Runtime.CompilerServices.IndexerName("Property")]
		public FilterProperty this[int i]
		{
			get
			{
				return this.properties[i];
			}
		}

		/// <summary>
		/// 配列型構成情報の数を取得する。
		/// </summary>
		public int ArrayCount
		{
			get
			{
				if(this.arrayProperties == null)
					return 0;
				else
					return this.arrayProperties.Length;
			}
		}

		/// <summary>
		/// フィルタの配列型構成情報を取得する。
		/// </summary>
		public FilterArrayProperty GetArrayProperty(int i)
		{
			return this.arrayProperties[i];
		}

		/// <summary>
		/// フィルタを作成する。
		/// </summary>
		/// <returns>作成したフィルタ</returns>
		public abstract IFilter GetFilter();

		/// <summary>
		/// 設定した属性がフィルタの制約を満たしているかどうかチェックする。
		/// </summary>
		/// <returns>制約を満たしていれば null、満たしていなければエラーメッセージを返す</returns>
		public abstract string CheckConstraint();

		/// <summary>
		/// XML にフィルタ構成を出力。
		/// </summary>
		/// <param name="xwriter">出力先</param>
		public abstract void ToXml(XmlWriter xwriter);

		/// <summary>
		/// XML からフィルタ構成を入力。
		/// </summary>
		/// <param name="elem">入力元</param>
		public abstract void FromXml(XmlElement elem);

		/// <summary>
		/// フィルタ構成を XML 形式でファイル出力。
		/// </summary>
		/// <param name="filename">XML ファイル名</param>
		public void WriteXml(string filename)
		{
			XmlTextWriter xwriter = new XmlTextWriter(filename, System.Text.Encoding.Default);
			xwriter.WriteStartDocument();
			this.ToXml(xwriter);
			xwriter.WriteEndDocument();
			xwriter.Close();
		}

		/// <summary>
		/// XML ファイル内のフィルタ構成情報から FilterGenerator を作成。
		/// </summary>
		/// <param name="filename">XML ファイル名</param>
		/// <returns>作成した FilterGenerator</returns>
		public static FilterGenerator CreateFromXml(string filename)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(filename);
			FilterGenerator gen = CreateFromXml(xdoc.DocumentElement);
			return gen;
		}

		/// <summary>
		/// XML ファイル内のフィルタ構成情報から FilterGenerator を作成。
		/// </summary>
		/// <param name="filename">XML ファイル名</param>
		/// <returns>作成した FilterGenerator</returns>
		public static FilterGenerator CreateFromXml(string filename, Converter converter)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(filename);
			FilterGenerator gen = CreateFromXml(xdoc.DocumentElement, converter);
			return gen;
		}

		/// <summary>
		/// XML から FilterGenerator を作成。
		/// </summary>
		/// <param name="xreader">入力</param>
		/// <returns>作成した FilterGenerator</returns>
		internal static FilterGenerator CreateFromXml(XmlElement elem)
		{
			return FilterGenerator.CreateFromXml(elem, null);
		}

		/// <summary>
		/// XML から FilterGenerator を作成。
		/// </summary>
		/// <param name="xreader">入力</param>
		/// <returns>作成した FilterGenerator</returns>
		internal static FilterGenerator CreateFromXml(XmlElement elem, Converter converter)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			FilterGenerator gen = (FilterGenerator)asm.CreateInstance("SoundLibrary.Filter." + elem.LocalName + "Generator");

			if(gen == null)
				throw new NotSupportedException("SoundLibrary.Filter." + elem.LocalName + " は実装されていません。");

			if(converter != null)
				gen.converter = converter;

			XmlAttribute att;
			att = elem.Attributes["samplingRate"];
			if(att != null)
				gen.SamplingRate = double.Parse(att.Value);
			att = elem.Attributes["dB"];
			if(att != null)
				gen.IsDB = att.Value == "true";

			gen.FromXml(elem);
			return gen;
		}

		/// <summary>
		/// IsDB が真のとき、入力された文字列は dB 値を表してるものとして、
		/// dB → リニア値の変換を行う。
		/// </summary>
		public bool IsDB
		{
			set{this.converter.IsDB = value;}
			get{return this.converter.IsDB;}
		}

		/// <summary>
		/// SamplingRate が非 0 のとき、その周波数で正規化を行う。
		/// </summary>
		public double SamplingRate
		{
			set{this.converter.SamplingRate = value;}
			get{return this.converter.SamplingRate;}
		}
	}//class FilterGenerator

	public class Util
	{
		public static string ArrayToString(double[] array)
		{
			string str = array[0].ToString();
			for(int i=1; i<array.Length; ++i)
				str += ',' + array[i].ToString();
			return str;
		}

		public static double[] StringToArray(string str)
		{
			string[] tokens = str.Split(',');
			double[] array = new double[tokens.Length];
			for(int i=0; i<tokens.Length; ++i)
				array[i] = double.Parse(tokens[i]);
			return array;
		}
	}

	/// <summary>
	/// 文字列→数値変換クラス。
	/// dB → リニア値の変換や、周波数の正規かも行う。
	/// </summary>
	public class Converter
	{
		bool isDB = false;
		double samplingRate = 0;

		/// <summary>
		/// IsDB が真のとき、入力された文字列は dB 値を表してるものとして、
		/// dB → リニア値の変換を行う。
		/// </summary>
		public bool IsDB
		{
			set{this.isDB = value;}
			get{return this.isDB;}
		}

		/// <summary>
		/// SamplingRate が非 0 のとき、その周波数で正規化を行う。
		/// </summary>
		public double SamplingRate
		{
			set{this.samplingRate = value;}
			get{return this.samplingRate;}
		}

		/// <summary>
		/// 文字列→数値変換。振幅版。
		/// IsDB の値に応じて数値を変換する。
		/// </summary>
		/// <param name="str">変換元</param>
		/// <returns>変換結果</returns>
		public double ToPower(string str)
		{
			double x = double.Parse(str);

			if(this.isDB)
				return SoundLibrary.Util.DBToLinear(x);

			return x;
		}

		/// <summary>
		/// 文字列→数値変換。周波数版。
		/// SamplingRate の値に応じて数値を変換する。
		/// </summary>
		/// <param name="str">変換元</param>
		/// <returns>変換結果</returns>
		public double ToFrequency(string str)
		{
			double x = double.Parse(str);

			if(this.samplingRate != 0)
				return SoundLibrary.Util.Normalize(x, this.samplingRate);

			return x;
		}
	}
}//namespace FilterGenerator
