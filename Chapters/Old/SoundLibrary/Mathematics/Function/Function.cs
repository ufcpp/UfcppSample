using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 関数を表すクラスの抽象基底。
	/// </summary>
	public abstract class Function : ICloneable
	{
		#region フィールド

		//ArrayList variables = new ArrayList();

		#endregion
		#region 内部クラス

		public class Parameter
		{
			public Variable  x;
			public ValueType val;

			public Parameter(Variable x, ValueType val)
			{
				this.x = x;
				this.val = val;
			}

			public override bool Equals(object obj)
			{
				Parameter v = (Parameter)obj;
				return this.x.Equals(v.x) && this.val.Equals(v.val);
			}

			public override int GetHashCode()
			{
				return this.x.GetHashCode() ^ this.val.GetHashCode();
			}
		}

		#endregion
		#region 値の計算

		public VariableTable GetVariableTable()
		{
			return new VariableTable(this.GetVariableList());
		}

		/// <summary>
		/// その関数に含まれている変数のリストを求める。
		/// 例えば、f(g(x, y), h(x)) + i(y, z) とか言うように、
		/// 関数の合成・四則演算を使って作った関数の場合、{x, y, z} というリストを返す。
		/// </summary>
		/// <returns></returns>
		public abstract System.Collections.ArrayList GetVariableList();

		/// <summary>
		/// 値の計算。
		/// </summary>
		public virtual ValueType this[VariableTable t]
		{
			get
			{
				return this.GetValue(t.GetParameterList());
			}
		}

		public abstract ValueType GetValue(params Parameter[] x);

		/// <summary>
		/// 値を固定する。
		/// 例えば、f(x, y) っていう関数があったとして、
		/// y = 1 で固定した関数 g(x) = f(x, 1) を求めたり。
		/// </summary>
		/// <param name="x">固定したい値のリスト</param>
		/// <returns>値固定後の関数</returns>
		public abstract Function Bind(params Parameter[] x);

		#endregion
		#region 複素数対応

		/// <summary>
		/// 関数 f(z) を複素関数とみなしたとき、その実部と虚部
		/// Re[f](Re(z), Im(z)), Im[f](Re(z), Im(z) を求める。
		/// 例えば、Exp の場合、
		/// reY = Exp(reX) * Cos(imX),
		/// imY = Exp(reX) * Sin(imX)
		/// </summary>
		/// <param name="reX">パラメータの実部</param>
		/// <param name="imX">パラメータの虚部</param>
		public virtual void GetComplexPart(out Function re, out Function im)
		{
			throw new InvalidOperationException("この関数は複素数未対応です。");
		}

		/// <summary>
		/// 複素関数のノルム関数 |f(z)|^2 を求める。
		/// </summary>
		/// <returns>ノルム</returns>
		public Function Norm()
		{
			Function re, im;
			this.GetComplexPart(out re, out im);
			return re * re + im * im;
		}

		#endregion
		#region 演算

		/// <summary>
		/// 正負反転。
		/// </summary>
		/// <returns>正負反転結果</returns>
		public virtual Function Negate()
		{
			return this.Multiply((Constant)(-1.0));
		}

		/// <summary>
		/// 加算。
		/// </summary>
		/// <param name="f">オペランド</param>
		/// <returns>加算結果</returns>
		public virtual Function Add(Function f)
		{
			return new Sum(this, f);
		}

		/// <summary>
		/// 減算。
		/// </summary>
		/// <param name="f">オペランド</param>
		/// <returns>減算結果</returns>
		public virtual Function Subtract(Function f)
		{
			return this.Add(f.Negate());
		}

		/// <summary>
		/// 乗算。
		/// </summary>
		/// <param name="f">オペランド</param>
		/// <returns>乗算結果</returns>
		public virtual Function Multiply(Function f)
		{
			Constant c = f as Constant;
			if(c != null)
			{
				return new Multiple(c.Value, this);
			}

			return new Product(this, f);
		}

		/// <summary>
		/// 除算。
		/// </summary>
		/// <param name="f">オペランド</param>
		/// <returns>除算結果</returns>
		public virtual Function Divide(Function f)
		{
			Constant c = f as Constant;
			if(c != null)
			{
				return new Multiple(1 / c.Value, this);
			}

			return Fraction.Create(this, f);
		}

		#endregion
		#region 演算子

		public static Function operator+ (Function f)
		{
			return f.Clone();
		}

		public static Function operator- (Function f)
		{
			return f.Negate();
		}

		public static Function operator+ (Function f, Function g)
		{
			if(f.Equals((Constant)0))
				return g;
			if(g.Equals((Constant)0))
				return f;
			if(f.Equals(g))
				return new Multiple(2, f);

			return f.Add(g);
		}

		public static Function operator- (Function f, Function g)
		{
			if(f.Equals(g))
				return (Constant)0;
			return f.Subtract(g);
		}

		public static Function operator* (Function f, Function g)
		{
			if(f.Equals((Constant)0) || g.Equals((Constant)0))
				return (Constant)0;
			if(f.Equals((Constant)1))
				return g;
			if(g.Equals((Constant)1))
				return f;
//			if(f.Equals(g))
//				return Function.X(f, 2);

			return f.Multiply(g);
		}

		public static Function operator/ (Function f, Function g)
		{
			if(f.Equals(g))
				return (Constant)1;
			return f.Divide(g);
		}

		public static implicit operator Function (double x)
		{
			return (Constant)x;
		}

		#endregion
		#region 微分

		/// <summary>
		/// (x による偏)導関数を求める。
		/// 解析的に計算。
		/// 解析的に計算できない関数の場合、InvalidOperationException を throw する。
		/// </summary>
		/// <param name="x">微分対象となる変数</param>
		/// <returns>導関数</returns>
		public virtual Function Differentiate(Variable x)
		{
			throw new InvalidOperationException("微分できません");
		}

		#endregion
		#region 内部構造の最適化

		/// <summary>
		/// 内部構造を最適化する。
		/// 例えば、1×f → f, 0×f → 0。
		/// </summary>
		/// <returns>最適化後の関数</returns>
		/// <remarks>
		/// 一発で100％完璧な最適化が掛かるわけではない。
		/// 2・3度繰り返すことで、より最適化がかかる場合もあり。
		/// </remarks>
		public virtual Function Optimize()
		{
			return this;
		}

		/// <summary>
		/// 内部構造を最適化する。
		/// </summary>
		/// <param name="n">反復回数</param>
		/// <returns>最適化後の関数</returns>
		/// <remarks>
		/// 一発で完全な最適化が出来ないので、何度か処理を繰り返す。
		/// </remarks>
		public Function Optimize(int n)
		{
			Function opt = this.Optimize();
			for(; n>0; --n) opt = opt.Optimize();
			return opt;
		}

		#endregion
		#region ICloneable メンバ

		public abstract Function Clone();

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
		#region 特殊な関数

		#region 単項関数

		public static Unary Exp(Function f)
		{
			return new Elementary.Exp(f);
		}

		public static Unary Log(Function f)
		{
			return new Elementary.LogE(f);
		}

		public static Unary Log10(Function f)
		{
			return new Elementary.Log10(f);
		}

		public static Unary Sin(Function f)
		{
			return new Elementary.Sin(f);
		}

		public static Unary Cos(Function f)
		{
			return new Elementary.Cos(f);
		}

		public static Unary Tan(Function f)
		{
			return new Elementary.Tan(f);
		}

		public static Unary Asin(Function f)
		{
			return new Unary(
				new UnaryFunction(Math.Asin),
				f);
		}

		public static Unary Acos(Function f)
		{
			return new Unary(
				new UnaryFunction(Math.Acos),
				f);
		}

		public static Unary Atan(Function f)
		{
			return new Unary(
				new UnaryFunction(Math.Atan),
				f);
		}

		public static Unary Sinh(Function f)
		{
			return new Elementary.Sinh(f);
		}

		public static Unary Cosh(Function f)
		{
			return new Elementary.Cosh(f);
		}

		public static Unary Tanh(Function f)
		{
			return new Elementary.Tanh(f);
		}

		public static Unary Abs(Function f)
		{
			return new Unary(
				new UnaryFunction(Math.Abs),
				f);
		}

		public static Unary Sqrt(Function f)
		{
			return new Elementary.Sqrt(f);
		}

		#endregion
		#region 二項関数

		public static Binary Pow(Function f, Function g)
		{
			return new Elementary.Pow(f, g);
		}

		public static Binary Log(Function f, Function g)
		{
			return new Elementary.Log(f, g);
		}

		public static Binary Atan2(Function f, Function g)
		{
			return new Binary(
				new BinaryFunction(Math.Atan2),
				f, g);
		}

		#endregion
		#region 多項式

		public static Unary X(Function f)
		{
			return X(f, 1, 1);
		}

		public static Unary X(Function f, int order)
		{
			return X(f, order, 1);
		}

		public static Unary X(Function f, int order, double coef)
		{
			return new Elementary.Polynomial(f, Mathematics.Expression.Polynomial.X(order, coef));
		}

		#endregion
		#region 虚数

		public static Function I(Function f)
		{
			return new Imaginary(f);
		}

		#endregion

		#endregion
	}
}
