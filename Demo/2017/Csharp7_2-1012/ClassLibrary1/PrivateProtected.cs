namespace ClassLibrary1
{
    public class Base
    {
        public int Public { get; set; } // どこからでも
        protected int Protected { get; set; } // 派生クラスからだけ
        internal int Internal { get; set; } // 同一アセンブリ(同一 exe/同一 dll)内からだけ
        protected internal int ProtectedInternal { get; set; } // 派生クラス "もしくは" 同一アセンブリ内 から
        private protected int PrivateProtected { get; set; } // 派生クラス "かつ" 同一アセンブリ内 から(C# 7.2 以降)
        private int Private { get; set; } // クラス内からだけ

        public void Method()
        {
            // 同一クラス内
            // 全部 OK
            Public = 0;
            Protected = 0;
            Internal = 0;
            ProtectedInternal = 0;
            Private = 0;
            PrivateProtected = 0;
        }
    }

    internal class Derived : Base
    {
        public void MethodInDerived()
        {
            // 同一アセンブリ内の派生クラス
            // コメントアウトしてないやつだけ OK
            Public = 0;
            Protected = 0;
            Internal = 0;
            ProtectedInternal = 0;
            //Private = 0;
            PrivateProtected = 0;
        }
    }

    internal class OtherClass
    {
        public void Method()
        {
            // 同一アセンブリ内の他のクラス
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
}
