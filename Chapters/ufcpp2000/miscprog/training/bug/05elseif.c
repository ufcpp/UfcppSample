/*
if と else の対応関係

{} でくくらない場合、
else は一番近い if に対応。

if(a)
if(b) y();
else z();

なら、

if(a)
{
	if(b)
		y();
	else
		z();
}
 */

#include<stdio.h>

int main()
{
	if (0)
	if (0)
		puts("1");
	else if (0)
	if (1)
		puts("2");
	else
		puts("3");
	else
		puts("4");
	else 
		puts("5");
	/* ↑
	 * 1～5、どれが表示されるでしょう？
	 */

	if (0)
	{
		if (0)
		{
			puts("1");
		}
		else if (0)
		{
			if (1)
				puts("2");
			else
				puts("3");
		}
		else
			puts("4");
	}
	else
		puts("5");
	/* ↑
	 * インデントすれば、どれが実行されるかはだいぶ明瞭。
	 * インデントはしっかりと。
	 */

	return 0;
}

/*
・演習
if() 中の 0, 1 をいろいろ変えて実行してみる。
 */
