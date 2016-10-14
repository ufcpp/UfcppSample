何とかジェネリックな実装でもヒープ確保なしで動かせないかと苦心した結果。

使う側が、

```cs
using Utf8String = UtfString.Generic.String<byte, UtfString.Generic.ByteAccessor, UtfString.Generic.Utf8Decoder>;
using Utf16String = UtfString.Generic.String<ushort, UtfString.Generic.ShortAccessor, UtfString.Generic.Utf16Decoder<UtfString.Generic.ShortAccessor>>;
```

みたいになるので結構微妙。
あと、単体テストプロジェクトがコンパイル エラー起こすようになってテストできてない。
