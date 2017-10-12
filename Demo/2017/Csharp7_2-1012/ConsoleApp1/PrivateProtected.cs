namespace ConsoleApp1
{
    internal class OtherClass
    {
        public void Method()
        {
            // コメントアウトしてないやつだけ OK
            // public 以外全滅

            var x = new ClassLibrary1.Base();

            x.Public = 0;
            //x.Protected = 0;
            //x.Internal = 0;
            //x.ProtectedInternal = 0;
            //x.Private = 0;
            //x.PrivateProtected = 0;
        }
    }

    public class Derived : ClassLibrary1.Base
    {
        public void MethodInDerived()
        {
            // コメントアウトしてないやつだけ OK

            Public = 0;
            Protected = 0;
            //Internal = 0;
            ProtectedInternal = 0;
            //Private = 0;
            //PrivateProtected = 0; // ここが protected internal との差
        }
    }
}
