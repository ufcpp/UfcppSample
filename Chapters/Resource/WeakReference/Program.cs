using System;
using System.Reflection;

namespace WeakReferenceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // サンプル掲載用に、Version* フォルダー以下の Program も単体で動くように作ってあって、
            // そっちにも private な Main メソッドがある。
            // ・ビルド設定で「スタートアップ オブジェクト」をこっちのクラスに設定してある。
            // ・あっちの private Main はリフレクションで無理やり呼ぶ。

            //InvokeMain(typeof(HowToUse.Program));
            //InvokeMain(typeof(WeakTable.Program));
            InvokeMain(typeof(WeakEvent.Program));
        }

        static void InvokeMain(Type t)
        {
            t.GetMethod(nameof(Main), BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, null);
        }
    }
}
