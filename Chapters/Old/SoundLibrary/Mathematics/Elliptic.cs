/* ↓ここにあるソースを参考にしてます。
 *  http://www.ldas-sw.ligo.caltech.edu/cgi-bin/cvsweb.cgi/?cvsroot=GDS#dirlist
 */

//! テストまだ。テストコードを書く。

using System;

namespace SoundLibrary.Mathematics
{
	/// <summary>
	/// 楕円積分/楕円関数関連の static メソッド群を定義。
	/// </summary>
	/// <remarks>
	/// u = ∫ dφ/Δ(φ)
	/// Δ(φ) = √(1 - k^2 sin^2 φ)
	/// z = sn u = sin φ
	/// cn u = cos φ
	/// dn = √（1 - k^2 sn^2）
	/// </remarks>
	/*static*/
	public class Elliptic
	{
		const double EPSILON = 1.11022302462515654042E-16;

		#region 楕円関数

		/// <summary>
		/// 楕円積分の引数 u から振幅φを求める。
		/// </summary>
		/// <param name="u">引数 u</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>振幅φ</returns>
		public static double Phi(double u, double m)
		{
			if(m < 0.0 || m > 1.0) return double.NaN;

			double t;
			double b;

			if(m < EPSILON)
			{
				t = Math.Sin(u);
				b = Math.Cos(u);
				double ai = 0.25 * m * (u - t*b);

				return u - ai;
			}

			double twon;
   
			if(m >= 1 - EPSILON)
			{
				double ai = 0.25 * (1.0-m);
				b = Math.Cosh(u);
				t = Math.Tanh(u);
				twon =  b * Math.Sinh(u);

				return 2.0*Math.Atan(Math.Exp(u)) - Constant.PI2 + ai * (twon - u) / b;
			}
   
			const int MAX = 10;
			double[] a = new double[MAX], c = new double[MAX];
   
			b = Math.Sqrt(1.0 - m);
			twon = 1.0;
			a[0] = 1.0;
			c[0] = Math.Sqrt(m);
			int i = 0;
   
			while( Math.Abs(c[i]/a[i]) > EPSILON)
			{
				if(i >= MAX - 1)
				{
					break;
				}
				double ai = a[i];
				++i;
				c[i] = ( ai - b )/2.0;
				t = Math.Sqrt( ai * b );
				a[i] = ( ai + b )/2.0;
				b = t;
				twon *= 2.0;
			}
   
			/* backward recurrence */
			double phi = twon * a[i] * u;

			for(; i != 0; --i)
			{
				t = c[i] * Math.Sin(phi) / a[i];
				phi = (Math.Asin(t) + phi)/2.0;
			}

			return phi;
		}

		#region Jacobi の楕円関数

		/// <summary>
		/// Jacobi の楕円関数 sn u を求める。
		/// </summary>
		/// <param name="u">引数 u</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>sn(u, k)</returns>
		public static double Sn(double u, double m)
		{
			if(m < 0.0 || m > 1.0) return double.NaN;

			return Math.Sin(Elliptic.Phi(u, m));
		}

		/// <summary>
		/// Jacobi の楕円関数 cn u を求める。
		/// </summary>
		/// <param name="u">引数 u</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>cn(u, k)</returns>
		public static double Cn(double u, double m)
		{
			if(m < 0.0 || m > 1.0) return double.NaN;

			return Math.Cos(Elliptic.Phi(u, m));
		}

		/// <summary>
		/// Jacobi の楕円関数 dn u を求める。
		/// </summary>
		/// <param name="u">引数 u</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>dn(u, k)</returns>
		public static double Dn(double u, double m)
		{
			if(m < 0.0 || m > 1.0) return double.NaN;

			double sn = Elliptic.Sn(u, m);
			return Math.Sqrt(1 - m * sn * sn);
		}

		/// <summary>
		/// Jacobi の楕円関数(振幅φから sn, cn, dn)を求める。
		/// </summary>
		/// <param name="phi">振幅φ</param>
		/// <param name="m">率 k の2乗</param>
		/// <param name="sn">sn(u, k)</param>
		/// <param name="cn">cn(u, k)</param>
		/// <param name="dn">dn(u, k)</param>
		public static void Jacobi(double phi, double m, out double sn, out double cn, out double dn)
		{
			if(double.IsNaN(m) || m < 0.0 || m > 1.0)
			{
				sn = double.NaN;
				cn = double.NaN;
				dn = double.NaN;
				return;
			}

			sn  = Math.Sin(phi);
			cn  = Math.Cos(phi);
			dn  = Math.Sqrt(1 - m * sn * sn);;
		}

		/// <summary>
		/// Jacobi の楕円関数(引数 u と率 k から振幅φおよび sn, cn, dn)を求める。
		/// </summary>
		/// <param name="u">引数 u</param>
		/// <param name="m">率 k の2乗</param>
		/// <param name="phi">振幅φ</param>
		/// <param name="sn">sn(u, k)</param>
		/// <param name="cn">cn(u, k)</param>
		/// <param name="dn">dn(u, k)</param>
		public static void Jacobi(double u, double m, out double phi, out double sn, out double cn, out double dn)
		{
			if(m < 0.0 || m > 1.0)
			{
				phi = double.NaN;
				sn  = double.NaN;
				cn  = double.NaN;
				dn  = double.NaN;
				return;
			}

			if(m < EPSILON)
			{
				double t = Math.Sin(u);
				double b = Math.Cos(u);
				double ai = 0.25 * m * (u - t*b);

				phi = u - ai;
				sn  = t - ai*b;
				cn  = b + ai*t;
				dn  = 1.0 - 0.5*m*t*t;
				return;
			}
 
			if(m >= 1 - EPSILON)
			{
				double ai = 0.25 * (1.0-m);
				double b = Math.Cosh(u);
				double t = Math.Tanh(u);
				double binv = 1.0/b;
				double twon =  b * Math.Sinh(u);

				phi = 2.0*Math.Atan(Math.Exp(u)) - Constant.PI2 + ai*(twon - u)/b;
				sn  = t + ai * (twon - u)/(b*b);
				ai *= t * phi;
				cn  = binv - ai * (twon - u);
				dn  = binv + ai * (twon + u);
				return;
			}

			phi = Elliptic.Phi(u, m);
			Elliptic.Jacobi(phi, m, out sn, out cn, out dn);
		}

		#endregion
		#region Jacobi の楕円関数の逆関数

#if false

		/// <summary>
		/// Jacobi の逆楕円関数 u = sn^-1 v の逆を求める。
		/// </summary>
		/// <param name="v">引数 v</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>sn(u, k)</returns>
		public static double InverseSn(double v, double m)
		{
			return 0;
		}

		// InverseCn
		// InverseDn

#endif

		#endregion
		#region テータ関数

#if false
		/// <summary>
		/// 楕円テータ関数θ_a(q, u) (a = 1, 2, 3, 4)を求める。
		/// </summary>
		/// <param name="a">θ_a の a</param>
		/// <param name="u">引数 u</param>
		/// <param name="q">パラメータ q</param>
		/// <returns></returns>
		public static double Theta(int a, double u, double q)
		{
			return 0;
		}

		/// <summary>
		/// 楕円テータ関数θ_a(q, u) (a = 1, 2, 3, 4)の導関数を求める。
		/// </summary>
		/// <param name="a">θ_a の a</param>
		/// <param name="u">引数 u</param>
		/// <param name="q">パラメータ q</param>
		/// <returns></returns>
		public static double ThetaPrime(int a, double u, double q)
		{
			return 0;
		}

		// WeierstrassP
		// WeierstrassSigma
		// WeierstrassZeta
#endif

		#endregion
		#region ノーム

#if false
		/// <summary>
		/// ノーム q(m) を計算する。
		/// </summary>
		/// <param name="m">率 k の2乗</param>
		/// <returns>ノーム q(m)</returns>
		/// <remarks>
		/// ノーム q(m) = exp( - pi K(1-m)/K(m) )
		/// </remarks>
		public double Q(double m)
		{
			return 0;
		}
#endif

		/// <summary>
		/// ノーム q(m) の逆を計算する。
		/// </summary>
		/// <param name="m">率 k の2乗</param>
		/// <returns>ノームの逆 m(q)</returns>
		public static double InverseQ(double q)
		{
			double t1, t2;

			double a = 1.0; // Theta3(0,q)
			double b = 1.0; // Theta2(0,q)/(2 q^(1/4))
			double r = 1.0;
			double p = q;

			do
			{
				r *= p;
				a += 2.0 * r;
				t1 = Math.Abs( r/a );

				r *= p;
				b += r;
				p *= q;
				t2 = Math.Abs( r/b );
      
			}
			while(t1 > EPSILON || t2 > EPSILON);

			b /= a;
			b *= b;
			b *= b; // b = (b / a)^4

			return 16.0 * q * b;
		}

		#endregion

		#endregion
		#region 楕円積分

		/// <summary>
		/// 第1種完全楕円積分。
		/// </summary>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double K(double m)
		{
			if(m < 0.0 || m >= 1.0) 
			{
				return double.NaN;
			}

			if(m == 0) 
			{
				return Math.PI / 2;
			}
   
			double a0 = 1, a1 = 1;
			double b0 = Math.Sqrt( 1 - m );
			double s0 = m;
			double mm = 1;
			int i = 0;
			double pow2i = 1;
   
			while(mm > EPSILON) 
			{
				a1 = (a0 + b0) / 2;
				double b1 = Math.Sqrt(a0 * b0);
				double c1 = (a0 - b0) / 2;
				++i;
				pow2i *= 2;
				double w1 = pow2i * c1 * c1;
				mm = w1;
      
				s0 += w1;
				a0 = a1;
				b0 = b1;
			}
   
			return Math.PI / 2 / a1;
		}

		/// <summary>
		/// 第1種不完全楕円積分。
		/// </summary>
		/// <param name="phi">振幅φ</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double F(double phi, double m)
		{
			if(m == 0.0)
				return phi;
   
			if(m == 1.0)
			{
				if(Math.Abs(phi) >= Math.PI / 2)
				{
					return double.NaN;
				}
				return Math.Log(Math.Tan((Math.PI / 2 + phi) / 2.0));
			}
   
			int npio2 = (int)Math.Floor(phi / (Math.PI / 2));
			if((npio2 & 1) != 0)
				++npio2;
   
			double K = npio2 == 0 ? Elliptic.K(1.0 - m) : 0.0;
   
			phi -= npio2 * Math.PI / 2;
			int sign = phi < 0.0 ? -1 : 1;
			phi = Math.Abs(phi);
   
			double t = Math.Tan(phi);
			if(Math.Abs(t) > 10.0)
			{
				/* Transform the amplitude */
				double e = 1.0 / (Math.Sqrt(1.0 - m) * t);
				/* ... but avoid multiple recursions.  */
				if(Math.Abs(e) < 10.0)
				{
					if(npio2 == 0)
						K = Elliptic.K(1.0 - m);
					double ret = K - Elliptic.F(Math.Atan(e), m);
					return sign * ret + npio2 * K;
				}
			}
   
			double a = 1.0;
			double b = Math.Sqrt(1.0 - m);
			double c = Math.Sqrt(m);
			int d = 1;
			int mod = 0;
			while(Math.Abs(c/a) > EPSILON)
			{
				double tmp = b/a;
				phi = phi + (Math.Atan(t * tmp) + mod * Math.PI);
				mod = (int)((phi + Math.PI / 2) / Math.PI);
				t = t * (1.0 + tmp) / (1.0 - tmp * t * t);
				c = (a - b) / 2.0;
				tmp = Math.Sqrt(a * b);
				a = (a + b) / 2.0;
				b = tmp;
				d += d;
			}
   
			return sign * (Math.Atan(t) + mod * Math.PI) / (d * a) + npio2 * K;
		}

#if false
		/// <summary>
		/// 第2種完全楕円積分。
		/// </summary>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double E(double m)
		{
			//!
			return 0;
		}

		/// <summary>
		/// 第2種不完全楕円積分。
		/// </summary>
		/// <param name="phi">振幅φ</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double E(double phi, double m)
		{
			//!
			return 0;
		}

		/// <summary>
		/// 第3種完全楕円積分。
		/// </summary>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double Pi(double n, double m)
		{
			//!
			return 0;
		}

		/// <summary>
		/// 第3種不完全楕円積分。
		/// </summary>
		/// <param name="phi">振幅φ</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double Pi(double phi, double n, double m)
		{
			//!
			return 0;
		}

		/// <summary>
		/// Jacobi のゼータ関数。
		/// </summary>
		/// <param name="phi">振幅φ</param>
		/// <param name="m">率 k の2乗</param>
		/// <returns>積分値</returns>
		public static double JacobiZeta(double phi, double m)
		{
			//!
			return 0;
		}
#endif

		#endregion
	}
}
