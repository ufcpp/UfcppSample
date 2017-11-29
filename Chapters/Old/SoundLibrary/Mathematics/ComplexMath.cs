using System;

namespace SoundLibrary.Mathematics
{
	/// <summary>
	/// Math 関数の複素数化。
	/// </summary>
	/* static */
	public class ComplexMath
	{
		public static Complex ExpI(double y)
		{
			return new Complex(Math.Cos(y), Math.Sin(y));
		}

		public static Complex Exp(Complex z)
		{
			return Math.Exp(z.Re) * ExpI(z.Im);
		}

		public static Complex Log(Complex z)
		{
			return new Complex(Math.Log(z.Abs), z.Arg);
		}

		static readonly double LOG10 = Math.Log(10);

		public static Complex Log10(Complex z)
		{
			return LOG10 * Log(z);
		}

		public static Complex Pow(Complex a, Complex z)
		{
			return Exp(Log(a) * z);
		}

		public static Complex Log(Complex a, Complex z)
		{
			return Log(z) / Log(a);
		}

		public static Complex Sin(Complex z)
		{
			return new Complex(
				Math.Sin(z.Re) * Math.Cosh(z.Im),
				Math.Cos(z.Re) * Math.Sinh(z.Im));
		}

		public static Complex Cos(Complex z)
		{
			return new Complex(
				Math.Cos(z.Re) * Math.Cosh(z.Im),
				-Math.Sin(z.Re) * Math.Sinh(z.Im));
		}

		public static Complex Tan(Complex z)
		{
			double x = Math.Tan(z.Re);
			double y = Math.Tanh(z.Im);

			return new Complex(x, y) / new Complex(1, -x * y);
		}

		public static Complex Sinh(Complex z)
		{
			return new Complex(
				Math.Cos(z.Im) * Math.Sinh(z.Re),
				Math.Sin(z.Im) * Math.Cosh(z.Re));
		}

		public static Complex Cosh(Complex z)
		{
			return new Complex(
				Math.Cos(z.Im) * Math.Cosh(z.Re),
				Math.Sin(z.Im) * Math.Sinh(z.Re));
		}

		public static Complex Tanh(Complex z)
		{
			double x = Math.Tanh(z.Re);
			double y = Math.Tan(z.Im);

			return new Complex(x, y) / new Complex(1, x * y);
		}

		/*
		public static Complex Asin(Complex z){return 0;}
		public static Complex Acos(Complex z){return 0;}
		public static Complex Atan(Complex z){return 0;}
		*/

		public static Complex Sqrt(Complex z)
		{
			double abs = Math.Sqrt(z.Abs);
			double arg = z.Arg / 2;

			return Complex.FromPolar(abs, arg);
		}
	}
}
