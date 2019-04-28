namespace VS16_1_p2.InterfaceDefault
{
    using System;

    interface IA
    {
        void M() => Console.WriteLine("A.M");
        void N() => Console.WriteLine("A.N");

        struct Inner { }
    }

    interface IB : IA
    {
        new void M() => Console.WriteLine("B.M");
        void IA.N() => Console.WriteLine("B.N");


        static IA.Inner x;
    }

    interface IC : IA
    {
        new void M() => Console.WriteLine("C.M");
        void IA.N() => Console.WriteLine("C.N");
    }

    interface ID : IB, IC
    {
        void IA.N() => base(IB).N();
    }

    class X : IB { }
    class Y : ID { }
    class Z : IB
    {
        public void N() => Console.WriteLine("Z.N");
    }

    class A
    {
        public virtual void M() => Console.WriteLine("A");
    }

    class B : A
    {
        public override void M() => Console.WriteLine("B");
    }

    class C : B
    {
        public override void M() => base(A).M();
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("- X");
            var x = new X();
            M((IA)x); // A.M
            M((IB)x); // B.M
            N((IA)x); // B.N
            N((IB)x); // B.N

            Console.WriteLine("- Y");
            var y = new Y();
            M((IA)y); // A.M
            M((IB)y); // B.M
            M((IC)y); // C.M
            N((IA)y); // B.N
            N((IB)y); // B.N
            N((IC)y); // B.N
            N((ID)y); // B.N

            Console.WriteLine("- Z");
            var z = new Z();
            M((IA)z); // A.M
            M((IB)z); // B.M
            N((IA)z); // Z.N
            N((IB)z); // Z.N
        }

        static bool X(int? x) => x is {} v;

        static void M(IA a) => a.M();
        static void M(IB a) => a.M();
        static void M(IC a) => a.M();
        //static void X(ID a) => a.Y(); // コンパイルエラー: IB, IC で不明瞭

        static void N(IA a) => a.N();
        static void N(IB a) => a.N();
        static void N(IC a) => a.N();
        static void N(ID a) => a.N();
    }
}
