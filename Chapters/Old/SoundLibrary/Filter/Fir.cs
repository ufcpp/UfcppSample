using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// 係数配列のプロキシ。
	/// </summary>
	public interface IFirCoefficient : System.Collections.IEnumerable
	{
		/// <summary>
		/// 係数の数を取得。
		/// </summary>
		int Count{get;}

		/// <summary>
		/// i 番目の係数を取得。
		/// </summary>
		double this[int i]{get; set;}
	}

	/// <summary>
	/// FIR フィルタインターフェース。
	/// </summary>
	public interface IFirFilter : IFilter
	{
		/// <summary>
		/// 係数配列を取得。
		/// </summary>
		IFirCoefficient Coefficient{get; set;}
	}

	/// <summary>
	/// FIR フィルタクラス。
	/// </summary>
	public class FirFilter : IFirFilter
	{
		protected double[] coef; // 係数配列
		protected CircularBuffer buff; // 遅延バッファ

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public FirFilter() : this(null){}

		/// <summary>
		/// タップ数を指定して FIR を作る。
		/// </summary>
		/// <param name="taps">タップ数</param>
		public FirFilter(int taps) : this(new double[taps]){}

		/// <summary>
		/// 係数を指定して FIR を作る。
		/// </summary>
		/// <param name="coef">係数を格納した配列。</param>
		public FirFilter(double[] coef)
		{
			this.Coefficient = coef;

			this.Clear();
		}

		/// <summary>
		/// フィルタリングを行う。
		/// N: フィルタ次数 (= this.coef.Length - 1)
		/// x: 入力
		/// y: 出力
		/// c[i]: 係数配列
		/// d[i]: i+1 サンプル前の x の値
		/// とすると、
		/// y = x*c[N] + Σ_{i=0}^{N-1} d[i]*c[N-1-i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			this.buff.PushFront(x);
			int N = this.coef.Length;
			double y = 0;
			for(int i=0; i<N; ++i)
			{
				y += this.buff[i] * this.coef[i];
			}
			return y;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buff.Length; ++i)
			{
				this.buff[i] = 0;
			}
		}

		#region IFirFilter メンバ

		public class CoefficientProxy : IFirCoefficient
		{
			internal double[] x;

			public CoefficientProxy(double[] x){this.x = x;}
			public static implicit operator CoefficientProxy(double[] x){return new CoefficientProxy(x);}

			#region IFirCoefficient メンバ

			public int Count
			{
				get{return this.x.Length;}
			}

			public double this[int i]
			{
				get
				{
					return this.x[i];
				}
				set
				{
					this.x[i] = value;
				}
			}

			#endregion
			#region IEnumerable メンバ

			internal class Enumerator : System.Collections.IEnumerator
			{
				IFirCoefficient x;
				int current;

				public Enumerator(IFirCoefficient x){this.x = x; this.current = -1;}

				#region IEnumerator メンバ

				public void Reset(){this.current = -1;}

				public object Current{get{return this.x[this.current];}}

				public bool MoveNext()
				{
					++this.current;
					return this.current < this.x.Count;
				}

				#endregion
			}

			public System.Collections.IEnumerator GetEnumerator()
			{
				return new Enumerator(this);
			}

			#endregion
		}

		IFirCoefficient IFirFilter.Coefficient
		{
			set{this.Coefficient = ((CoefficientProxy)value).x;}
			get{return new CoefficientProxy(this.Coefficient);}
		}

		/// <summary>
		/// 係数の取得/設定
		/// </summary>
		public double[] Coefficient
		{
			set
			{
				this.coef = value;
				if(value == null)
					this.buff = null;
				else if(this.buff == null)
					this.buff = new CircularBuffer(this.coef.Length);
				else if(this.buff.Length < this.coef.Length)
					this.buff.Resize(this.coef.Length);
			}
			get
			{
				return this.coef;
			}
		}

		#endregion

		public object Clone()
		{
			return new FirFilter((double[])this.coef.Clone());
		}
	}//class FirFilter

	/// <summary>
	/// 線形位相 FIR フィルタクラス。
	/// 係数が実対象であることを利用して計算量/メモリ量削減。
	/// 奇数タップバージョン
	/// (タップ数 2n - 1 で、coef[n-1-i] == coef[n-1+i] foreach i)
	/// </summary>
	public class OddLinearFir : IFirFilter
	{
		protected double[] coef;
		protected CircularBuffer buff;

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public OddLinearFir() : this(null){}

		/// <summary>
		/// タップ数を指定して FIR を作る。
		/// </summary>
		/// <param name="n">タップ数 ＝ 2n - 1</param>
		public OddLinearFir(int n) : this(new double[n]){}

		/// <summary>
		/// 係数を指定して FIR を作る。
		/// </summary>
		/// <param name="coef">係数を格納した配列。</param>
		public OddLinearFir(double[] coef)
		{
			this.Coefficient = coef;

			this.Clear();
		}

		/// <summary>
		/// フィルタリングを行う。
		/// n: タップ数が 2n + 1 (n = this.coef.Length - 1)
		/// x: 入力
		/// y: 出力
		/// c[i]: 係数配列
		/// d[i]: i+1 サンプル前の x の値
		/// とすると、
		/// y = d[n]*c[n] + Σ_{i=1}^{n} (d[n+i] + d[n-i])*c[i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			this.buff.PushFront(x);

			int n = this.coef.Length - 1;
			double y = this.coef[n] * this.buff[n];
			for(int i=0; i<n; ++i)
			{
				y += this.coef[i] * (this.buff[i] + this.buff[2*n-i]);
			}
			return y;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buff.Length; ++i)
			{
				this.buff[i] = 0;
			}
		}

		#region IFirFilter メンバ

		public class CoefficientProxy : IFirCoefficient
		{
			internal double[] x;

			public CoefficientProxy(double[] x){this.x = x;}
			public static implicit operator CoefficientProxy(double[] x){return new CoefficientProxy(x);}

			#region IFirCoefficient メンバ

			public int Count
			{
				get{return 2 * this.x.Length - 1;}
			}

			public double this[int i]
			{
				get
				{
					if(i < this.x.Length)
						return this.x[i];
					else
						return this.x[2 * (this.x.Length - 1) - i];
				}
				set
				{
					if(i < this.x.Length)
						this.x[i] = value;
					else
						this.x[2 * (this.x.Length - 1) - i] = value;
				}
			}

			#endregion
			#region IEnumerable メンバ

			public System.Collections.IEnumerator GetEnumerator()
			{
				return new FirFilter.CoefficientProxy.Enumerator(this);
			}

			#endregion
		}

		IFirCoefficient IFirFilter.Coefficient
		{
			set{this.Coefficient = ((CoefficientProxy)value).x;}
			get{return new CoefficientProxy(this.Coefficient);}
		}

		/// <summary>
		/// 係数の取得/設定
		/// </summary>
		public double[] Coefficient
		{
			set
			{
				this.coef = value;
				if(value == null)
					this.buff = null;
				else if(this.buff == null)
					this.buff = new CircularBuffer(2 * this.coef.Length - 1);
				else if(this.buff.Length < 2 * this.coef.Length - 1)
					this.buff.Resize(2 * this.coef.Length - 1);
			}
			get
			{
				return this.coef;
			}
		}

		#endregion

		public object Clone()
		{
			return new OddLinearFir((double[])this.coef.Clone());
		}
	}//class OddLinearFir

	/// <summary>
	/// 線形位相 FIR フィルタクラス。
	/// 係数が実対象であることを利用して計算量/メモリ量削減。
	/// 偶数タップバージョン
	/// (タップ数 2n で、coef[n-1-i] == coef[n+i] foreach i)
	/// </summary>
	public class EvenLinearFir : IFirFilter
	{
		protected double[] coef;
		protected CircularBuffer buff;

		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public EvenLinearFir() : this(null){}

		/// <summary>
		/// タップ数を指定して FIR を作る。
		/// </summary>
		/// <param name="n">タップ数 ＝ 2n - 1</param>
		public EvenLinearFir(int n) : this(new double[n]){}

		/// <summary>
		/// 係数を指定して FIR を作る。
		/// </summary>
		/// <param name="coef">係数を格納した配列。</param>
		public EvenLinearFir(double[] coef)
		{
			this.Coefficient = coef;

			this.Clear();
		}

		/// <summary>
		/// フィルタリングを行う。
		/// n: タップ数が 2n (n = this.coef.Length)
		/// x: 入力
		/// y: 出力
		/// c[i]: 係数配列
		/// d[i]: i+1 サンプル前の x の値
		/// とすると、
		/// y = Σ_{i=1}^{n} (d[n+i] + d[n-1-i])*c[i]
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			this.buff.PushFront(x);

			int n = this.coef.Length;
			double y = 0;
			for(int i=0; i<n; ++i)
			{
				y += this.coef[i] * (this.buff[i] + this.buff[2*n-1-i]);
			}
			return y;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buff.Length; ++i)
			{
				this.buff[i] = 0;
			}
		}

		#region IFirFilter メンバ

		public class CoefficientProxy : IFirCoefficient
		{
			internal double[] x;

			public CoefficientProxy(double[] x){this.x = x;}
			public static implicit operator CoefficientProxy(double[] x){return new CoefficientProxy(x);}

			#region IFirCoefficient メンバ

			public int Count
			{
				get{return 2 * this.x.Length;}
			}

			public double this[int i]
			{
				get
				{
					if(i < this.x.Length)
						return this.x[i];
					else
						return this.x[2 * this.x.Length - 1 - i];
				}
				set
				{
					if(i < this.x.Length)
						this.x[i] = value;
					else
						this.x[2 * this.x.Length - 1 - i] = value;
				}
			}

			#endregion
			#region IEnumerable メンバ

			public System.Collections.IEnumerator GetEnumerator()
			{
				return new FirFilter.CoefficientProxy.Enumerator(this);
			}

			#endregion
		}

		IFirCoefficient IFirFilter.Coefficient
		{
			set{this.Coefficient = ((CoefficientProxy)value).x;}
			get{return new CoefficientProxy(this.Coefficient);}
		}

		/// <summary>
		/// 係数の取得/設定
		/// </summary>
		public double[] Coefficient
		{
			set
			{
				this.coef = value;
				if(value == null)
					this.buff = null;
				else if(this.buff == null)
					this.buff = new CircularBuffer(2 * this.coef.Length);
				else if(this.buff.Length < 2 * this.coef.Length)
					this.buff.Resize(2 * this.coef.Length);
			}
			get
			{
				return this.coef;
			}
		}

		#endregion

		public object Clone()
		{
			return new EvenLinearFir((double[])this.coef.Clone());
		}
	}//class EvenLinearFir

}//namespace Filter
