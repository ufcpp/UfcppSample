using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text.RegularExpressions;

/// <summary>
/// 指定したフォルダ内にあるすべての XML ファイルに
/// XSL スタイルシートを適用して HTML 化した結果を
/// 元ファイル名の拡張子を xml から html に変えた名前で保存する。
/// </summary>
class XslApplier
{
	/// <summary>
	/// アプリケーションのメイン エントリ ポイントです。
	/// </summary>
	[STAThread]
	static void Main(string[] args)
	{
		if(args.Length == 0)
		{
			Console.Write("フォルダ名を指定してください\n");
		}

		ApplyXslFiles(args[0]);
	}

	/// <summary>
	/// フォルダ内の全ての XML ファイルに XSLT を適用。
	/// </summary>
	static void ApplyXslFiles(string dirName)
	{
		foreach(string subdirName in Directory.GetDirectories(dirName))
		{
			ApplyXslFiles(subdirName);
		}

		foreach(string fileName in Directory.GetFiles(dirName, "*.xml"))
		{
			ApplyXsl(fileName);
		}
	}

	/// <summary>
	/// XML ファイルに XSLT を適用。
	/// </summary>
	static void ApplyXsl(string fileName)
	{
		string xslName = GetXSlUri(fileName);
		if(xslName == null)
			return;
		xslName = Path.GetDirectoryName(fileName) + @"\" + xslName.Replace('/', '\\');

		string htmlName = Path.ChangeExtension(fileName, ".html");

		Console.Write("xml : {0}\nxsl : {1}\nhtml:{2}\n\n", fileName, xslName, htmlName);

		XslTransform xslt = new XslTransform();
		xslt.Load(xslName);

		xslt.Transform(fileName, htmlName);
	}

	/// <summary>
	/// XML 中の <?xml-stylesheet ?> 処理命令から .xsl ファイルの名前を取り出す。
	/// </summary>
	static string GetXSlUri(string fileName)
	{
		XmlDocument xdoc = new XmlDocument();
		xdoc.Load(fileName);

		Regex regType = new Regex("type\\s*=\\s*\"(?<1>[^\\\"]*)\"");
		Regex regHref = new Regex("href\\s*=\\s*\"(?<1>[^\\\"]*)\"");

		foreach(XmlNode node in xdoc.ChildNodes)
		{
			if(
				node.NodeType == XmlNodeType.ProcessingInstruction &&
				node.LocalName == "xml-stylesheet")
			{
				Match m = regType.Match(node.Value);
				if(m.Success && m.Groups[1].Value == "text/xsl")
				{
					m = regHref.Match(node.Value);
					return m.Groups[1].Value;
				}
			}
		}
		return null;
	}
}
