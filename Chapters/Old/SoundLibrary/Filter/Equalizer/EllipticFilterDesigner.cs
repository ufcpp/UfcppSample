using System;

using SoundLibrary.Mathematics;

namespace SoundLibrary.Filter.Equalizer
{
	/// <summary>
	/// 楕円フィルタ設計。
	/// </summary>
	public class EllipticFilterDesigner : FilterDesigner
	{
		#region フィールド

		/// <summary>
		/// ε
		/// </summary>
		double epsilon;

		/// <summary>
		/// 楕円関数の率 k1 の2乗
		/// </summary>
		double m1;

		#endregion
		#region 初期化

		public EllipticFilterDesigner(int order, double rp, double rs) : base(order)
		{
			this.SetParameter(rp, rs);
		}

		#endregion
		#region パラメータ設定

		/// <summary>
		/// パスバンドリプルとストップバンドリプルからパラメータ計算。
		/// </summary>
		/// <param name="rp">パスバンドリプル</param>
		/// <param name="rs">ストップバンドリプル</param>
		public void SetParameter(double rp, double rs)
		{
			rp *= rp;
			rs *= rs;
			this.epsilon = Math.Sqrt(1/rp - 1);
			this.m1 = (1/rp - 1) / (1/rs - 1);
		}

		public double Epsilon
		{
			get{return this.epsilon;}
			set{this.epsilon = value;}
		}

		public double M1
		{
			get{return this.m1;}
			set{this.m1 = value;}
		}

		#endregion
		#region 零点/極の計算

		/// <summary>
		/// フィルタの零点/極を計算。
		/// </summary>
		/// <param name="roots">零点/極一覧の格納先</param>
		public override void GetZeroPole(ZeroPole[] roots)
		{
			double m1 = this.m1;
			double m1p = 1 - this.m1;

			double Kk1  = Elliptic.K(m1);
			double Kk1p = Elliptic.K(m1p);

			double temp1 = -Math.PI * Kk1p / (this.order * Kk1);
			double temp2 = Math.Exp(temp1);

			double m    = Elliptic.InverseQ(Math.Exp(-Math.PI * Kk1p / (this.order * Kk1)));
			double mp   = 1 - m;
			double k    = Math.Sqrt(m);
			double Kk   = Elliptic.K(m);

			double Kkp  = Elliptic.K(mp);
			double temp3 = (Kk / Kkp) / (Kk1 / Kk1p);

			double phi, sn_p, cn_p, dn_p;
			double v = Elliptic.F(Math.Atan(1 / this.epsilon), m1p) * Kk / (this.order * Kk1);
			Elliptic.Jacobi(v, mp, out phi, out sn_p, out cn_p, out dn_p);

			for(int i=this.order-1, j=0; i>0; i-=2, ++j)
			{
				double sn, cn, dn;
				double u = Kk * (double)i / this.order;
				Elliptic.Jacobi(u, m, out phi, out sn, out cn, out dn);

				double denom = 1 - dn * dn * sn_p * sn_p;
				double re = -cn * dn * sn_p * cn_p / denom;
				double im = -sn * dn_p / denom;

				roots[j].zero = new Root(Root.Type.Complex, 0, 1 / (k * sn));
				roots[j].pole = new Root(Root.Type.Complex, re, im);
			}

			if((this.order & 1) == 1)
			{
				double denom = 1 - sn_p * sn_p;
				double re = -sn_p * cn_p / denom;

				roots[this.order / 2].zero = new Root(Root.Type.None, 0, 0);
				roots[this.order / 2].pole = new Root(Root.Type.Single, re, 0);
			}
		}

		#endregion
		#region アナログプロトタイプ

		public override void GetAnalogPrototype(Coefficient[] coefs)
//		public void GetAnalogPrototype2(Coefficient[] coefs)
		{
			double m1 = this.m1;
			double m1p = 1 - this.m1;

			double Kk1  = Elliptic.K(m1);
			double Kk1p = Elliptic.K(m1p);

			double m    = Elliptic.InverseQ(Math.Exp(-Math.PI * Kk1p / (this.order * Kk1)));
			double mp   = 1 - m;
			double Kk   = Elliptic.K(m);

			double phi, sn_p, cn_p, dn_p;
			double v = Elliptic.F(Math.Atan(1 / this.epsilon), m1p) * Kk / (this.order * Kk1);
			Elliptic.Jacobi(v, mp, out phi, out sn_p, out cn_p, out dn_p);

			for(int i=this.order-1, j=0; i>0; i-=2, ++j)
			{
				Coefficient coef = coefs[j];

				double sn, cn, dn;
				double u = Kk * (double)i / this.order;
				Elliptic.Jacobi(u, m, out phi, out sn, out cn, out dn);

				double denom = 1 - dn * dn * sn_p * sn_p;
				double re = -cn * dn * sn_p * cn_p / denom;
				double im = -sn * dn_p / denom;

				double alpha = -2 * re;
				double beta  = re * re + im * im;
				double gamma = 1 / (m * sn * sn);

				coef.a[0] = 1; coef.a[1] = alpha / beta; coef.a[2] = 1 / beta;
				coef.b[0] = 1; coef.b[1] = 0;            coef.b[2] = 1 / gamma;
			}

			if((this.order & 1) == 1)
			{
				Coefficient coef = coefs[this.order / 2];

				double denom = 1 - sn_p * sn_p;
				double re = -sn_p * cn_p / denom;

				coef.a[0] = 1; coef.a[1] = -1 / re; coef.a[2] = 0;
				coef.b[0] = 1; coef.b[1] = 0;       coef.b[2] = 0;
			}
		}

		#endregion
	}
}
