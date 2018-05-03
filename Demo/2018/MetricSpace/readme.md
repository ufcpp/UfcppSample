# Shapes の実用例: 距離空間上のアルゴリズム

- NonGeneric: 初期状態。汎用性なし
- Virtualized: インターフェイスを切って汎用化した状態。遅い。最終系の4倍くらい遅い
- Devirtualized1: 四則演算を値型ジェネリック化。これで、仮想呼び出しが消えて(devirtualize)、インライン展開が効くようになって倍は速い
- Devirtualized2: 配列を固定長に。配列のヒープ確保がなくなって倍は速い
- Devirtualized3: 距離計算も同様に汎用化。ユークリッド距離以外も使えるように
- Instantiation: 型引数書くのつらいでござる。ごまかし

詳しい説明: [C# にも型クラス(Shapes)が欲しい… 距離空間上のアルゴリズム実装](http://ufcpp.net/blog/2018/5/metricspace/)
