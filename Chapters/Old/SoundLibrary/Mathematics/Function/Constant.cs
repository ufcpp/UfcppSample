using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 定数。
	/// </summary>
	public class Constant : Function
	{
		#region フィールド

		/// <summary>
		/// 定数の値。
		/// </summary>
		ValueType val;

		#endregion
		#region 初期化

		public Constant() : this(0){}

		public Constant(ValueType val)
		{
			this.val = val;
		}

		public static implicit operator Constant (ValueType val)
		{
			return new Constant(val);
		}

		#endregion
		#region 値

		public ValueType Value
		{
			get{return this.val;}
		}

		#endregion
		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			re = this;
			im = (Constant)0;
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			return new System.Collections.ArrayList();
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			return this.val;
		}

		public override Function Bind(params Parameter[] x)
		{
			return this;
		}

		#endregion
		#region 演算

		public override Function Negate()
		{
			return (Constant)(-this.val);
		}

		public override Function Add(Function f)
		{
			Constant c = f as Constant;

			if(c != null)
			{
				return (Constant)(this.val + c.val);
			}

			return f.Add(this);
		}

		public override Function Subtract(Function f)
		{
			Constant c = f as Constant;

			if(c != null)
			{
				return (Constant)(this.val - c.val);
			}

			return -f.Subtract(this);
		}

		public override Function Multiply(Function f)
		{
			Constant c = f as Constant;
			if(c != null)
			{
				return (Constant)(this.val * c.val);
			}

			Variable v = f as Variable;
			if(v != null)
			{
				return new Multiple(this.val, v);
			}

			return f.Multiply(this);
		}

		public override Function Divide(Function f)
		{
			Constant c = f as Constant;

			if(c != null)
			{
				return (Constant)(this.val / c.val);
			}

			return base.Divide(f);
		}

		#endregion
		#region 微分

		public override Function Differentiate(Variable x)
		{
			return (Constant)0;
		}

		#endregion
		#region object

		public override string ToString()
		{
			return this.val.ToString();
		}

		public override bool Equals(object obj)
		{
			Constant c = obj as Constant;

			if(c == null)
			{
				return false;
			}

			return this.val == c.val;
		}

		public override int GetHashCode()
		{
			return this.val.GetHashCode();
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Variable(this.val);
		}

		#endregion
	}
}
