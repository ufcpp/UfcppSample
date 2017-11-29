using System;

using SoundLibrary.Mathematics;

namespace SoundLibrary.Filter.Equalizer
{
	/// <summary>
	/// 根(零点または極)。
	/// </summary>
	/// <remarks>
	/// 信号処理の分野では、フィルタの零/極が共役複素数の場合でも実数係数フィルタで実現できるように、
	/// フィルタを2次ずつに区切って実現することが多いので、解を2個ずつペアにして表現。
	/// 	根のタイプ …
	/// 		単根、実根×2(重根含む)、共役複素数根、なし(定数項のみ)。
	/// 	a, b …
	/// 		単根の場合、a に値を。b は無視。
	/// 		実根の場合、a, b にそれぞれの根の値を。
	/// 		共役複素根、a に実部、b に虚部。
	/// </remarks>
	public class Root : ICloneable
	{
		#region インナークラス

		/// <summary>
		/// 根のタイプ
		/// </summary>
		public enum Type
		{
			None,    // 根なし(定数項のみ)
			Real,    // 実数根×2
			Single,  // 実数根×1
			Complex, // 共役複素数根
		}

		#endregion
		#region public フィールド

		public Type type;
		public double a;
		public double b;

		#endregion
		#region 初期化

		public Root() : this(Type.Complex, 0, 0) {}

		public Root(Type type, double a, double b)
		{
			this.type = type;
			this.a = a;
			this.b = b;
		}

		#endregion
		#region 解の取り出し

		/// <summary>
		/// ちゃんとした複素数にして解の値を返す。
		/// </summary>
		/// <param name="x1">解1</param>
		/// <param name="x2">解2(実数根×1の場合には0に)</param>
		/// <returns>解の数(0 ～ 2)</returns>
		public int GetRoots(out Complex x1, out Complex x2)
		{
			switch(this.type)
			{
				case Type.None:
					x1 = 0;
					x2 = 0;
					return 0;
				case Type.Real:
					x1 = this.a;
					x2 = this.b;
					return 2;
				case Type.Single:
					x1 = this.a;
					x2 = 0;
					return 1;
				default:
					x1 = new Complex(this.a, this.b);
					x2 = new Complex(this.a, -this.b);
					return 2;
			}
		}

		#endregion
		#region ICloneable メンバ

		public Root Clone()
		{
			return new Root(this.type, this.a, this.b);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
	}

	/// <summary>
	/// 零点と極のペア。
	/// </summary>
	/// <remarks>
	/// IIR フィルタを想定。
	/// 2次のIIRフィルタ→零点と極が1ペアずつ。
	/// </remarks>
	public class ZeroPole : ICloneable
	{
		#region public フィールド

		public Root zero;
		public Root pole;

		#endregion
		#region 初期化

		public ZeroPole() : this(new Root(), new Root()) {}

		public ZeroPole(Root zero, Root pole)
		{
			this.zero = zero;
			this.pole = pole;
		}

		#endregion
		#region ICloneable メンバ

		public ZeroPole Clone()
		{
			return new ZeroPole(this.zero, this.pole);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
	}

	/// <summary>
	/// フィルタ係数。
	/// アナログプロトタイプ/ディジタル兼用。
	/// </summary>
	/// <remarks>
	/// アナログ  : ∑b[i]s^i  / ∑a[i]s^i
	/// ディジタル: ∑b[i]z^-i / ∑a[i]z^-i
	/// 双一次変換では分母/分子の次数が変わらないことを利用して、
	/// アナログ・ディジタルで係数クラスを使いまわす。
	/// </remarks>
	public class Coefficient : ICloneable
	{
		#region public フィールド

		public double[] a = new double[3];
		public double[] b = new double[3];

		#endregion
		#region 初期化

		public Coefficient() : this(1, 0, 0, 1, 0, 0) {}

		public Coefficient(double a0, double a1, double a2, double b0, double b1, double b2)
		{
			this.a[0] = a0;
			this.a[1] = a1;
			this.a[2] = a2;
			this.b[0] = b0;
			this.b[1] = b1;
			this.b[2] = b2;
		}

		#endregion
		#region ICloneable メンバ

		public Coefficient Clone()
		{
			return new Coefficient(
				this.a[0], this.a[1], this.a[2],
				this.b[0], this.b[1], this.b[2]);
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
	}
}
