namespace ClassLibrary1
{
    public class Base
    {
        // どこからでも
        public int Public { get; set; }

        // 派生クラスからだけ
        protected int Protected { get; set; }

        // 同一アセンブリ(同一 exe/同一 dll)内からだけ
        internal int Internal { get; set; }

        // 派生クラス "もしくは" 同一アセンブリ内 から
        protected internal int ProtectedInternal { get; set; }

        // クラス内からだけ
        private int Private { get; set; }

        // [new!] C# 7.2
        // 派生クラス "かつ" 同一アセンブリ内 から
        private protected int PrivateProtected { get; set; }

        public void Method()
        {
            // 全部 OK
            Public = 0;
            Protected = 0;
            Internal = 0;
            ProtectedInternal = 0;
            Private = 0;
            PrivateProtected = 0;
        }
    }

    internal class OtherClass
    {
        public void Method()
        {
            // コメントアウトしてないやつだけ OK
            var x = new Base();

            x.Public = 0;
            //x.Protected = 0;
            x.Internal = 0;
            x.ProtectedInternal = 0;
            //x.Private = 0;
            //x.PrivateProtected = 0;
        }
    }

    internal class Derived : Base
    {
        public void MethodInDerived()
        {
            // コメントアウトしてないやつだけ OK
            Public = 0;
            Protected = 0;
            Internal = 0;
            ProtectedInternal = 0;
            //Private = 0;
            PrivateProtected = 0;
        }
    }
}
