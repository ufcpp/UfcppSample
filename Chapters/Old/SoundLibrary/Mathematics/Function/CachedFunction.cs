using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 関数クラスにキャッシュ機能を付ける。
	/// f[Exp(x)] とかすると、f の中で、Exp の値が何度も計算される場合があるので。
	/// 一度 Exp(x) の値を計算したら、x の値が同じ限り極力値を使いまわす。
	/// </summary>
	public class CachedFunction : Function
	{
		#region 内部クラス

		class Cache
		{
			static int count; //! for debug
			int unique_id;
			public Cache(){this.unique_id = count; ++count;}

			public Parameter[] bind_x = null;
			public Function    bind   = null;
			public Parameter[] gv_x   = null;
			public ValueType   gv     = 0;
			public Function    re     = null;
			public Function    im     = null;
		}

		#endregion
		#region フィールド

		/// <summary>
		/// 関数の実体。
		/// </summary>
		Function f;

		/// <summary>
		/// キャッシュ値。
		/// </summary>
		Cache cache;

		#endregion
		#region 初期化

		static int count; //! for debug
		int unique_id;

		public CachedFunction(Function f) : this(f, new Cache()) {}

		CachedFunction(Function f, Cache c)
		{
			this.f = f;
			this.cache = c;

			this.unique_id = count; ++count;
		}

		#endregion
		#region Parameter[] の等値判定

		static bool ParameterEquals(Parameter[] a, Parameter[] b)
		{
			return a == b;
			/*
			if(a == null || b == null) return false;
			if(a.Length != b.Length) return false;

			for(int i=0; i<a.Length; ++i)
				if(!a[i].Equals(b[i])) return false;

			return true;
			*/
		}

		#endregion
		#region Bind

		public override Function Bind(params Parameter[] x)
		{
			if(!ParameterEquals(x, this.cache.bind_x))
			{
				this.cache.bind_x = x;
				this.cache.bind = new CachedFunction(this.f.Bind(x));
			}
			return this.cache.bind;
		}

		#endregion
		#region GetValue

		public override ValueType GetValue(params Parameter[] x)
		{
			if(!ParameterEquals(x, this.cache.gv_x))
			{
				this.cache.gv_x = x;
				this.cache.gv = this.f.GetValue(x);
			}
			return this.cache.gv;
		}

		#endregion
		#region GetComplexPart

		public override void GetComplexPart(out Function re, out Function im)
		{
			if(this.cache.re == null)
			{
				this.f.GetComplexPart(out this.cache.re, out this.cache.im);
				this.cache.re = new CachedFunction(this.cache.re);
				this.cache.im = new CachedFunction(this.cache.im);
			}

			re = this.cache.re;
			im = this.cache.im;
		}

		#endregion
		#region その他の抽象関数

		public override System.Collections.ArrayList GetVariableList()
		{
			return this.f.GetVariableList();
		}

		#endregion
		#region 内部構造の最適化

		/// <remarks>
		/// 現状の CachedFunction には Optimize はかからないので、
		/// 予め Optimize を掛けてからキャッシュ化するように。
		/// </remarks>
		public override Function Optimize()
		{
			return this;
		}

		#endregion
		#region ICloneable メンバ

		public override Function Clone()
		{
			return new CachedFunction(this.f.Clone(), this.cache);
		}

		#endregion
		#region object

		public override string ToString()
		{
#if DEBUG
			return "c(" + this.f.ToString() + ")";
#else
			return this.f.ToString();
#endif
		}

		public override bool Equals(object obj)
		{
			CachedFunction cf = obj as CachedFunction;
			if(cf == null) return false;

			return this.f.Equals(cf.f);
		}

		public override int GetHashCode()
		{
			return 658376238 ^ this.f.GetHashCode();
		}

		#endregion
		#region 特殊な関数

		#region 単項関数

		public static new CachedFunction Exp(Function f)
		{
			return new CachedFunction(Function.Exp(f));
		}

		public static new CachedFunction Log(Function f)
		{
			return new CachedFunction(Function.Log(f));
		}

		public static new CachedFunction Log10(Function f)
		{
			return new CachedFunction(Function.Log10(f));
		}

		public static new CachedFunction Sin(Function f)
		{
			return new CachedFunction(Function.Sin(f));
		}

		public static new CachedFunction Cos(Function f)
		{
			return new CachedFunction(Function.Cos(f));
		}

		public static new CachedFunction Tan(Function f)
		{
			return new CachedFunction(Function.Tan(f));
		}

		public static new CachedFunction Asin(Function f)
		{
			return new CachedFunction(Function.Asin(f));
		}

		public static new CachedFunction Acos(Function f)
		{
			return new CachedFunction(Function.Acos(f));
		}

		public static new CachedFunction Atan(Function f)
		{
			return new CachedFunction(Function.Atan(f));
		}

		public static new CachedFunction Sinh(Function f)
		{
			return new CachedFunction(Function.Sinh(f));
		}

		public static new CachedFunction Cosh(Function f)
		{
			return new CachedFunction(Function.Cosh(f));
		}

		public static new CachedFunction Tanh(Function f)
		{
			return new CachedFunction(Function.Tanh(f));
		}

		public static new CachedFunction Abs(Function f)
		{
			return new CachedFunction(Function.Abs(f));
		}

		public static new CachedFunction Sqrt(Function f)
		{
			return new CachedFunction(Function.Sqrt(f));
		}

		#endregion
		#region 二項関数

		public static new CachedFunction Pow(Function f, Function g)
		{
			return new CachedFunction(Function.Pow(f, g));
		}

		public static new CachedFunction Log(Function f, Function g)
		{
			return new CachedFunction(Function.Log(f, g));
		}

		public static new CachedFunction Atan2(Function f, Function g)
		{
			return new CachedFunction(Function.Atan2(f, g));
		}

		#endregion

		#endregion
	}
}
