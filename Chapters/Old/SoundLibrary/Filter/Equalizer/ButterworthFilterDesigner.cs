using System;

namespace SoundLibrary.Filter.Equalizer
{
	/// <summary>
	/// バターワースフィルタ設計。
	/// </summary>
	public class ButterworthFilterDesigner : FilterDesigner
	{
		#region 初期化

		public ButterworthFilterDesigner(int order) : base(order)
		{
		}

		#endregion
		#region 零点/極の計算

		/// <summary>
		/// フィルタの零点/極を計算。
		/// </summary>
		/// <param name="roots">零点/極一覧の格納先</param>
		public override void GetZeroPole(ZeroPole[] roots)
		{
			for(int i=this.order-1, j=0; i>0; i-=2, ++j)
			{
				double w = Math.PI / 2.0 * (double)i / this.order;
				double sin, cos;
				GetSinCos(w, out sin, out cos);

				roots[j].zero = new Root(Root.Type.None, 0, 0);
				roots[j].pole = new Root(Root.Type.Complex, -cos, -sin);
			}

			if((this.order & 1) == 1)
			{
				roots[this.order / 2].zero = new Root(Root.Type.None, 0, 0);
				roots[this.order / 2].pole = new Root(Root.Type.Single, -1, 0);
			}
		}

		#endregion
		#region アナログプロトタイプ

		//public override void GetAnalogPrototype(Coefficient[] coefs)
		public void GetAnalogPrototype2(Coefficient[] coefs)
		{
			for(int i=this.order-1, j=0; i>0; i-=2, ++j)
			{
				Coefficient coef = coefs[j];

				coef.a[0] = 1;
				coef.a[1] = 2 * Math.Cos(Math.PI / 2 * (double)i / this.order);
				coef.a[2] = 1;
				coef.b[0] = 1;
				coef.b[1] = 0;
				coef.b[2] = 0;
			}

			if((this.order & 1) == 1)
			{
				Coefficient coef = coefs[this.order / 2];

				coef.a[0] = 1;
				coef.a[1] = 1;
				coef.a[2] = 0;
				coef.b[0] = 1;
				coef.b[1] = 0;
				coef.b[2] = 0;
			}
		}

		#endregion
		#region ディジタル LPF

		//public override void GetDigitalLPF(double w, Coefficient[] coefs)
		public void GetDigitalLPF2(double w, Coefficient[] coefs)
		{
			double sin, cos;
			GetSinCos(w, out sin, out cos);
			double b0 = (1 - cos) / 2;
			double a1 = -2 * cos;

			double nu = (this.order & 1) == 1 ? 1 : 0.5;

			for(int i=this.order-1, j=0; i>0; i-=2, ++j)
			{
				Coefficient coef = coefs[j];

				double alpha = sin * Math.Cos(Math.PI / 2.0 * (double)i / this.order);

				coef.a[0] = 1 + alpha;
				coef.a[1] = a1;
				coef.a[2] = 1 - alpha;
				coef.b[0] = b0;
				coef.b[1] = 2 * b0;
				coef.b[2] = b0;
			}

			if((this.order & 1) == 1)
			{
				Coefficient coef = coefs[this.order / 2];

				coef.a[0] = sin + cos + 1;
				coef.a[1] = sin - cos - 1;
				coef.a[2] = 0;
				coef.b[0] = sin;
				coef.b[1] = sin;
				coef.b[2] = 0;
			}
		}

		#endregion
	}
}
