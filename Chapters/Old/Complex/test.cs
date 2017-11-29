using System;

namespace MyMath
{
	/// <summary>
	/// Complex クラスの動作確認
	/// </summary>
	class TestComplex
	{
		static void Main(string[] args)
		{
			Complex z, w;

			z = new CartesianComplex(1, 1);
			w = new CartesianComplex(0, 1);
			Show(z, w); // Cartesian 同士

			z = new PolarComplex(Math.Sqrt(2), Math.PI/4);
			Show(z, w); // Polar と Cartesian

			w = new PolarComplex(1, Math.PI/2);
			Show(z, w); // Polar 同士

			z = new CartesianComplex(1, 1);
			Show(z, w); // Cartesian と Polar
		}//Main

		static void Show(Complex z, Complex w)
		{
			Console.Write("z = {0}, w = {1}\n", z, w);
			Console.Write("|z| = {0}, ", z.Abs);
			Console.Write("||z|| = {0}\n", z.Norm());
			Console.Write("∠z = {0}\n", z.Arg);
			Console.Write("+z = {0}, ", +z);
			Console.Write("-z = {0}, ", -z);
			Console.Write("~z = {0}\n", ~z);
			Console.Write("z + w = {0}, ", z + w);
			Console.Write("z - w = {0}\n", z - w);
			Console.Write("z * w = {0}, ", z * w);
			Console.Write("z / w = {0}\n", z / w);
		}
	}//class TestComplex
}//namespace MyMath
