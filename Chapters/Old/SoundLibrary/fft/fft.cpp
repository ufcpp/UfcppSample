/*********************************************************************
fftsg.c の内容を Managed コードから利用するためのラッパー。
fftsg.c 内の rdft (時間領域の実信号→正の周波数成分) を使っています。

[使用方法]
・実フーリエ変換
Fft::Fft(length);
length: 変換元信号の長さ。

Fft::Transform(sgn, a);
sgn: 変換の方向。
     1 で順方向、-1 で逆方向。
a  : 変換元のデータ。

変換前のデータ形式は、
時間領域の実信号
a[k] = x[k];
(0 <= k < N)

変換後のデータ形式は、
a[0]    : 直流成分       Re(X[0])
a[1]    : 最高周波数成分 Re(X[N/2])
a[2*i]  : 成分の実部     Re(X[i])
a[2*i+1]: 成分の虚部     Im(X[i])
(0 < i < N/2)

Fft::Transform(1, x);
の逆変換は
Fft::Transform(-1, x);
for(int i=0; i<N, ++i)
  x[i] *= 2/N;

・複素フーリエ変換
Fft::Fft(length);
length: 変換元信号の長さ。
        (a の長さ ＝ x の長さの2倍)

CFft::Transform(sgn, x);
sgn: 変換の方向。
     1 で順方向、-1 で逆方向。
x  : 変換元のデータ。

変換前のデータ形式は、
a[2*i]  : 成分の実部 Re(x[k])
a[2*i+1]: 成分の虚部 Im(x[k])
(0 <= k < N/2)

変換後のデータ形式は、
a[2*i]  : 成分の実部 Re(X[i])
a[2*i+1]: 成分の虚部 Im(X[i])
(0 < i= < N/2)

CFft::Transform(1, x);
の逆変換は
CFft::Transform(-1, x);
for(int i=0; i<N, ++i)
  x[i] *= 2/N;

 *********************************************************************/

#using <mscorlib.dll>
using namespace System;

extern "C"
{
	void rdft(int n, int isgn, double *a, int *ip, double *w);
	void cdft(int n, int isgn, double *a, int *ip, double *w);
};

namespace Fft
{
	//==================================================================
	// 実フーリエ変換クラス。
	__gc public class Fft
	{
		int length; // FFT の長さ
		double* w; // bit reversal 用ワーク領域
		int* ip;   // sin/cos テーブル用ワーク領域

	public:
		Fft(int length)
		{
			this->length = length;
			this->ip = new int[(int)(2 + Math::Ceiling(Math::Sqrt(length / 2)))];
			this->ip[0] = 0;
			this->w = new double[length / 2];
		}

		void Transform(int sgn, double* x)
		{
			rdft(this->length, sgn, x, this->ip, this->w);
		}
	};

	//==================================================================
	// 複素フーリエ変換クラス。
	__gc public class CFft
	{
		int length; // FFT の長さ
		double* w; // bit reversal 用ワーク領域
		int* ip;   // sin/cos テーブル用ワーク領域

	public:
		CFft(int length)
		{
			this->length = length;
			this->ip = new int[(int)(2 + Math::Ceiling(Math::Sqrt(length / 2)))];
			this->ip[0] = 0;
			this->w = new double[length / 2];
		}

		void Transform(int sgn, double* x)
		{
			cdft(this->length, sgn, x, this->ip, this->w);
		}
	};
};
