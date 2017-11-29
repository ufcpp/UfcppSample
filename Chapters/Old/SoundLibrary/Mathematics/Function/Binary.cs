using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 二項関数デリゲート。
	/// </summary>
	public delegate ValueType BinaryFunction(ValueType x, ValueType y);

	/// <summary>
	/// 二項関数。
	/// </summary>
	public class Binary : Function
	{
		#region フィールド

		/// <summary>
		/// 関数本体。
		/// </summary>
		protected BinaryFunction func;

		/// <summary>
		/// 内部関数。
		/// this.GetValue(x) == func(inner.GetValue(x))
		/// </summary>
		protected Function inner0;
		protected Function inner1;

		#endregion
		#region 初期化

		public Binary(BinaryFunction func, Function inner0, Function inner1)
		{
			this.func = func;
			this.inner0 = inner0;
			this.inner1 = inner1;
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			return Misc.Merge(this.inner0.GetVariableList(), this.inner1.GetVariableList());
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			return this.func(this.inner0.GetValue(x), this.inner1.GetValue(x));
		}

		public override Function Bind(params Parameter[] x)
		{
			Function inner0 = this.inner0.Bind(x);
			Function inner1 = this.inner1.Bind(x);

			Constant c0 = inner0 as Constant;
			Constant c1 = inner1 as Constant;

			if(c0 != null && c1 != null)
			{
				return (Constant)this.func(c0.Value, c1.Value);
			}

			Binary f = this.Clone() as Binary;
			f.inner0 = inner0;
			f.inner1 = inner1;
			return f;
		}

		#endregion
		#region 内部構造の最適化

		public override Function Optimize()
		{
			Function f0 = this.inner0.Optimize();
			Function f1 = this.inner1.Optimize();
			Binary u = (Binary)this.Clone();
			u.inner0 = f0;
			u.inner1 = f1;
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
			return this.FunctionName() + "(" + this.inner0.ToString() + ", " + this.inner1.ToString() + ")";
		}

		public override bool Equals(object obj)
		{
			Binary f = obj as Binary;

			if(f == null)
			{
				return false;
			}

			return this.func.Equals(f.func)
				&& this.inner0.Equals(f.inner0)
				&& this.inner1.Equals(f.inner1);
		}

		public override int GetHashCode()
		{
			return this.func.GetHashCode() ^ this.inner0.GetHashCode()
				^ (this.inner1.GetHashCode() << 1);
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Binary(
				(BinaryFunction)this.func.Clone(),
				this.inner0.Clone(),
				this.inner1.Clone());
		}

		#endregion
	}
}
