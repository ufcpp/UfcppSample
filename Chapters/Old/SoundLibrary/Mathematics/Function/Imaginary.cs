using System;

namespace SoundLibrary.Mathematics.Function
{
	/// <summary>
	/// 虚数倍。
	/// ×i (i は虚数単位)。
	/// GetValue (実数値計算)には適用されない。
	/// </summary>
	public class Imaginary : Function
	{
		Function f;

		public Imaginary(Function f)
		{
			this.f = f;
		}

		public override System.Collections.ArrayList GetVariableList()
		{
			return this.f.GetVariableList();
		}

		public override Function Bind(params SoundLibrary.Mathematics.Function.Function.Parameter[] x)
		{
			return new Imaginary(this.f.Bind(x));
		}

		public override Double GetValue(params SoundLibrary.Mathematics.Function.Function.Parameter[] x)
		{
			return this.f.GetValue(x);
		}

		public override void GetComplexPart(out Function re, out Function im)
		{
			Function r, i;
			this.f.GetComplexPart(out r, out i);
			re = -i;
			im = r;
		}

		public override Function Clone()
		{
			return new Imaginary(this.f.Clone());
		}
	}
}
