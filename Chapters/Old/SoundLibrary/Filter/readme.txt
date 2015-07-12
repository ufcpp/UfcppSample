//==============================================================================
// 音声フィルタライブラリ

Copyright(C) 2003 Iwanaga Nobuyuki
e-mail   : iwanaga@ise.eng.osaka-u.ac.jp
web page : http://www-ise2.ise.eng.osaka-u.ac.jp/~iwanaga/

//----------------------------------------------------------
// 概要

音声フィルタライブラリです。

フィルタは全て IFilter インタフェースを実装します。
・IFilter
	- double GetValue(double x);
		フィルタリングを行い、その結果を返す。
	- void Clear();
		フィルタの内部状態を初期状態に戻す。



