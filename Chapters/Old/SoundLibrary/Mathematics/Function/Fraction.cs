using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 分数型の関数。
	/// </summary>
	public class Fraction : Function
	{
		#region フィールド

		/// <summary>
		/// 分子。
		/// </summary>
		Function num;

		/// <summary>
		/// 分母
		/// </summary>
		Function denom;

		#endregion
		#region 初期化

		public Fraction(Function num, Function denom)
		{
			this.num = num;
			this.denom = denom;
		}

		public static Function Create(Function num, Function denom)
		{
			if(num == denom)
				return (Constant)1;
			return new Fraction(num, denom);
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			return Misc.Merge(this.num.GetVariableList(), this.denom.GetVariableList());
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			ValueType y;

			y = this.num.GetValue(x);
			y /= this.denom.GetValue(x);

			return y;
		}

		public override Function Bind(params Parameter[] x)
		{
			Function num   = this.num.Bind(x);
			Function denom = this.denom.Bind(x);

			if(denom is Constant)
			{
				Constant c = denom as Constant;
				ValueType val = 1 / c.Value;

				if(num is Constant)
				{
					c = num as Constant;
					val *= c.Value;
					return (Constant)val;
				}

				c = (Constant)val;
				return c * num;
			}

			return Fraction.Create(num, denom);
		}

		#endregion
		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			Function reN, imN, reD, imD;
			this.num.GetComplexPart(out reN, out imN);
			this.denom.GetComplexPart(out reD, out imD);

			Function denom = reD * reD + imD * imD;

			re = (reN * reD + imN * imD) / denom;
			im = (imN * reD - reN * imD) / denom;
		}

		#endregion
		#region 演算

		public override Function Multiply(Function f)
		{
			if(f is Fraction)
			{
				Fraction frac = f as Fraction;
				return Fraction.Create(this.num * frac.num, this.denom * frac.denom);
			}

			return Fraction.Create(this.num * f, this.denom);
		}

		public override Function Divide(Function f)
		{
			if(f is Fraction)
			{
				Fraction frac = f as Fraction;
				return Fraction.Create(this.num * frac.denom, this.denom * frac.num);
			}

			return Fraction.Create(this.num, this.denom * f);
		}

		#endregion
		#region 微分

		public override Function Differentiate(Variable x)
		{
			Function num  = this.num;
			Function nump = num.Differentiate(x);
			Function denom  = this.denom;
			Function denomp = denom.Differentiate(x);

			return (nump * denom - num * denomp) / (denom * denom);
		}

		#endregion
		#region 内部構造の最適化

		public override Function Optimize()
		{
			Function num = this.num.Optimize();
			Function denom = this.denom.Optimize();

			if(num.Equals(denom))
			{
				return (Constant)1;
			}

			return Fraction.Create(num, denom);
		}

		#endregion
		#region object

		public override string ToString()
		{
			string str = "{";
			str += this.num.ToString();
			str += "}/{";
			str += this.denom.ToString();
			str += "}";
			return str;
		}

		public override bool Equals(object obj)
		{
			Fraction f = obj as Fraction;

			if(f == null)
			{
				return false;
			}

			return this.num.Equals(f.num) && this.denom.Equals(f.denom);
		}

		public override int GetHashCode()
		{
			return this.num.GetHashCode() ^ this.denom.GetHashCode();
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Fraction(this.num.Clone(), this.denom.Clone());
		}

		#endregion
	}
}
