using System;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WebsiteSample
{
	public class RssWriter
	{
		#region 内部クラス

		public class Item
		{
			public readonly string Url;
			public readonly string Title;
			public readonly DateTime UpdatedDateTime;
			public readonly string Digest;

			public Item(string url, string title, DateTime updatedDateTime, string digest)
			{
				Url = url;
				Title = title;
				UpdatedDateTime = updatedDateTime;
				Digest = digest;
			}
		}

		#endregion
		#region 定数

		const string rss10ns = "http://purl.org/rss/1.0/";
		const string rdfns = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
		const string dcns = "http://purl.org/dc/elements/1.1/";

		#endregion
		#region フィールド

		string siteName;
		string adminName;
		string url;

		List<Item> items = new List<Item>();

		#endregion
		#region プロパティ

		/// <summary>
		/// RSS 中に表示されるウェブサイト名。
		/// </summary>
		public string SiteName
		{
			get { return siteName; }
			set { siteName = value; }
		}

		/// <summary>
		/// RSS 中に表示されるサイト管理者名。
		/// </summary>
		public string AdministratorName
		{
			get { return adminName; }
			set { adminName = value; }
		}

		/// <summary>
		/// サイトの URL。
		/// </summary>
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		#endregion
		#region 項目の追加

		/// <summary>
		/// 項目の追加。
		/// </summary>
		/// <param name="item">追加したい項目</param>
		public void Add(Item item)
		{
			this.items.Add(item);
		}

		/// <summary>
		/// 項目の追加。
		/// </summary>
		/// <param name="url">項目の URL</param>
		/// <param name="title">項目のページ/記事タイトル</param>
		/// <param name="updatedDateTime">更新日時</param>
		/// <param name="digest">記事の要約</param>
		public void Add(string url, string title, DateTime updatedDateTime, string digest)
		{
			this.items.Add(new Item(url, title, updatedDateTime, digest));
		}

		public int Count
		{
			get { return this.items.Count; }
		}

		#endregion
		#region 出力

		/// <summary>
		/// RSS フィードを output ストリームに書き出す。
		/// </summary>
		/// <param name="absoluteUri">RSS ファイル自体の URI</param>
		/// <param name="output">RSS の出力先</param>
		public void Write(string absoluteUri, Stream output)
		{
			XmlTextWriter writer = new XmlTextWriter(output, System.Text.Encoding.UTF8);

			try
			{
				writer.WriteStartDocument();
				writer.Formatting = Formatting.Indented;

				writer.WriteStartElement("rdf", "RDF", rdfns);
				writer.WriteAttributeString("xmlns", null, rss10ns);
				writer.WriteAttributeString("xmlns", "dc", null, dcns);
				writer.WriteAttributeString("xml", "lang", null, "ja");

				writer.WriteStartElement("channel", rss10ns);
				writer.WriteAttributeString("about", rdfns, absoluteUri);
				writer.WriteElementString("title", rss10ns, this.siteName + " Update Information");

				writer.WriteElementString("link", rss10ns, this.url);

				writer.WriteElementString("title", dcns, this.siteName);
				writer.WriteElementString("creator", dcns, this.adminName);

				if (items.Count > 0)
				{
					writer.WriteElementString("date", dcns, getRSSDate(items[0].UpdatedDateTime));
				}

				writer.WriteElementString("language", dcns, "ja");
				writer.WriteElementString("description", rss10ns, "このRSSデータは、最近の更新コンテンツです。");

				writer.WriteStartElement("items", rss10ns);
				writer.WriteStartElement("Seq", rdfns);

				foreach (Item item in this.items)
				{
					writer.WriteStartElement("li", rdfns);
					writer.WriteAttributeString("rdf", "resource", item.Url);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();

				foreach (Item item in this.items)
				{
					writer.WriteStartElement("item", rss10ns);
					writer.WriteAttributeString("rdf", "about", rdfns, item.Url);

					writer.WriteElementString("title", rss10ns, item.Title);
					writer.WriteElementString("link", rss10ns, item.Url);
					writer.WriteElementString("description", rss10ns, item.Digest);
					writer.WriteElementString("date", dcns, getRSSDate(item.UpdatedDateTime));

					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			finally
			{
				writer.Flush();
			}
		}

		/// <summary>
		/// 時刻を RSS で使う形式にフォーマット。
		/// </summary>
		/// <param name="datetime">時刻</param>
		/// <returns>フォーマット結果の文字列</returns>
		private string getRSSDate(DateTime datetime)
		{
			DateTime utcTime = datetime.ToUniversalTime();
			return utcTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
		}

		#endregion
	}
}
