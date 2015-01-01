namespace Observable
{
    using System;

    class AnonymousFunctionProbrem
    {
        public event Action X;

        private void RaiseX() => X?.Invoke();

        public void IncorrectSample()
        {
            // 購読開始
            X += () => Console.WriteLine("X");

            // 数回、イベントを起こしてみる
            for (int i = 0; i < 5; i++) RaiseX();

            // 購読解除は、これだと実はできてない
            // () => ... の部分が、それぞれ別オブジェクトになってて、remove できない
            // というか、2か所に書かせるな
            X -= () => Console.WriteLine("X");

            // このイベントは受け取ってしまう。
            for (int i = 0; i < 5; i++) RaiseX();
        }

        public void CorrectSample()
        {
            // こうすればいい。のだけど…
            // どこにでも書けるのが匿名関数(ラムダ式)のいいところなのに、そのよさが台無し
            Action handler = () => Console.WriteLine("X");

            X += handler;

            for (int i = 0; i < 5; i++) RaiseX();

            X -= handler;

            // ちゃんと購読解除されてる
            for (int i = 0; i < 5; i++) RaiseX();
        }
    }
}

namespace Observable
{
    using System;
    using System.Reactive.Disposables;

    class SubscribeAnonymousFunctionProbrem
    {
        public event Action X;

        public IDisposable SubscribeX(Action handler)
        {
            X += handler;
            return Disposable.Create(() => X -= handler);
        }

        private void RaiseX() => X?.Invoke();

        public void CorrectSample()
        {
            // 購読解除は Dispose でやればいい
            using (SubscribeX(() => Console.WriteLine("X")))
            {
                for (int i = 0; i < 5; i++) RaiseX();
            }

            // ちゃんと購読解除されてる
            for (int i = 0; i < 5; i++) RaiseX();
        }
    }
}
