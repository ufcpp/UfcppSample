/*
= と ==
 */

#include<stdio.h>

#define ACTIVE 0
#define DONE 1

int SomeAction()
{
	printf("some action\n");
	return DONE;
}

int main()
{
	int flag = ACTIVE;

	while(flag == ACTIVE)
	{
		flag = SomeAction();
	}

	return 0;
}

/*
・演習1
16行目、 == を = と間違うと・・・
試してみよう。

ちなみに、-Wall オプションとかを付けて警告レベルを上げると怒られるはず。
while とか if の () の中に代入文を直接書くなって。

・演習2
さらに、ACTIVE と DONE の値を 0, 1 逆にしたらどうなるかも試してみよう。

プログラムの強勢終了は ctrl + C。
 */
