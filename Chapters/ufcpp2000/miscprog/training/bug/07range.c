/*
数値の範囲
 */

#include<stdio.h>
#include<math.h>

int main()
{
	char i;
	unsigned int j;

	for (i = 0; i < 300; ++i)
	{
		printf("%d", i);
	}
	/* ↑
	 * i が char 型な事を忘れていると・・・
	 * 
	 * char は 0～255 もしくは -128～127 なので、300 は絶対超えない。
	 * 
	 * 0～255 の場合、255 + 1 は 0 だし、
	 * -128～127 の場合、127 + 1 は -128。
	 * ちなみに、char が符号付になるか符合なしになるかは処理系依存。
	 * 
	 * プログラムの強勢終了は ctrl + C で。
	 */

	for (j = 30; j >= 0; --j)
	{
		printf("%d", j);
	}
	/* ↑
	 * j が 符号なしな事を忘れていると・・・
	 * 
	 * unsigned の場合、0 - 1 は UINT_MAX（unsigend int の最大値）になる。
	 * j >= 0 は常に真。
	 * 
	 * プログラムの強勢終了は ctrl + C で。
	 */

	return 0;
}

/*
・演習
i, j を int に変えてみましょう。
 */
