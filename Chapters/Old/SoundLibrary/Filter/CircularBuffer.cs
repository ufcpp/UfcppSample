using System;
using System.Collections;

namespace SoundLibrary.Filter
{
#if false
	// ↓ちょっとバグあり。
	// #else の方の実装と比べて10～20％程度高速。
	// 10％程度のためにデバッグするのが面倒で。
	/// <summary>
	/// 循環バッファ
	/// </summary>
	public class CircularBuffer : IEnumerable
	{
		double[] buff;
		int current;
		int length;

		/// <summary>
		/// 循環バッファコンストラクタ。
		/// </summary>
		/// <param name="len">循環バッファの長さ。</param>
		public CircularBuffer(int len)
		{
			this.buff   = new double[2 * len];
			this.length = len;
		}

		/// <summary>
		/// 循環バッファ内の要素のアクセス。
		/// </summary>
		public double this[int n]
		{
			set{this.buff[this.current + n] = value;}
			get{return this.buff[this.current + n];}
		}

		/// <summary>
		/// 循環バッファの末尾に値を挿入。
		/// </summary>
		/// <param name="x">挿入する値。</param>
		public void PushBack(double x)
		{
			this.buff[this.current] = this.buff[this.current + this.length] = x;
			++this.current;
			if(this.current >= this.length) this.current -= this.length;
		}

		/// <summary>
		/// 循環バッファの先頭に値を挿入。
		/// </summary>
		/// <param name="x">挿入する値。</param>
		public void PushFront(double x)
		{
			--this.current;
			if(this.current < 0) this.current += this.length;
			this.buff[this.current] = this.buff[this.current + this.length] = x;
		}

		/// <summary>
		/// 循環バッファの先頭の要素を返す。
		/// </summary>
		public double Top
		{
			get{return this.buff[this.current];}
		}

		/// <summary>
		/// バッファ長(＝this.buff.Length)を返す。
		/// </summary>
		public int Length
		{
			get{return this.length;}
		}

		/// <summary>
		/// this.buff の列挙子を返す。
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return this.buff.GetEnumerator();
		}

		/// <summary>
		/// 循環バッファの長さを変更する。
		/// </summary>
		/// <param name="length">新しい長さ</param>
		public void Resize(int length)
		{
			double[] tmp = new double[2 * length];
			int len = Math.Min(length, this.Length);

			int i=0;
			for(; i<len; ++i)
				tmp[i] = this[i];
			for(; i<length; ++i)
				tmp[i] = 0;
			for(int j=0; j<length; ++i, ++j)
				tmp[i] = tmp[j];
			this.buff = tmp;
		}
	}//class CircularBuffer

#else

	/// <summary>
	/// 循環バッファ
	/// </summary>
	public class CircularBuffer : IEnumerable
	{
		double[] buff;

		/// <summary>
		/// 循環バッファコンストラクタ。
		/// </summary>
		/// <param name="len">循環バッファの長さ。</param>
		public CircularBuffer(int len)
		{
			this.buff = new double[len];
		}

		/// <summary>
		/// 循環バッファ内の要素のアクセス。
		/// </summary>
		public double this[int n]
		{
			set{this.buff[n] = value;}
			get{return this.buff[n];}
		}

		/// <summary>
		/// 循環バッファの末尾に値を挿入。
		/// </summary>
		/// <param name="x">挿入する値。</param>
		public void PushBack(double x)
		{
			for(int i=0; i<this.buff.Length-1; ++i)
			{
				this.buff[i] = this.buff[i+1];
			}
			this.buff[this.buff.Length-1] = x;
		}

		/// <summary>
		/// 循環バッファの先頭に値を挿入。
		/// </summary>
		/// <param name="x">挿入する値。</param>
		public void PushFront(double x)
		{
			for(int i=this.buff.Length-1; i>0; --i)
			{
				this.buff[i] = this.buff[i-1];
			}
			this.buff[0] = x;
		}

		/// <summary>
		/// 循環バッファの先頭の要素を返す。
		/// </summary>
		public double Top
		{
			get{return this.buff[0];}
		}

		/// <summary>
		/// バッファ長(＝this.buff.Length)を返す。
		/// </summary>
		public int Length
		{
			get{return this.buff.Length;}
		}

		/// <summary>
		/// this.buff の列挙子を返す。
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return this.buff.GetEnumerator();
		}

		/// <summary>
		/// 循環バッファの長さを変更する。
		/// </summary>
		/// <param name="length">新しい長さ</param>
		public void Resize(int length)
		{
			double[] tmp = new double[length];
			int len = Math.Min(length, this.Length);
			int i=0;
			for(; i<len; ++i)
				tmp[i] = this[i];
			for(; i<length; ++i)
				tmp[i] = 0;
			this.buff = tmp;
		}

		/// <summary>
		/// 中身を0クリア。
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buff.Length; ++i)
			{
				this.buff[i] = 0;
			}
		}

		/// <summary>
		/// 係数との積和演算。
		/// ∑this[i]*coef[i] を計算。
		/// </summary>
		/// <param name="coef">係数</param>
		/// <returns>積和結果</returns>
		public double Mac(double[] coef)
		{
			int n = coef.Length;
			double sum = 0;
			for(int i=0; i<n; ++i)
			{
				sum += this.buff[i] * coef[i];
			}
			return sum;
		}

		/// <summary>
		/// 係数との積和演算。
		/// ∑this[i + offset]*coef[i] を計算。
		/// </summary>
		/// <param name="offset">畳込みの開始位置オフセット</param>
		/// <param name="coef">係数</param>
		/// <returns>積和結果</returns>
		public double Mac(int offset, double[] coef)
		{
			int n = coef.Length;
			double sum = 0;
			for(int i=0, j=offset; i<n; ++i, ++j)
			{
				sum += this.buff[j] * coef[i];
			}
			return sum;
		}
	}//class CircularBuffer
#endif
}
