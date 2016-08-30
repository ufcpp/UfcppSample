/*
&& と ||

&& と || はちょっと特殊な演算子。
 */

#include<stdio.h>

int foo(int flag)
{
	puts("foo");
	return flag;
}

int bar(int flag)
{
	puts("bar");
	return flag;
}

int main()
{
	printf("%d\n", foo(0) || bar(0));
	printf("%d\n", foo(1) || bar(0));
	printf("%d\n", foo(0) && bar(0));
	printf("%d\n", foo(1) && bar(0));
	/* ↑
	 * "bar" が表示されるときとされないときがあるはず。
	 * 
	 * && … 左側が 0 だと、その時点で AND の結果が 0 に確定するので、
	 *       右側は実行されない。
	 * || … 同上。
	 *       左側が非 0 だと、右側は実行されない。
	 */

	return 0;
}

/*
|| の性質を使って、

SomeOperation() || exit();
↑
もし、SomeOperation が 0 を返す場合、プログラム終了。

というようなトリッキーなコードを書く人も。
(C 言語ではあまり推奨されない。Perl だとよく見かける。)

・演習
||, を && を |, & に変えてやってみよう。

ちなみに、||, && 以外の演算子の場合、
演算子の左側と右側、どちらが先に実行されるかは
C 言語の規格上決まっていない（どちらでも規格違反ではない）。
 */
