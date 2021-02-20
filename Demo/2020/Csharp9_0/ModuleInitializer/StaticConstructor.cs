#if false

// 方式2: 静的コンストラクターで登録

using System;
using System.Collections.Generic;

// 後述するように、静的コンストラクターはこの用途だと呼ばれない。
// なので、Register が呼ばれてなくて、CreateInstance が null を返す。
Console.WriteLine(TypeRepository.CreateInstance("A")); // null
Console.WriteLine(TypeRepository.CreateInstance("B")); // null

// これが例えば、どこでもいいから1度 A のメンバーを空呼びすると上記コードがちゃんと new A(), new B() を返すようになる。
// 静的コンストラクターが呼ばれるタイミングは「その型のメンバーを最初に使った直後」
_ = new A(); // このタイミングで A の静的コンストラクターが呼ばれる
_ = new B(); // このタイミングで B の静的コンストラクターが呼ばれる
Console.WriteLine(TypeRepository.CreateInstance("A")); // A
Console.WriteLine(TypeRepository.CreateInstance("B")); // B

// あまり手書きはしたくないものの、Source Generator がある今、
// 必要な型に対して以下のようなコード生成をするのは十分現実的。
// ただ、静的コンストラクターは呼ばれるタイミングに問題があって…
class A
{
    static A() => TypeRepository.Register(nameof(A), () => new A());
}

class B
{
    static B() => TypeRepository.Register(nameof(B), () => new B());
}

#endif
