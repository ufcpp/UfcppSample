using System;
using System.Collections;

namespace Filter
{
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
		/// 循環バッファの末尾に値を挿入。
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
	}//class CircularBuffer
}
