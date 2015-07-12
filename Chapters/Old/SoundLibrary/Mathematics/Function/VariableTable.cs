using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 関数のパラメータ変数のテーブル。
	/// </summary>
	/// <example>
	/// Variable x = ....;
	/// Variable y = ....;
	/// Function f = ....;
	/// VariableTalbe t = f.GetVariableTable();
	/// t[x] = 1;
	/// t[y] = 2;
	/// ValueType result = f[t];
	/// </example>
	public class VariableTable : ICloneable
	{
		Hashtable table;

		VariableTable(Hashtable table)
		{
			this.table = table;
		}

		public VariableTable(params Variable[] list) : this((IList)list)
		{
		}

		public VariableTable(IList list)
		{
			this.table = new Hashtable();

			foreach(Variable v in list)
			{
				this.table.Add(v, (ValueType)0);
			}
		}

		public Function.Parameter[] GetParameterList()
		{
			Function.Parameter[] p = new Function.Parameter[this.table.Count];

			int i = 0;
			foreach(DictionaryEntry entry in this.table)
			{
				p[i] = new Function.Parameter((Variable)entry.Key, (ValueType)entry.Value);
				++i;
			}

			return p;
		}

		public ValueType this[Variable v]
		{
			get{return (ValueType)this.table[v];}
			set{this.table[v] = value;}
		}

		public VariableTable Clone()
		{
			return new VariableTable((Hashtable)this.table.Clone());
		}

		#region ICloneable メンバ

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
	}
}
