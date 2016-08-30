/*
その他諸々、やっちゃいがちなミス
 */

#include<stdio.h>

int * FunctionWithPointerValue(int n)
{
	int a = n * n;
	return &a;
	/* ↑
	 * ローカル変数のアドレスを返しても、
	 * 関数を抜けた時点でローカル変数の寿命が尽きて消えるので、
	 * 期待通りの結果は得られない。
	 */
}

double BasicLikeArrayAccess(double *x, int i)
{
	return x[i - 1];
	/* ↑
	 * BASIC なんかでは、配列インデックスは 1 から始まって要素数 N で終わる。
	 * C 言語の配列は 0 ～ N-1。
	 * 
	 * 配列のインデックスに関しては、すぐになれてきて、
	 * 1 から始めるミスなんてしなくなると思うけど、
	 * ↑の例みたいに、時々、BASIC 式のインデックスの取り方をする関数があったりするので注意。
	 */
}

void GetInterval(int *points, int n, int *intervals)
{
	int i;
	for (i = 0; i < n - 1; ++i)
	{
		intervals[i] = points[i + 1] - points[i];
	}
	/* ↑
	 * for 中の i < n - 1 に注意。
	 * n 要素の配列の差分は n - 1 要素。
	 */
}

void IncreamentAll(int *array, int n)
{
	//while (--n >= 0) ++*(array++);
	/* ↑
	 * 配列の全要素を ＋1 するコード。
	 * 
	 * 1行でいろんなことをやっていて、かっこよさげに見えるけど、
	 * 人が見たときのことも考えて、もっと見やすいコードを書く方がいい。
	 * 面倒でも、以下のように書くのを推奨。
	 * 

	for (; n >= 0; --n, ++array)
	{
		++(*array);
	}

	 * 
	 * ++ や -- を式の途中で使うのはミスの原因になりやすい。
	 * ++a と a++ の違いにも気をつけなきゃいけなくなるし。
	 */
}

void DoubleLoop()
{
	while (1) /* 外ループ */
	{
		while (1) /* 内ループ */
		{
			break;
		}
	}
	/* ↑
	 * 永久ループ。
	 * break では内ループしか抜けられない。
	 * 
	 * 多重ループから break したければ goto を使わざるを得ない。
	 * あんまりループが深いとこの手のミスが多くなるので、
	 * ループが深くなりすぎるようならループの中身を関数化すべき。
	 */
}

#include <stdlib.h>

void FogetFree()
{
#define LARGE_SIZE 1000000

	int i;
	for (i = 0; i<1000000; ++i)
	{
		int *x = malloc(LARGE_SIZE);

		/*
		free(x);
		 */
	}
	/* ↑
	 * 多分、途中で実行時エラーになるか、OS が凍りかけるはず。
	 * malloc で確保したメモリは、使い終わったら free で解法しないと駄目。
	 * 
	 * free(x) の所のコメントを外せばいくらループをまわしても落ちなくなる。
	 */
}

void FogetInitialize()
{
	int x, y, z;
	printf("%d, %d, %d\n", x, y, z);
	/* ↑
	 * 初期化せずに値を読もうとすると変な値が入っている。
	 * C 言語は、自動で値を初期化してくれたりはしない。
	 * 
	 * 普段からちゃんと初期化する癖を付けて置くように。
	 */
}

int main()
{
	return 0;
}

/*
・演習
main 関数から各関数を呼び出して動作を確認してみよう。



その他:
・ ; と :、. と , など、タイプミスしやすいので注意
・scanf("%d", &x) と書くべきところを scanf("%d",x) と書いてしまうことも多い
 */
