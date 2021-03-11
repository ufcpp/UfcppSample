## emoji-data 読み

RGI (Recommended for General Interchange、最低限表示できるべきとされている候補)の絵文字シーケンスについて調べもの。
https://github.com/iamcal/emoji-data の emoji.json の中身を眺めてみてるコード。
調査用なので結構適当。

RGI 絵文字シーケンスだけに絞れば [UAX #29](https://unicode.org/reports/tr29/) ほど真面目に書記素判定やらないでよさそう。

- keycaps と国旗だけ特殊なので先に判定
- それを除けば、    
  - Extended_Pictographic は以下の4つで代用できそう
    - 0xA9 (copyright マーク)
    - 0xAE (registered マーク)
    - `> 0x200D and < 0x3300` (Shift JIS 由来の絵文字が大体この辺り。途中にちょっと漢字が混ざってるのでそこは除外してもいいかも)
    - 1F000 台 (キャリア絵文字の辺り。今後絵文字が追加されるとしたら多分全部ここ)
  - Extend は以下の2つしか出てこない
    - 0xFE0F (異体字セレクター16)
    - `>= 0x1F3FB and <= 0x1F3FF` (skin tone、肌色セレクター5文字)

本来の Extended_Pictographic よりもだいぶ多いけど大は小を兼ねるので大丈夫(RGI 判定はテーブルを引くしかないので、テーブルにないものは無視するだけ)。

本来の Extend には他に Nonspacing Mark とかが含まれるけど、絵文字と混ぜてちゃんとレンダリングできる気しないので無視でいいと思う。

see also: https://gist.github.com/ufcpp/e5c2450a7e99175aeb49a184e9e8d50e
