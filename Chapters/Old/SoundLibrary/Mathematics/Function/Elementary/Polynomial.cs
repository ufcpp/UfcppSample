using System;

namespace SoundLibrary.Mathematics.Function.Elementary
{
	using CoefType   = System.Double;
	using DomainType = System.Double;
	using ValueType  = System.Double;
	using Poly = SoundLibrary.Mathematics.Expression.Polynomial;

	/// <summary>
	/// 多項式。
	/// </summary>
	public class Polynomial : Unary
	{
		#region フィールド

		Poly p;

		#endregion
		#region 初期化

		public Polynomial(Function f) : this(f, 0.0) {}

		public Polynomial(Function f, int order) : this(f, new CoefType[order + 1]) {}

		public Polynomial(Function f, params CoefType[] coef) : this(f, new Poly(coef)) {}

		public Polynomial(Function f, Poly p) : base(null, f)
		{
			this.p = p;
			this.func = new UnaryFunction(this.p.Value);
		}

		#endregion
		#region 複素数対応

		protected override void GetComplexPart(Function reX, Function imX, out Function reY, out Function imY)
		{
			Polynomial[] reCoef = new Polynomial[this.p.Coef.Length];
			Polynomial[] imCoef = new Polynomial[this.p.Coef.Length];

			reY = new GeneralPolynomial(reX, reCoef);
			imY = new GeneralPolynomial(reX, imCoef);

			Function z = imX / reX;

			for(int n=0; n<this.p.Coef.Length; ++n)
			{
				reCoef[n] = new Polynomial(z, n);
				imCoef[n] = new Polynomial(z, n);

				for(int k=0; k<=n; ++k)
				{
					switch(k % 4)
					{
						case 0:
							reCoef[n].p.Coef[k] = this.p.Coef[n] * Misc.Combination(n, k);
							imCoef[n].p.Coef[k] = 0;
							break;
						case 1:
							reCoef[n].p.Coef[k] = 0;
							imCoef[n].p.Coef[k] = this.p.Coef[n] * Misc.Combination(n, k);
							break;
						case 2:
							reCoef[n].p.Coef[k] = -this.p.Coef[n] * Misc.Combination(n, k);
							imCoef[n].p.Coef[k] = 0;
							break;
						default:
							reCoef[n].p.Coef[k] = 0;
							imCoef[n].p.Coef[k] = -this.p.Coef[n] * Misc.Combination(n, k);
							break;
					}
				}
			}
		}

		#endregion
		#region 演算子

		public override Function Negate()
		{
			return new Polynomial(this.inner, -this.p);
		}

		public override Function Add(Function f)
		{
			Polynomial poly = f as Polynomial;
			if(poly != null && this.inner.Equals(poly.inner))
			{
				Poly p = this.p + poly.p;
				return new Polynomial(this.inner, p);
			}

			if(this.inner.Equals(f))
			{
				return new Polynomial(this.inner, this.p + Poly.X(1));
			}

			Constant c = f as Constant;
			if(c != null)
			{
				return new Polynomial(this.inner, this.p + c.Value);
			}

			return base.Add(f);
		}

		public override Function Subtract(Function f)
		{
			Polynomial poly = f as Polynomial;

			if(poly == null || !this.inner.Equals(poly.inner))
			{
				return base.Subtract(f);
			}

			Poly p = this.p - poly.p;
			return new Polynomial(this.inner, p);
		}

		public override Function Multiply(Function f)
		{
			Polynomial poly = f as Polynomial;

			if(poly != null && this.inner.Equals(poly.inner))
			{
				Poly p = this.p * poly.p;
				return new Polynomial(this.inner, p);
			}

			if(this.inner.Equals(f))
			{
				return new Polynomial(this.inner, this.p * Poly.X(1));
			}

			Constant c = f as Constant;
			if(c != null)
			{
				return new Polynomial(this.inner, this.p * c.Value);
			}

			return base.Multiply(f);
		}

		#endregion
		#region 微分

		protected override Function Differentiate()
		{
			CoefType[] coef  = this.p.Coef;

			if(coef.Length <= 1)
				return (Constant)0;

			//! ↓ Mathematics.Expression.Polynomial の方にあるべきコードかも。
			CoefType[] deriv = new CoefType[coef.Length - 1];

			for(int i=1; i<coef.Length; ++i)
			{
				deriv[i - 1] = i * coef[i];
			}

			return new Polynomial(this.inner, new Poly(deriv));
		}

		#endregion
		#region 内部構造の最適化

		public override Function Optimize()
		{
			if(this.p.Coef.Length == 1)
				return (Constant)this.p.Coef[0];

			if(this.p.Coef.Length == 2 && this.p.Coef[0] == 0)
			{
				if(this.p.Coef[1] == 1)
					return this.inner;

				return new Multiple(this.p.Coef[1], this.inner);
			}

			return this;
		}

		#endregion
		#region object

		public override string ToString()
		{
			string str;

			str = this.p.Coef[0].ToString();

			for(int i=1; i<this.p.Coef.Length; ++i)
			{
				if(this.p.Coef[i] < 0)
				{
					str += string.Format(" - {0}X^{1}", Math.Abs(this.p.Coef[i]), i);
				}
				else if(this.p.Coef[i] > 0)
				{
					str += string.Format(" + {0}X^{1}", this.p.Coef[i], i);
				}
			}

			str += string.Format("(X = {0})", this.inner);

			return str;
		}

		public override bool Equals(object obj)
		{
			Polynomial f = obj as Polynomial;

			if(f == null)
			{
				return false;
			}

			return this.p.Equals(f.p) && this.inner.Equals(f.inner);
		}

		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.inner.GetHashCode();
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Polynomial(this.inner, this.p);
		}

		#endregion
	}
}
