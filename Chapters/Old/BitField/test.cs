#define CHECK_BCD
//#define CHECK_MUL
//#define CHECK_DIV
//#define CHECK_MUL_DIV

using System;

namespace BitField
{
	/// <summary>
	/// BitField クラスのテスト用プログラム。
	/// </summary>
	class BitFieldTest
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Random rnd = new Random();

			// BinToBcd および BcdToBin のチェック
#if CHECK_BCD
			for(int i=1; i<100; ++i)
			{
				TestBcd(unchecked((ulong)rnd.Next(99999999)));
				TestBcd(unchecked((ulong)rnd.Next() + 99999999UL));
			}
			TestBcd(99999999UL);
			TestBcd(100000000UL);
#endif

			// MulDiv のチェック
#if CHECK_MUL_DIV
			for(int i=1; i<100; ++i)
			{
				TestMulDiv(unchecked((ulong)rnd.Next()), unchecked((ulong)rnd.Next()));
				TestMulDiv(unchecked((ulong)rnd.Next()), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
				TestMulDiv(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)rnd.Next()));
				TestMulDiv(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
			}
#endif

			// Mul のチェック
#if CHECK_MUL
			for(int i=1; i<100; ++i)
			{
				TestMul(unchecked((ulong)rnd.Next()), unchecked((ulong)rnd.Next()));
				TestMul(unchecked((ulong)rnd.Next()), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
				TestMul(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)rnd.Next()));
				TestMul(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
			}
#endif

			// Div のチェック
#if CHECK_DIV
			for(int i=1; i<100; ++i)
			{
				TestDiv(unchecked((ulong)rnd.Next()), unchecked((ulong)rnd.Next()));
				TestDiv(unchecked((ulong)rnd.Next()), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
				TestDiv(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)rnd.Next()));
				TestDiv(unchecked((ulong)(rnd.Next(0x10000) - 0x8000)), unchecked((ulong)(rnd.Next(0x10000) - 0x8000)));
			}
#endif
		}

		/// <summary>
		/// BinToBcd および BcdToBin のチェック
		/// </summary>
		/// <param name="m">テスト入力値</param>
		static void TestBcd(ulong m)
		{
			BitField bin;
			bin = BitField.Create(31, 0, m);

			Console.Write("{0,15:d08} : ", bin.Value);
			bool of;
			BitField bcd = SampleLogic.BinToBcd(bin, out of);
			Console.Write("{0,9:x08} : ", bcd.Value);
			BitField bin2 = SampleLogic.BcdToBin(bcd);
			Console.Write("{0,9:d08}", bin2.Value);
			if((bin.Value % 100000000) != bin2.Value)
				Console.Write(" **");
			if(of)
				Console.Write("  overflow");
			Console.Write("\n");
		}

		/// <summary>
		/// MulDiv のテスト
		/// </summary>
		/// <param name="m">テスト入力(オペランド1)</param>
		/// <param name="n">テスト入力(オペランド2)</param>
		static void TestMulDiv(ulong m, ulong n)
		{
			BitField a, b;
			a = BitField.Create(31, 0, m);
			b = BitField.Create(31, 0, n);

			BitField p = SampleLogic.MulDiv(a, b, true);
			BitField q = SampleLogic.MulDiv(a, b, false);

			ulong tmp1 = p.Value;
			ulong tmp2 = unchecked((ulong)((long)(int)a.Value * (long)(int)b.Value));

			Console.Write("{0:x8}×{1:x8} = ", (int)a.Value, (int)b.Value);
			Console.Write("{0:x16} ({1:x16})", tmp1, tmp2);

			if(tmp1 != tmp2)
			{
				Console.Write(" ** ");
				Console.ReadLine();
			}
			Console.Write("\n");

			uint tmp3 = (uint)q[31, 0].Value;
			uint tmp4 = unchecked((uint)((long)(int)a.Value / (long)(int)b.Value));

			Console.Write("{0:x8}÷{1:x8} = ", (int)a.Value, (int)b.Value);
			Console.Write("{0:x8} ({1:x8})", tmp3, tmp4);

			if(tmp3 != tmp4)
			{
				Console.Write(" ** ");
				Console.ReadLine();
			}
			Console.Write("\n");
		}

		/// <summary>
		/// Mul のテスト
		/// </summary>
		/// <param name="m">テスト入力(オペランド1)</param>
		/// <param name="n">テスト入力(オペランド2)</param>
		static void TestMul(ulong m, ulong n)
		{
			BitField a, b;
			a = BitField.Create(31, 0, m);
			b = BitField.Create(31, 0, n);

			BitField p = SampleLogic.Mul(a, b);

			ulong tmp1 = p.Value;
			ulong tmp2 = unchecked((ulong)((long)(int)a.Value * (long)(int)b.Value));

			Console.Write("{0:x8}×{1:x8} = ", (int)a.Value, (int)b.Value);
			Console.Write("{0:x16} ({1:x16})", tmp1, tmp2);

			if(tmp1 != tmp2)
			{
				Console.Write(" ** ");
				Console.ReadLine();
			}
			Console.Write("\n");
		}

		/// <summary>
		/// Div のテスト
		/// </summary>
		/// <param name="m">テスト入力(オペランド1)</param>
		/// <param name="n">テスト入力(オペランド2)</param>
		static void TestDiv(ulong m, ulong n)
		{
			BitField a, b;
			a = BitField.Create(31, 0, m);
			b = BitField.Create(31, 0, n);

			BitField q = SampleLogic.Div(a, b);

			uint tmp3 = (uint)q[31, 0].Value;
			uint tmp4 = unchecked((uint)((long)(int)a.Value / (long)(int)b.Value));

			Console.Write("{0:x8}÷{1:x8} = ", (int)a.Value, (int)b.Value);
			Console.Write("{0:x8} ({1:x8})", tmp3, tmp4);

			if(tmp3 != tmp4)
			{
				Console.Write(" ** ");
				Console.ReadLine();
			}
			Console.Write("\n");
		}
	}//class BitFieldTest
}//namespace BitField
