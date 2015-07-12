using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// テスト用のデータ生成インターフェース
	/// </summary>
	public interface IDataGenerator : ICloneable
	{
		/// <summary>
		/// 次のデータを取り出す。
		/// </summary>
		/// <returns>データ</returns>
		double Next();

		/// <summary>
		/// 初期状態に戻す。
		/// </summary>
		void Reset();
	}
}//namespace test
