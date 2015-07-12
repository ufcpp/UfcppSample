using System;

namespace SoundLibrary.Mathematics.Function
{
	using ValueType = System.Double;

	/// <summary>
	/// 変数。
	/// </summary>
	public class Variable : Function, IComparable
	{
		#region フィールド

		/// <summary>
		/// 変数同士を区別するための id。
		/// </summary>
		protected IComparable id;

		#endregion
		#region 初期化

		public Variable() : this((int)0){}

		public Variable(IComparable id)
		{
			this.id = id;
		}

		#endregion
		#region 値

		public IComparable ID
		{
			get{return this.id;}
		}

		#endregion
		#region 値の計算

		public override System.Collections.ArrayList GetVariableList()
		{
			System.Collections.ArrayList list = new System.Collections.ArrayList();
			list.Add(this);
			return list;
		}

		public override ValueType GetValue(params Parameter[] x)
		{
			foreach(Parameter p in x)
			{
				if(p.x.Equals(this))
				{
					return p.val;
				}
			}
			return 0; //! エラー
		}

		public override Function Bind(params Parameter[] x)
		{
			foreach(Parameter p in x)
			{
				if(this.Equals(p.x))
				{
					return (Constant)p.val;
				}
			}
			return this;
		}

		#endregion
		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			re = this;
			im = (Constant)0;
		}

		#endregion
		#region 演算子

		public override Function Add(Function f)
		{
			Variable g = f as Variable;
			if(g != null && this.Equals(g))
			{
				return new Multiple(2, this);
			}

			if(f is Constant)
			{
				return base.Add(f);
			}

			return f.Add(this);
		}

		public override Function Multiply(Function f)
		{
			Constant c = f as Constant;
			if(c != null)
			{
				return new Multiple(c.Value, this);
			}

			if(f is Variable)
			{
				return base.Multiply(f);
			}

			return f.Multiply(this);
		}

		#endregion
		#region 微分

		public override Function Differentiate(Variable x)
		{
			if(this.Equals(x))
			{
				return (Constant)1;
			}
			return (Constant)0;
		}

		#endregion
		#region ICloneable

		public override Function Clone()
		{
			return new Variable(this.id);
		}

		#endregion
		#region object

		public override string ToString()
		{
			return this.id.ToString();
		}

		public override bool Equals(object obj)
		{
			Variable var = obj as Variable;

			if(var == null)
			{
				return false;
			}

			return this.id.Equals(var.id);
		}

		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}


		#endregion
		#region IComparable メンバ

		public int CompareTo(object obj)
		{
			Variable v = (Variable)obj;

			if(this.id.GetType().Equals(v.id.GetType()))
			{
				return this.id.CompareTo(v.id);
			}

			return this.id.GetType().GUID.CompareTo(v.id.GetType().GUID);
		}

		#endregion
	}
}
