using System;

namespace SoundLibrary.Mathematics.Function
{
	/// <summary>
	/// 複素数変数。
	/// </summary>
	public class ComplexVariable : Variable
	{
		public ComplexVariable(IComparable id) : base(id) {}

		#region 複素数対応

		/// <summary>
		/// 実部。
		/// </summary>
		public Variable Re
		{
			get{return new Variable(new ComplexId(ComplexId.ReIm.Re, this.id));}
		}

		/// <summary>
		/// 虚部。
		/// </summary>
		public Variable Im
		{
			get{return new Variable(new ComplexId(ComplexId.ReIm.Im, this.id));}
		}

		public override void GetComplexPart(out Function re, out Function im)
		{
			re = this.Re;
			im = this.Im;
		}

		#endregion
	}

	/// <summary>
	/// 純虚数変数。
	/// </summary>
	public class ImaginaryVariable : Variable
	{
		public ImaginaryVariable(IComparable id) : base(id) {}

		#region 複素数対応

		public override void GetComplexPart(out Function re, out Function im)
		{
			re = (Constant)0;
			im = this;
		}

		#endregion
	}

	/// <summary>
	/// Variable を複素数対応化するためのクラス。
	/// Variable x が id 'x' を持つとき、
	/// x.GetComplexPart(out re, out im); の結果 re, im を
	/// re の id = ComplexId(Re, 'x'),
	/// im の id = ComplexId(Im, 'x')
	/// にする。
	/// </summary>
	internal struct ComplexId : IComparable
	{
		#region 内部型

		public enum ReIm
		{
			Re, Im
		}

		#endregion
		#region フィールド

		public ReIm ri;
		public IComparable id;

		#endregion
		#region 初期化

		public ComplexId(ReIm ri, IComparable id)
		{
			this.ri = ri;
			this.id = id;
		}

		#endregion
		#region object

		public override bool Equals(object obj)
		{
			ComplexId id = (ComplexId)obj;
			return this.ri == id.ri && this.id.Equals(id.id);
		}

		public override int GetHashCode()
		{
			return this.ri.GetHashCode() ^ this.id.GetHashCode();
		}

		public override string ToString()
		{
			if(this.ri == ReIm.Re)
			{
				return "Re(" + this.id.ToString() + ")";
			}
			else
			{
				return "Im(" + this.id.ToString() + ")";
			}
		}

		#endregion
		#region IComparable メンバ

		public int CompareTo(object obj)
		{
			ComplexId id = (ComplexId)obj;

			int comp = this.id.CompareTo(id.id);

			if(comp != 0) return comp;
			
			return this.ri.CompareTo(id.ri);
		}

		#endregion
	}
}
