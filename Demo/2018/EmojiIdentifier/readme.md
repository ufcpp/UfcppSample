# Emoji 識別子

Swift が絵文字識別子を使えるようになったって頃にいろいろ試してたやつ。
そこら中に書き捨ててたけど、最近ロスト気味なので1か所に集めとく。

## 色付き❤

[❤🧡💛💙💚💜🖤](http://ufcppfree.azurewebsites.net/Grapheme?s=%E2%9D%A4%F0%9F%A7%A1%F0%9F%92%9B%F0%9F%92%99%F0%9F%92%9A%F0%9F%92%9C%F0%9F%96%A4)

```swift
var 💜 = 2;
var 💛 = 3;
var 💚 = 5;
var 💙 = 7;
var 🖤 = 11;
var 🧡 = 13;
print(💙 * 💚 * 💛 * 💜 * 🖤 * 🧡)
```

カラーフォントに対応している環境だといいけど、カラー表示できないエディターだと…

![Color Hearts](ColorHearts.png)

人の絵文字の肌色セレクターでも似たようなことできる。

## シグマ

```swift
prefix operator ∑ // 'N-ARY SUMMATION' (U+2211)

prefix func ∑ (array : [Int]) -> Int {
    var sum : Int = 0
    for item in array {
        sum += item
    }
    return sum
}

let a = [ 1, 2, 3, 4, 5]
let Σa = 0 // 'GREEK CAPITAL LETTER SIGMA' (U+03A3) + a

print(∑a) // 15
print(Σa) // 0
```

N-ARY Sum は演算子として使えて、ギリシャ文字のΣは識別子として使える。
∑aは「aの総和」、Σaは「識別子Σa」になる。

## 書式付き数字

[𝟎𝟘𝟢𝟬𝟶](http://ufcppfree.azurewebsites.net/Grapheme?s=%F0%9D%9F%8E%F0%9D%9F%98%F0%9D%9F%A2%F0%9D%9F%AC%F0%9D%9F%B6)

```swift
var 𝟎 = 2;
var 𝟘 = 3;
var 𝟢 = 5;
var 𝟬 = 7;
var 𝟶 = 11;
print(𝟎 * 𝟘 * 𝟢 * 𝟬 * 𝟶)
```

いわゆる Mathematical Alphanumeric Symbols。
上から順に、Bold, Double-struck, Sans-serif, Sans-serif bold, Mono-space。
この辺りの Unicode カテゴリー、Nd (decimal digits)なんだけど、Swift はサロゲートペアな文字を全部識別子として受け付ける実装なのでこういうことになる。

フォントによってはほんとに 0 と区別がつかないので、0 なのに 0 じゃないみたいなことができる。
1~9にも同様の文字あり。

## 不可視演算子

[全く見えないけども、文字は入ってる](http://ufcppfree.azurewebsites.net/Grapheme?s=%E2%81%A1%E2%81%A2%E2%81%A3%E2%81%A4)

```swift
var ⁡ = 2
var ⁢ = 3
var ⁣ = 5
var ⁤ = 7
print(⁡ * ⁢ * ⁣ * ⁤)
```

上から順に、

- Function Apply … f(x) を fx みたいに書いたりするやつ。あるいは、微分演算子を (d/dx)f みたいに書くやつ。この関数と変数、演算子と関数の関係を表す
- Invisible Times … xy で「x 掛ける y」を表すこと多い。この「不可視の×」を表す
- Invisible Separator … a<sub>ij</sub>みたいに書くとき、このijはi×jじゃなくて、単にi, jの意味。この不可視の , を表す
- Invisible Plus … 帯分数だと、1 (2/3) みたいに書いて 1 + (2 / 3) と考える。この場合の「不可視の+」を表す

Unicode のコードポイント的にはU+2061~2063で、カテゴリーは Cf (Other Format)。なぜか識別子に使える。

ちなみに、フォントによっては見える。

![不可視演算子が見えるフォント](InvisibleOperators.png)
