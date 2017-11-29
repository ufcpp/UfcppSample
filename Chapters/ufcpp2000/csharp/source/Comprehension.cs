using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ConsoleApplication1
{
	class Program
	{
		/// <summary>
		/// 以下のような、総当りで解を求める類の問題を、
		/// LINQ とかを使って解いてみる。
		/// 
		/// 設問：
		/// Baker, Cooper, Fletcher, MillerとSmithは五階建てアパートの異なる階に住んでいる。
		/// Bakerは最上階に住むのではない。
		/// Cooperは最下階に住むのではない。
		/// Fletcherは最上階にも最下階にも住むのではない。
		/// MillerはCooperより上の階に住んでいる。
		/// SmithはFletcherの隣の階に住むのではない。
		/// FletcherはCooperの隣の階に住むのではない。
		/// それぞれはどの階に住んでいるか。 
		/// </summary>
		/// <remarks>
		/// 実行結果のまとめ：
		/// ・この手の単純なクエリは、
		///   クエリの順序入れ替えで10倍以上パフォーマンスがあがることがざら。
		/// ・でも、from が前に固まってるものと比べて、順序を入れ替えたクエリは結構見づらい。
		/// ・多重 from を使ったクエリ式をメソッド形式で書こうとすると SelectMany の透過識別子がえらいことに。
		/// ・from, where, select を foreach, if, yield return で展開すると、パフォーマンス1.5～2倍くらいあがったりする。
		/// ・yield return（いわゆるイテレータ）と、一度 List.Add してからその List を返すののパフォーマンスは大差なし。
		/// 
		/// 結論：
		/// IQueryable / ラムダ式を使って、
		/// from, where, select を foreach, if, yield return に展開
		/// ＆ クエリの順序最適化をかけるようなライブラリが欲しいなぁ。
		/// </remarks>
		static void Main()
		{
			// 設問どおりの順序でクエリ
			var answers1 =
				from baker in five
				from cooper in five
				from fletcher in five
				from miller in five
				from smith in five
				where Distinct(baker, cooper, fletcher, miller, smith)
				where baker != 5
				where cooper != 1
				where fletcher != 1 && fletcher != 5
				where miller > cooper
				where Discrete(smith, fletcher)
				where Discrete(fletcher, cooper)
				select new { baker, cooper, fletcher, miller, smith };

			// answers1 のクエリ式と等価なクエリ演算
			var answers0 = five
				.SelectMany(x => five, (baker, cooper) => new { baker, cooper })
				.SelectMany(x => five, (x, fletcher) => new { x, fletcher })
				.SelectMany(x => five, (x, miller) => new { x, miller })
				.SelectMany(x => five, (x, smith) => new { x, smith })
				.Where(x => Distinct(x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith))
				.Where(x => x.x.x.x.baker != 5)
				.Where(x => x.x.x.x.cooper != 1)
				.Where(x => x.x.x.fletcher != 1 && x.x.x.fletcher != 5)
				.Where(x => x.x.miller > x.x.x.x.cooper)
				.Where(x => Discrete(x.smith, x.x.x.fletcher))
				.Where(x => Discrete(x.x.x.fletcher, x.x.x.x.cooper))
				.Select(x => new { x.x.x.x.baker, x.x.x.x.cooper, x.x.x.fletcher, x.x.miller, x.smith });

			// answers0 の透過識別子をちょっと整理
			var answers01 = five
				.SelectMany(x => five, (baker, cooper) => new { baker, cooper })
				.SelectMany(x => five, (x, fletcher) => new { x.baker, x.cooper, fletcher })
				.SelectMany(x => five, (x, miller) => new { x.baker, x.cooper, x.fletcher, miller })
				.SelectMany(x => five, (x, smith) => new { x.baker, x.cooper, x.fletcher, x.miller, smith })
				.Where(x => Distinct(x.baker, x.cooper, x.fletcher, x.miller, x.smith))
				.Where(x => x.baker != 5)
				.Where(x => x.cooper != 1)
				.Where(x => x.fletcher != 1 && x.fletcher != 5)
				.Where(x => x.miller > x.cooper)
				.Where(x => Discrete(x.smith, x.fletcher))
				.Where(x => Discrete(x.fletcher, x.cooper));

			// answers1 の from, where の順序を入れ替えて最適化
			var answers2 =
				from baker in five
				where baker != 5
				from cooper in five
				where cooper != 1
				from fletcher in five
				where fletcher != 1 && fletcher != 5
				where Discrete(fletcher, cooper)
				from miller in five
				where miller > cooper
				from smith in five
				where Discrete(smith, fletcher)
				where Distinct(baker, cooper, fletcher, miller, smith)
				select new { baker, cooper, fletcher, miller, smith };

			// answers2 とほぼ等価（透過識別子だけ整理）なクエリ演算
			var answers02 = five
				.Where(baker => baker != 5)
				.SelectMany(x => five, (baker, cooper) => new { baker, cooper })
				.Where(x => x.cooper != 1)
				.SelectMany(x => five, (x, fletcher) => new { x.baker, x.cooper, fletcher })
				.Where(x => x.fletcher != 1 && x.fletcher != 5)
				.Where(x => Discrete(x.fletcher, x.cooper))
				.SelectMany(x => five, (x, miller) => new { x.baker, x.cooper, x.fletcher, miller })
				.Where(x => x.miller > x.cooper)
				.SelectMany(x => five, (x, smith) => new { x.baker, x.cooper, x.fletcher, x.miller, smith })
				.Where(x => Discrete(x.smith, x.fletcher))
				.Where(x => Distinct(x.baker, x.cooper, x.fletcher, x.miller, x.smith));

			CheckPerformance(answers1, "クエリ式 　　　");
			CheckPerformance(answers0, "メソッド (等価)");
			CheckPerformance(answers01, "メソッド 　　　");
			CheckPerformance(YieldAnswers1(), "yield    　　　");
			CheckPerformance<Tuple>(ListAnswers1, "list     　　　");

			CheckPerformance(answers2, "クエリ式 最適化");
			CheckPerformance(answers02, "メソッド 最適化");
			CheckPerformance(YieldAnswers2(), "yield    最適化");
			CheckPerformance<Tuple>(ListAnswers2, "list     最適化");
		}

		#region 補助関数

		// 1～5
		static IEnumerable<int> five = Enumerable.Range(1, 5);

		// x の要素に重複がないとき true
		static bool Distinct(params int[] x)
		{
			return x.Distinct().Count() == x.Length;
		}

		// x, y が隣り合う数字でないとき true
		static bool Discrete(int x, int y)
		{
			return Math.Abs(checked(x - y)) != 1;
		}

		#endregion
		#region パフォーマンス計測

		const int N = 500;
		static bool quiet = true;

		/// <summary>
		/// クエリのパフォーマンスの確認。
		/// シーケンスを N 回 ToList() するのにかかる時間を計測。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="seq">パフォーマンスを計りたいシーケンス</param>
		/// <param name="label">結果表示用のラベル</param>
		static void CheckPerformance<T>(IEnumerable<T> seq, string label)
		{
			var sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < N; ++i)
			{
				var x = seq.ToList();
			}
			sw.Stop();
			if (!quiet)
				foreach (var x in seq)
					Console.WriteLine(x);
			Console.Write(label + " {0}\n", sw.ElapsedMilliseconds);
		}

		// 比較用。リスト版。
		static void CheckPerformance<T>(Func<List<T>> getList, string label)
		{
			var sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < N; ++i)
			{
				var x = getList();
			}
			sw.Stop();
			if (!quiet)
				foreach (var x in getList())
					Console.WriteLine(x);
			Console.Write(label + " {0}\n", sw.ElapsedMilliseconds);
		}

		#endregion
		#region イテレータ版

		/// <summary>
		/// 比較のため、イテレータ版を作りたいけど、
		/// イテレータは匿名メソッドでは作れない（＝ 匿名型を返せない）ので
		/// 等価な型を作成。
		/// </summary>
		struct Tuple
		{
			public int baker { get; set; }
			public int cooper { get; set; }
			public int fletcher { get; set; }
			public int miller { get; set; }
			public int smith { get; set; }

			public override string ToString()
			{
				return
					"{ " +
					string.Format("baker = {0}, cooper = {1}, fletcher = {2}, miller = {3}, smith = {4}",
						baker, cooper, fletcher, miller, smith) +
					" }";
			}
		}

		// answers1 相当のイテレータ
		static IEnumerable<Tuple> YieldAnswers1()
		{
			foreach (var baker in five)
			foreach (var cooper in five)
			foreach (var fletcher in five)
			foreach (var miller in five)
			foreach (var smith in five)
			if (Distinct(baker, cooper, fletcher, miller, smith))
			if (baker != 5)
			if (cooper != 1)
			if (fletcher != 1 && fletcher != 5)
			if (miller > cooper)
			if (Discrete(smith, fletcher))
			if (Discrete(fletcher, cooper))
			yield return new Tuple { baker = baker, cooper = cooper, fletcher = fletcher, miller = miller, smith = smith };
		}

		// answers2 相当のイテレータ
		static IEnumerable<Tuple> YieldAnswers2()
		{
			foreach (var baker in five)
			if (baker != 5)
			foreach (var cooper in five)
			if (cooper != 1)
			foreach (var fletcher in five)
			if (fletcher != 1 && fletcher != 5)
			if (Discrete(fletcher, cooper))
			foreach (var miller in five)
			if (miller > cooper)
			foreach (var smith in five)
			if (Discrete(smith, fletcher))
			if (Distinct(baker, cooper, fletcher, miller, smith))
			yield return new Tuple { baker = baker, cooper = cooper, fletcher = fletcher, miller = miller, smith = smith };
		}

		#endregion
		#region リスト版

		// 比較用。YieldAnswers1 のリスト版
		static List<Tuple> ListAnswers1()
		{
			var list = new List<Tuple>();
			foreach (var baker in five)
			foreach (var cooper in five)
			foreach (var fletcher in five)
			foreach (var miller in five)
			foreach (var smith in five)
			if (Distinct(baker, cooper, fletcher, miller, smith))
			if (baker != 5)
			if (cooper != 1)
			if (fletcher != 1 && fletcher != 5)
			if (miller > cooper)
			if (Discrete(smith, fletcher))
			if (Discrete(fletcher, cooper))
			list.Add(new Tuple { baker = baker, cooper = cooper, fletcher = fletcher, miller = miller, smith = smith });
			return list;
		}

		// 比較用。YieldAnswers2 のリスト版
		static List<Tuple> ListAnswers2()
		{
			var list = new List<Tuple>();
			foreach (var baker in five)
			if (baker != 5)
			foreach (var cooper in five)
			if (cooper != 1)
			foreach (var fletcher in five)
			if (fletcher != 1 && fletcher != 5)
			if (Discrete(fletcher, cooper))
			foreach (var miller in five)
			if (miller > cooper)
			foreach (var smith in five)
			if (Discrete(smith, fletcher))
			if (Distinct(baker, cooper, fletcher, miller, smith))
			list.Add(new Tuple { baker = baker, cooper = cooper, fletcher = fletcher, miller = miller, smith = smith });
			return list;
		}

		#endregion
	}
}
