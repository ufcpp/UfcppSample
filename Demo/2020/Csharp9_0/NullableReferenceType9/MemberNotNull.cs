#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace NullableReferenceType9.MemberNotNull
{
    class Sample
    {
        /// <summary>
        /// string (? が付いていないので非 null)なプロパティは、#nullable enable コンテキスト下では初期化が必須。
        /// 初期化しないと警告あり。
        /// </summary>
        public string Text { get; private set; }

#if false
        // こう書くなら警告が消える
        public Sample(string text)
        {
            Text = text;
        }
#endif

#if false
        public Sample()
        {
            // InitText の中で Text プロパティを非 null 初期化してるので認めてほしいものの…
            // この状態だとその判定ができなくて警告が消えない。
            InitText();
        }

        private void InitText()
        {
            Text = "non-null string";
        }
#endif

        public Sample()
        {
            // これなら警告が消える。
            InitText();
        }

        // MemberNotNull 属性を付けることで、このメソッド内で Text プロパティを初期化している保証が付く。
        [MemberNotNull(nameof(Text))]
        private void InitText()
        {
            Text = "non-null string";
        }
    }
}
