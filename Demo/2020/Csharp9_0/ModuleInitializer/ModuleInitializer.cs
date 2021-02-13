#if true

// 方式3: モジュール初期化子で登録

using System;
using System.Runtime.CompilerServices;

// モジュール初期化子の場合、その型を含むモジュール(dll とか exe とか)がロードされた直後に必ず呼ばれる。
// 静的コンストラクターの「型に触れた瞬間」よりは確実に呼ばれる保証あり。
Console.WriteLine(TypeRepository.CreateInstance("A")); // A
Console.WriteLine(TypeRepository.CreateInstance("B")); // B

// 静的コンストラクターだと呼ばれるタイミングが不定で問題があったけど、モジュール初期化子なら大丈夫。
class A
{
    [ModuleInitializer]
    public static void Init() => TypeRepository.Register(nameof(A), () => new A());
}

class B
{
    [ModuleInitializer]
    public static void Init() => TypeRepository.Register(nameof(B), () => new B());
}

#endif
