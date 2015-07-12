using System;

namespace BitField
{
	/// <summary>
	/// BitField がらみの例外
	/// </summary>
	public class BitFieldException : System.Exception
	{
		/// <summary>
		/// デフォルトコンストラクタ。
		/// </summary>
		public BitFieldException(){}

		/// <summary>
		/// メッセージ付きコンストラクタ。
		/// </summary>
		/// <param name="msg">エラーメッセージ</param>
		public BitFieldException(string msg) : base(msg){}
	}//class BitFieldException

	/// <summary>
	/// ビットフィールドクラス。
	/// Verilog っぽい操作が可能。
	/// 64ビットが限界(仕様です)。
	/// </summary>
	/// <example>
	/// reg [31:0] z;
	/// z &lt;= {z[30:1], z[31]};
	/// ↓
	/// BitField z = BitField.Create(31, 0);
	/// z.Assign(BitField.Concat(z[30, 1], z[31])); 
	/// </example>
	/// <example>
	/// wire [3:0] a;
	/// assign a = 4'hA;
	/// ↓
	/// BitField a = BitField.Create(3, 0);
	/// a.Assign(0xA);
	/// </example>
	abstract class BitField
	{
		/// <summary>
		/// MSB が m、LSB が l のビットフィールドを作成。
		/// verilog の <c>wire [m:l] z;</c> に相当。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>MSB が m、LSB が l のビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// m &lt; l のとき発生。
		/// </exception>
		public static BitField Create(int m, int l)
		{
			return new BitFieldImmediate(m, l);
		}

		/// <summary>
		/// MSB が m、LSB が l のビットフィールドを作成。
		/// verilog の <c>wire [m:l] z;</c> に相当。
		/// val で値を初期化する。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <param name="val">初期値</param>
		/// <returns>MSB が m、LSB が l、値が val のビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// m &lt; l のとき発生。
		/// </exception>
		public static BitField Create(int m, int l, ulong val)
		{
			return new BitFieldImmediate(m, l, val);
		}

		/// <summary>
		/// 1 びっとのビットフィールドを作成。
		/// </summary>
		/// <param name="b">ビットの真理値</param>
		/// <returns>1ビットのビットフィールド</returns>
		public static BitField Create(bool b)
		{
			return new BitFieldImmediate(0, 0, b ? 1UL : 0UL);
		}

		/// <summary>
		/// 格納されている値を ulong 化して返す。
		/// </summary>
		/// <returns>格納されている値</returns>
		internal abstract ulong GetValue();

		/// <summary>
		/// m～l ビット目に格納されている値を ulong 化して返す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>格納されている値</returns>
		/// <exception cref="BitFieldException">
		/// m &lt; l のとき発生。
		/// </exception>
		internal abstract ulong GetValue(int m, int l);

		/// <summary>
		/// 値を割り当てる。
		/// </summary>
		/// <param name="val">割り当てたい値(ulong)</param>
		public void Assign(ulong val)
		{
			BitField tmp = BitField.Create(this.Msb, this.Lsb, val);
			this.Assign(tmp);
		}

		/// <summary>
		/// 値を割り当てる。
		/// </summary>
		/// <param name="a">割り当てたい値の入ったビットフィールド。</param>
		/// <exception cref="BitFieldException">
		/// this.Width != a.Width のとき発生。
		/// </exception>
		public abstract void Assign(BitField a);

		/// <summary>
		/// m～l ビット目に値を割り当てる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <param name="a">割り当てたい値の入ったビットフィールド</param>
		/// <exception cref="BitFieldException">
		/// this.Sub(m, l).Width != a.Width のとき発生。
		/// </exception>
		public abstract void Assign(int m, int l, BitField a);

		/// <summary>
		/// ビット幅。
		/// </summary>
		public int Width
		{
			get{return this.Msb - this.Lsb + 1;}
		}

		/// <summary>
		/// MSBを取得。
		/// </summary>
		public abstract int Msb
		{
			get;
		}

		/// <summary>
		/// LSBを取得。
		/// </summary>
		public abstract int Lsb
		{
			get;
		}

		/// <summary>
		/// i ビット目の値を読み書き。
		/// </summary>
		/// <exception cref="BitFieldException">
		/// i が範囲外のとき発生。
		/// </exception>
		public BitField this[int i]
		{
			set
			{
				this.Assign(i, i, value);
			}
			get
			{
				return this.Sub(i);
			}
		}

		/// <summary>
		/// m～l ビット目の値を読み書き。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <exception cref="BitFieldException">
		/// m, l  が範囲外のときと m &lt; l のとき発生。
		/// </exception>
		public BitField this[int m, int l]
		{
			set
			{
				this.Assign(m, l, value);
			}
			get
			{
				return this.Sub(m, l);
			}
		}

		/// <summary>
		/// 格納されている値を ulong 化して返す。
		/// </summary>
		public ulong Value
		{
			get
			{
				return GetValue();
			}
		}

		/// <summary>
		/// i ビット目のみを切り出す。
		/// verilog の <c>z[i]</c> に相当。
		/// <c>z.Sub(i)</c> を書き換えると <c>z</c> そのものも書き換えられる。
		/// </summary>
		/// <param name="i">切り出したいビットのインデックス</param>
		/// <returns>切り出されたビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// i が範囲外のとき発生。
		/// </exception>
		internal BitField Sub(int i)
		{
			return Sub(i, i);
		}

		/// <summary>
		/// m～l ビット目のみを切り出す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// <c>z.Sub(m, l)</c> を書き換えると <c>z</c> そのものも書き換えられる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>切り出されたビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// m, l  が範囲外のときと m &lt; l のとき発生。
		/// </exception>
		internal abstract BitField Sub(int m, int l);

		/// <summary>
		/// 2つのビットフィールドを結合する。
		/// verilog の <c>{x, y, z}</c> に相当。
		/// いくつでもつなげられる。
		/// </summary>
		/// <param name="a">つなげたいビットフィールド</param>
		/// <returns>つながったビットフィールド</returns>
		static public BitField Concat(params BitField[] a)
		{
			ulong val = 0L;
			int width = 0;

			foreach(BitField bf in a)
			{
				width += bf.Width;
				val <<= bf.Width;
				val |= bf.Value;
			}
			return new BitFieldImmediate(width - 1, 0, val);
		}

		/// <summary>
		/// 各ビットに対して AND 演算。
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド2</param>
		/// <returns>計算結果</returns>
		/// <exception cref="BitFieldException">
		/// a.Width != b.Width のとき発生。
		/// </exception>
		static public BitField operator& (BitField a, BitField b)
		{
			if(a.Width != b.Width)
				throw new BitFieldException("width not match");

			ulong val = a.Value & b.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 各ビットに対して OR 演算。
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド2</param>
		/// <returns>計算結果</returns>
		/// <exception cref="BitFieldException">
		/// a.Width != b.Width のとき発生。
		/// </exception>
		static public BitField operator| (BitField a, BitField b)
		{
			if(a.Width != b.Width)
				throw new BitFieldException("width not match");

			ulong val = a.Value | b.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 各ビットに対して XOR 演算。
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド2</param>
		/// <returns>計算結果</returns>
		/// <exception cref="BitFieldException">
		/// a.Width != b.Width のとき発生。
		/// </exception>
		static public BitField operator^ (BitField a, BitField b)
		{
			if(a.Width != b.Width)
				throw new BitFieldException("width not match");

			ulong val = a.Value ^ b.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 加算。
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド2</param>
		/// <returns>計算結果</returns>
		static public BitField operator+ (BitField a, BitField b)
		{
			if(a.Width != b.Width)
				throw new BitFieldException("width not match");

			ulong val = a.Value + b.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 減算。
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド2</param>
		/// <returns>計算結果</returns>
		static public BitField operator- (BitField a, BitField b)
		{
			if(a.Width != b.Width)
				throw new BitFieldException("width not match");

			ulong val = a.Value - b.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 符号反転。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>計算結果</returns>
		static public BitField operator- (BitField a)
		{//! Max
			ulong val = ~a.Value + 1;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// 補数。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>計算結果</returns>
		static public BitField operator~ (BitField a)
		{//! Max
			ulong val = ~a.Value;
			return new BitFieldImmediate(a.Msb, a.Lsb, val);
		}

		/// <summary>
		/// ブール値からビットフィールドに変換
		/// </summary>
		/// <param name="b">ビットフィールドの真理値</param>
		/// <returns>ビットフィールド</returns>
		static public implicit operator BitField(bool b)
		{
			return BitField.Create(b);
		}

		/// <summary>
		/// ビットフィールドからブール値に変換
		/// </summary>
		/// <param name="bf">ビットフィールド</param>
		/// <returns>ビットフィールドの真偽</returns>
		static public implicit operator bool(BitField bf)
		{
			return bf.Value != 0;
		}

		static public bool operator true(BitField bf)
		{
			return bf.Value != 0;
		}

		static public bool operator false(BitField bf)
		{
			return bf.Value == 0;
		}

		/// <summary>
		/// 文字列化。
		/// 0 と 1 の羅列。
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string str = "";

			for(int i=this.Msb; i>=this.Lsb; --i)
			{
				str += this[i] ? "1" : "0";
			}

			return str;
		}
	}//class BitField

	/// <summary>
	/// 値を直接格納しているビットフィールド。
	/// 普通に <c>BitFiled.Create()</c> で BitField を作るとこいつができる。
	/// </summary>
	internal class BitFieldImmediate : BitField
	{
		int msb; // MSB
		int lsb; // LSB
		ulong n; // 値

		/// <summary>
		/// MSB が m、 LSB が l のビットフィールドを作成。
		/// </summary>
		/// <param name="msb">MSB</param>
		/// <param name="lsb">LSB</param>
		public BitFieldImmediate(int msb, int lsb) : this(msb, lsb, 0){}

		/// <summary>
		/// MSB が m、 LSB が l のビットフィールドを作成。
		/// 値の初期化も行う。
		/// </summary>
		/// <param name="msb">MSB</param>
		/// <param name="lsb">LSB</param>
		/// <param name="n">初期値</param>
		public BitFieldImmediate(int msb, int lsb, ulong n)
		{
			if(this.msb < this.lsb)
			{
				throw new BitFieldException("msb must be greater than lsb");
			}

			this.msb = msb;
			this.lsb = lsb;
			this.n = n << lsb;
		}

		/// <summary>
		/// 格納されている値を ulong 化して返す。
		/// </summary>
		/// <returns>格納されている値</returns>
		internal override ulong GetValue()
		{
			return (this.n & this.GetMask(this.msb, this.lsb)) >> this.lsb;
		}

		/// <summary>
		/// m～l ビット目に格納されている値を ulong 化して返す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>格納されている値</returns>
		/// <exception cref="BitFieldException">
		/// m &lt; l のとき発生。
		/// </exception>
		internal override ulong GetValue(int m, int l)
		{
			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");

			return (this.n & this.GetMask(m, l)) >> l;
		}

		/// <summary>
		/// 値を割り当てる。
		/// </summary>
		/// <param name="a">割り当てたい値の入ったビットフィールド。</param>
		/// <exception cref="BitFieldException">
		/// this.Width != a.Width のとき発生。
		/// </exception>
		public override void Assign(BitField a)
		{
			if(a.Width != this.Width)
				throw new BitFieldException("width don't match");

			ulong val = a.Value;
			this.n = val << this.lsb;
		}

		/// <summary>
		/// m～l ビット目に値を割り当てる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <param name="a">割り当てたい値の入ったビットフィールド</param>
		/// <exception cref="BitFieldException">
		/// this.Sub(m, l).Width != a.Width のとき発生。
		/// </exception>
		public override void Assign(int m, int l, BitField a)
		{
			if(a.Width != m-l+1)
				throw new BitFieldException("width don't match");

			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");

			ulong val = a.Value;
			ulong mask = this.GetMask(m, l);
			val <<= l;
			val &= mask;

			this.n = val | (this.n & ~mask);
		}

		/// <summary>
		/// MSBを取得。
		/// </summary>
		public override int Msb
		{
			get
			{
				return this.msb;
			}
		}

		/// <summary>
		/// LSBを取得。
		/// </summary>
		public override int Lsb
		{
			get
			{
				return this.lsb;
			}
		}

		/// <summary>
		/// m～l ビット目のみを切り出す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// <c>z.Sub(m, l)</c> を書き換えると <c>z</c> そのものも書き換えられる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>切り出されたビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// m, l  が範囲外のときと m &lt; l のとき発生。
		/// </exception>
		internal override BitField Sub(int m, int l)
		{
			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");
			
			return new BitFieldSub(m, l, this);
		}

		/// <summary>
		/// m～l ビット目のみ 1 のマスクを生成。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>マスク</returns>
		private ulong GetMask(int m, int l)
		{
			ulong mask = 0UL;

			for(int i=l; i<=m; ++i)
				mask |= 1UL << i;

			return mask;
		}
	}//class BitFieldImmediate

	/// <summary>
	/// <c>BitField.Sub</c> で取り出す部分ビットフィールド。
	/// </summary>
	internal class BitFieldSub : BitField
	{
		int msb;
		int lsb;
		BitField bf;

		/// <summary>
		/// bf の msb～lsb 目。
		/// </summary>
		/// <param name="msb">MSB</param>
		/// <param name="lsb">LSB</param>
		/// <param name="bf">元となるビットフィールド</param>
		public BitFieldSub(int msb, int lsb, BitField bf)
		{
			this.msb = msb;
			this.lsb = lsb;
			this.bf = bf;
		}

		/// <summary>
		/// 格納されている値を ulong 化して返す。
		/// </summary>
		/// <returns>格納されている値</returns>
		internal override ulong GetValue()
		{
			return this.bf.GetValue(this.msb, this.lsb);
		}

		/// <summary>
		/// m～l ビット目に格納されている値を ulong 化して返す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>格納されている値</returns>
		/// <exception cref="BitFieldException">
		/// m &lt; l のとき発生。
		/// </exception>
		internal override ulong GetValue(int m, int l)
		{
			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");

			return this.bf.GetValue(m, l);
		}

		/// <summary>
		/// 値を割り当てる。
		/// </summary>
		/// <param name="a">割り当てたい値の入ったビットフィールド。</param>
		/// <exception cref="BitFieldException">
		/// this.Width != a.Width のとき発生。
		/// </exception>
		public override void Assign(BitField a)
		{
			if(a.Width != this.Width)
				throw new BitFieldException("width don't match");

			this.bf.Assign(this.msb, this.lsb, a);
		}

		/// <summary>
		/// m～l ビット目に値を割り当てる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <param name="a">割り当てたい値の入ったビットフィールド</param>
		/// <exception cref="BitFieldException">
		/// this.Sub(m, l).Width != a.Width のとき発生。
		/// </exception>
		public override void Assign(int m, int l, BitField a)
		{
			if(a.Width != m-l+1)
				throw new BitFieldException("width don't match");

			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");

			this.bf.Assign(m, l, a);
		}

		/// <summary>
		/// MSBを取得。
		/// </summary>
		public override int Msb
		{
			get
			{
				return this.msb;
			}
		}

		/// <summary>
		/// LSBを取得。
		/// </summary>
		public override int Lsb
		{
			get
			{
				return this.lsb;
			}
		}

		/// <summary>
		/// m～l ビット目のみを切り出す。
		/// verilog の <c>z[m:l]</c> に相当。
		/// <c>z.Sub(m, l)</c> を書き換えると <c>z</c> そのものも書き換えられる。
		/// </summary>
		/// <param name="m">MSB</param>
		/// <param name="l">LSB</param>
		/// <returns>切り出されたビットフィールド</returns>
		/// <exception cref="BitFieldException">
		/// m, l  が範囲外のときと m &lt; l のとき発生。
		/// </exception>
		internal override BitField Sub(int m, int l)
		{
			if(m < l || this.msb < m || this.lsb > l)
				throw new BitFieldException("illegal range");
			
			return new BitFieldSub(m, l, this);
		}
	}//class BitFieldSub
}//namespace BitField
