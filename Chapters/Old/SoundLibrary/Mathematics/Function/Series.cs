using System;
using System.Collections;

namespace SoundLibrary.Mathematics.Function
{
	/// <summary>
	/// Sum(累和)/Product(累積)クラスの共通基底。
	/// </summary>
	public abstract class Series : Function
	{
		#region フィールド

		protected ArrayList functions = new ArrayList();

		#endregion
		#region 初期化

		public Series(params Function[] functions)
		{
			foreach(Function f in functions)
			{
				this.functions.Add(f);
			}
		}

		public Series(ArrayList functions)
		{
			this.functions = functions;
		}

		#endregion
		#region 関数の追加

		/// <summary>
		/// 加算対象リストに関数を追加する。
		/// </summary>
		/// <param name="f">追加する関数</param>
		internal void AddList(Function f)
		{
			if(f is Constant)
			{
				Constant c = f as Constant;
				if(c.Value == 0)
					return;
			}

			this.functions.Add(f);
		}

		/// <summary>
		/// 加算対象リストに関数を追加する。
		/// </summary>
		/// <param name="list">追加する関数のリスト</param>
		internal void AddList(IList list)
		{
			foreach(Function f in list)
				this.AddList(f);
		}

		#endregion
		#region object

		public override bool Equals(object obj)
		{
			Series s = obj as Series;
			if(s == null) return false;
			if(this.functions.Count != s.functions.Count) return false;

			for(int i=0; i<this.functions.Count; ++i)
			{
				if(!this.functions[i].Equals(s.functions[i])) return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			int code = 0;
			foreach(Function f in this.functions)
			{
				code <<= 1;
				code ^= f.GetHashCode();
			}

			return code;
		}

		#endregion
	}
}
