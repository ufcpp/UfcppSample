using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 単項関数デリゲート。
	/// </summary>
	public delegate ValueType UnaryFunction(ValueType x);

	/// <summary>
	/// 単項関数。
	/// </summary>
	public class Unary : Function
	{
		#region フィールド

		/// <summary>
		/// 関数本体。
		/// </summary>
		protected UnaryFunction func;

		/// <summary>
		/// 内部関数。
		/// this.GetValue(x) == func(inner.GetValue(x))
		/// </summary>
		protected Function inner;

		#endregion
		#region 初期化

		public Unary(UnaryFunction func, Function inner)
		{
			this.func = func;
			this.inner = inner;
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			return this.inner.GetVariableList();
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			return this.func(this.inner.GetValue(x));
		}

		public override Function Bind(params Parameter[] x)
		{
			Function inner = this.inner.Bind(x);

			Constant c = inner as Constant;

			if(c != null)
			{
				return (Constant)this.func(c.Value);
			}

			Unary f = this.Clone() as Unary;
			f.inner = inner;
			return f;
		}

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
		/// <param name="reY">関数値の実部</param>
		/// <param name="imY">関数値の虚部</param>
		protected virtual void GetComplexPart(Function reX, Function imX, out Function reY, out Function imY)
		{
			throw new InvalidOperationException("この関数は複素数未対応です。");
		}

		public override void GetComplexPart(out Function re, out Function im)
		{
			this.inner.GetComplexPart(out re, out im);
			this.GetComplexPart(re, im, out re, out im);
		}

		#endregion
		#region 微分

		/// <summary>
		/// this.func 自体の導関数を求める。
		/// </summary>
		/// <returns>this.func の導関数に相当する Function 型インスタンス</returns>
		protected virtual Function Differentiate()
		{
			throw new InvalidOperationException("微分できません");
		}

		public override Function Differentiate(Variable x)
		{
			Function innerDeriv = this.inner.Differentiate(x);
			Function deriv = this.Differentiate();

			return innerDeriv * deriv;
		}

		#endregion
		#region 内部構造の最適化

		public override Function Optimize()
		{
			Function f = this.inner.Optimize();
			Unary u = (Unary)this.Clone();
			u.inner = f;
			return u;
		}

		#endregion
		#region object

		protected virtual string FunctionName()
		{
			return string.Empty;
		}

		public override string ToString()
		{
			return this.FunctionName() + "(" + this.inner.ToString() + ")";
		}

		public override bool Equals(object obj)
		{
			Unary f = obj as Unary;

			if(f == null)
			{
				return false;
			}

			return this.func.Equals(f.func) && this.inner.Equals(f.inner);
		}

		public override int GetHashCode()
		{
			return this.func.GetHashCode() ^ this.inner.GetHashCode();
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Unary((UnaryFunction)this.func.Clone(), this.inner.Clone());
		}

		#endregion
	}
}
