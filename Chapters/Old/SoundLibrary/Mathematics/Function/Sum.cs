using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 関数の和。
	/// </summary>
	public class Sum : Series
	{
		#region 初期化

		public Sum(params Function[] functions) : base(functions) {}
		public Sum(ArrayList functions) : base(functions) {}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			System.Collections.ArrayList list = new System.Collections.ArrayList();

			foreach(Function f in this.functions)
			{
				list = Misc.Merge(list, f.GetVariableList());
			}
			return list;
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			ValueType y = 0;

			foreach(Function f in this.functions)
			{
				y += f.GetValue(x);
			}

			return y;
		}

		public override Function Bind(params Parameter[] x)
		{
			Function y = (Constant)0;

			foreach(Function f in this.functions)
			{
				Function g = f.Bind(x);
				y += g;
			}

			return y;
		}

		#endregion
		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			Function f;

			f = this.functions[0] as Function;
			f.GetComplexPart(out re, out im);

			for(int i=1; i<this.functions.Count; ++i)
			{
				Function re1, im1;

				f = this.functions[i] as Function;
				f.GetComplexPart(out re1, out im1);

				re += re1;
				im += im1;
			}
		}

		#endregion
		#region 演算子

		public override Function Add(Function f)
		{
			Sum h = (Sum)this.Clone();

			if(f is Sum)
			{
				Sum g = f as Sum;
				h.AddList(g.functions);
			}
			else
			{
				h.AddList(f);
			}

			return h;
		}

		#endregion
		#region 微分

		public override Function Differentiate(Variable x)
		{
			ArrayList func = new ArrayList();

			foreach(Function f in this.functions)
			{
				func.Add(f.Differentiate(x));
			}

			return new Sum(func);
		}

		#endregion
		#region 内部構造の最適化

		/// <remarks>
		/// 同じ種類の関数ごとに足していった方が最適な構造が得られるため、
		/// 関数のリストを一度ソートしてから足しなおす。
		/// あと、定数 0 が混ざってたら消す。
		/// </remarks>
		public override Function Optimize()
		{
			Hashtable table = new Hashtable();

			// 種類わけ
			foreach(Function f in this.functions)
			{
				Function g = f.Optimize();

				Type t = g.GetType();
				if(table[t] == null)
				{
					table[t] = g;
				}
				else
				{
					table[t] = (Function)table[t] + g;
				}
			}

			// 繋ぎなおす
			ArrayList func = new ArrayList();

			foreach(Function f in table.Values)
			{
				if(f is Sum)
				{
					Sum s = f as Sum;
					func.AddRange(s.functions);
				}
				else
				{
					if(!f.Equals((Constant)0))
						func.Add(f);
				}
			}

			// 特殊な場合
			if(func.Count == 0)
				return (Constant)0;

			if(func.Count == 1)
				return func[0] as Function;

			// 足しなおす。
			Function h = func[0] as Function;
			for(int i=1; i<func.Count; ++i)
			{
				h += func[i] as Function;
			}

			return h;
		}

		#endregion
		#region object

		public override bool Equals(object obj)
		{
			if(!(obj is Sum)) return false;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			int code = base.GetHashCode();
			code ^= 83742673;
			return code;
		}

		public override string ToString()
		{
			string str = "";
#if DEBUG
			str = "s";
#endif
			str += "(" + this.functions[0].ToString();

			for(int i=1; i<this.functions.Count; ++i)
			{
				string tmp = this.functions[i].ToString();
				if(tmp[0] == '-')
				{
					str += " " + tmp;
				}
				else
				{
					str += " + " + tmp;
				}
			}

			str += ")";

			return str;
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Sum((ArrayList)this.functions.Clone());
		}

		#endregion
	}
}
