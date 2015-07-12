using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 定数×関数。
	/// </summary>
	public class Multiple : Function
	{
		#region フィールド

		/// <summary>
		/// 加算対象関数リスト。
		/// </summary>
		ValueType factor;

		/// <summary>
		/// 内包している関数。
		/// </summary>
    Function inner;

		#endregion
		#region プロパティ

		public double Factor{get{return this.factor;}}
		public Function Inner{get{return this.inner;}}

		#endregion
		#region 初期化

		public Multiple(ValueType factor, Function f)
		{
			this.factor = factor;
			this.inner  = f;
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			return this.inner.GetVariableList();
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			return this.factor * this.inner.GetValue(x);
		}

		public override Function Bind(params Parameter[] x)
		{
			Function f = this.inner.Bind(x);

			if(f is Constant)
			{
				Constant c = f as Constant;
				return (Constant)(this.factor * c.Value);
			}

			return new Multiple(this.factor, f);
		}

		#endregion
		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			this.inner.GetComplexPart(out re, out im);

			re *= this.factor;
			im *= this.factor;
		}

		#endregion
		#region 演算子

		public override Function Negate()
		{
			return new Multiple(-this.factor, this.inner);
		}

		public override Function Add(Function f)
		{
			Multiple g = f as Multiple;
			if(g != null && this.inner.Equals(g.inner))
			{
				return new Multiple(this.factor + g.factor, this.inner);
			}

			Variable v = f as Variable;
			if(v != null && this.inner.Equals(v))
			{
				return new Multiple(this.factor + 1, this.inner);
			}

			Constant c = f as Constant;
			if(c != null && (this.inner is Constant))
			{
				Constant c0 = this.inner as Constant;
				return (Constant)(c.Value + this.factor * c0.Value);
			}

			return base.Add(f);
		}

		/*
		public override Function Subtract(Function f)
		{
			Multiple g = f as Multiple;
			if(g != null && this.inner.Equals(g.inner))
			{
				return new Multiple(this.factor - g.factor, this.inner);
			}

			Variable v = f as Variable;
			if(v != null && this.inner.Equals(v))
			{
				return new Multiple(this.factor - 1, this.inner);
			}

			Constant c = f as Constant;
			if(c != null && (this.inner is Constant))
			{
				Constant c0 = this.inner as Constant;
				return (Constant)(this.factor * c0.Value - c.Value);
			}

			return base.Subtract(f);
		}
		*/

		public override Function Multiply(Function f)
		{
			Multiple g = f as Multiple;
			if(g != null)
			{
				return new Multiple(this.factor * g.factor, this.inner * g.inner);
			}

			Constant c = f as Constant;
			if(c != null)
			{
				if(this.inner is Constant)
				{
					Constant c0 = this.inner as Constant;
					return (Constant)(this.factor * c0.Value * c.Value);
				}
				return new Multiple(this.factor * c.Value, this.inner);
			}

			return new Multiple(this.factor, this.inner * f);
		}

		public override Function Divide(Function f)
		{
			Multiple g = f as Multiple;

			if(g != null)
			{
				return new Multiple(this.factor / g.factor, this.inner / g.inner);
			}

			return new Multiple(this.factor, this.inner / f);
		}

		#endregion
		#region 微分

		public override Function Differentiate(Variable x)
		{
			Function f = this.inner.Differentiate(x);

			if(f is Constant)
			{
				Constant c = f as Constant;
				return (Constant)(this.factor * c.Value);
			}

			return new Multiple(this.factor, f);
		}

		#endregion
		#region 内部構造の最適化

		public override Function Optimize()
		{
			if(this.factor == 0)
				return (Constant)0;

			Function f = this.inner.Optimize();

			if(this.factor == 1)
				return f;

			Constant c = f as Constant;
			if(c != null)
			{
				return (Constant)(this.factor * c.Value);
			}

			Multiple m = f as Multiple;
			if(m != null)
			{
				return new Multiple(this.factor * m.factor, m.inner);
			}

			Product p = f as Product;
			if(p != null)
			{
				Product tmp = p.Clone() as Product;
				tmp.AddList((Constant)this.factor);
				return tmp.Optimize();
			}

			return new Multiple(this.factor, f);
		}

		#endregion
		#region object

		public override string ToString()
		{
#if DEBUG
			return "m" + this.factor.ToString() + " * " + this.inner.ToString();
#else
			return this.factor.ToString() + " * " + this.inner.ToString();
#endif
		}

		public override bool Equals(object obj)
		{
			Multiple f = obj as Multiple;

			if(f == null)
			{
				return false;
			}

			return (this.factor == f.factor) && this.inner.Equals(f.inner);
		}

		public override int GetHashCode()
		{
			return (int)(this.inner.GetHashCode() * this.factor);
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Multiple(this.factor, this.inner);
		}

		#endregion
	}
}
