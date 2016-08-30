using System.Linq;
using System.Dynamic;
using System.Xml.Linq;

namespace Ufcpp.Dynamic
{
    /// <summary>
    /// XML を PowerShell の [xml] 並に動的簡単読み出しするための DynamicObject。
    /// 今のところ読み取り専用。
    /// </summary>
    public class DynamicXml : DynamicObject
    {
        XElement element;

        /// <summary>
        /// XElement を与えて初期化。
        /// </summary>
        /// <param name="element">読み取り対象の XElement。</param>
        public DynamicXml(XElement element) { this.element = element; }

        /// <summary>
        /// XDocument を与えて初期化。
        /// ルート要素を読み出し。
        /// </summary>
        /// <param name="doc">読み取り対象の XDocument。</param>
        public DynamicXml(XDocument doc) : this(doc.Root) { }

        /// <summary>
        /// ファイルのパスを与えて初期化。
        /// </summary>
        /// <param name="uri">読み取り対象の XML ファイル名。</param>
        public DynamicXml(string uri) : this(XDocument.Load(uri)) { }

        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            var name = binder.Name;

            // 属性値は _属性名 で取得。文字列として返す。
            if (name.StartsWith("_"))
            {
                var attName = name.Substring(1);

                result = element.Attribute(attName).Value;
                return true;
            }

            var subElements = element.Elements(name).ToList();

            // 要素がないときは null 返す。
            if (subElements.Count == 0)
            {
                result = (string)null;
                return true;
            }

            // 要素が1個だけの時は素直にその要素を返す。
            if (subElements.Count == 1)
            {
                var e = subElements[0];

                result = new DynamicXml(e);
                return true;
            }

            // 要素が複数ある時はリストで要素一覧を返す。
            var es = subElements.Select(x => new DynamicXml(x));
            result = es.ToList();
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // string へのキャストで、要素の値を取得。
            if (binder.Type == typeof(string))
            {
                result = element.Value;
                return true;
            }
            // int へのキャストで int.Parse。
            // Parse できないときは例外丸投げ。
            if (binder.Type == typeof(int))
            {
                result = int.Parse(element.Value);
                return true;
            }

            // 要素単体に対して foreach やっちゃったときでもエラーにならないように、IEnumerable へのキャストを定義。
            // これやっとかないと、元々複数要素あったのに XML を修正して要素が1個だけになった時に挙動おかしくなる。
            if (binder.Type == typeof(System.Collections.IEnumerable))
            {
                result = new[] { this };
                return true;
            }

            result = null;
            return false;
        }

        public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        {
            switch (binder.Name)
            {
                case "GetEnumerator": // IEnumerable へのキャストと同様の理由。
                    result = new[] { this }.GetEnumerator();
                    return true;

                case "All": // All() 呼び出しで、子要素を全部取得できるようにする。
                    result = element.Elements().Select(x => new DynamicXml(x)).ToList();
                    return true;

                case "Name": // Name() で要素名を取得。
                    result = element.Name.ToString();
                    return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        public override string ToString()
        {
            return element.Value;
        }
    }
}
