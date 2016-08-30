using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text;
using System.Reflection;

namespace ExpressionTree
{
	class ExpressionTest
	{
		static public void Main()
		{
			Lambda();
			ArithmeticUnaryOperator();
			ArithmeticBinaryOperator();
			ComparisonOperator();
			LogicalOperator();
			OtherOperator();
			TypeOperator();
			MemberAccess();
			New();
			Call();
		}
		#region 2つの Expression が一致するかどうか確認

		static void SimpleCheck(Expression e1, Expression e2)
		{
			SimpleCheck(e1, e2, false);
		}

		/// <summary>
		/// 式木の構造が一致してれば、少なくとも ToString の結果は一致するので、
		/// それで2つの式木の一致性を判定。
		/// </summary>
		static void SimpleCheck(Expression e1, Expression e2, bool verbose)
		{
			if (e1.ToString() != e2.ToString())
			{
				Console.Write("not match: {0}, {1}\n", e1, e2);
			}
		}

		#endregion
		#region Paramter

		static ParameterExpression intX = Expression.Parameter(typeof(int), "x");
		static ParameterExpression intY = Expression.Parameter(typeof(int), "y");
		static ParameterExpression boolX = Expression.Parameter(typeof(bool), "x");
		static ParameterExpression boolY = Expression.Parameter(typeof(bool), "y");

		#endregion
		#region テスト

		static void Lambda()
		{
			// Constant
			SimpleCheck(
				Make.Expression(() => 5).Body,
				Expression.Constant(5)
				);

			// Paramter
			SimpleCheck(
				Make.Expression((int x) => x).Body,
				intX
				);

			// Lambda
			SimpleCheck(
				Make.Expression((int x) => 0),
				Expression.Lambda<Func<int, int>>(
					Expression.Constant(0), // Body
					intX) // Paremters[0]
				);
			SimpleCheck(
				Make.Expression((int x) => x + 5),
				Expression.Lambda(Expression.Add(intX, Expression.Constant(5)), intX)
				);

			// Quote
			SimpleCheck(
				Make.Expression(() => (Expression<Func<int>>)(() => 0)).Body,
				Expression.Convert(Expression.Quote(
					(Expression<Func<int>>)(() => 0)),
					typeof(Expression<Func<int>>))
				);
		}

		static void ArithmeticUnaryOperator()
		{
			// ↓これは最適化がかかって +x が x になる。
			SimpleCheck(
				Make.Expression((int x) => +x).Body,
				intX
			);
			SimpleCheck(
				Make.Expression((CustomUnaryPlus x) => +x).Body,
				Expression.UnaryPlus(Expression.Parameter(typeof(CustomUnaryPlus), "x"))
			);
			SimpleCheck(
				Make.Expression((int x) => -x).Body,
				Expression.Negate(intX)
			);
			SimpleCheck(
				Make.Expression((int x) => checked(-x)).Body,
				Expression.NegateChecked(intX)
			);
		}

		static void ArithmeticBinaryOperator()
		{
			// unchecked
			SimpleCheck(
				Make.Expression((int x, int y) => x + y).Body,
				Expression.Add(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => x - y).Body,
				Expression.Subtract(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => x * y).Body,
				Expression.Multiply(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => x / y).Body,
				Expression.Divide(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => x % y).Body,
				Expression.Modulo(intX, intY)
			);

			// checked
			SimpleCheck(
				Make.Expression((int x, int y) => checked(x + y)).Body,
				Expression.AddChecked(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => checked(x - y)).Body,
				Expression.SubtractChecked(intX, intY)
			);
			SimpleCheck(
				Make.Expression((int x, int y) => checked(x * y)).Body,
				Expression.MultiplyChecked(intX, intY)
			);

			// たとえ checked がついていても、
			// double 同士の演算はオーバーフローをチェックしない
			SimpleCheck(
				Make.Expression((double x, double y) => checked(x + y)).Body,
				Expression.Add(
					Expression.Parameter(typeof(double), "x"),
					Expression.Parameter(typeof(double), "y"))
			);

			// Power
		}

		static void ComparisonOperator()
		{
			SimpleCheck(
				Make.Expression((int x, int y) => x == y).Body,
				Expression.Equal(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x != y).Body,
				Expression.NotEqual(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x < y).Body,
				Expression.LessThan(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x <= y).Body,
				Expression.LessThanOrEqual(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x > y).Body,
				Expression.GreaterThan(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x >= y).Body,
				Expression.GreaterThanOrEqual(intX, intY)
				);
		}

		static void LogicalOperator()
		{
			SimpleCheck(
				Make.Expression((int x, int y) => x & y).Body,
				Expression.And(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x | y).Body,
				Expression.Or(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x ^ y).Body,
				Expression.ExclusiveOr(intX, intY)
				);

			SimpleCheck(
				Make.Expression((bool x) => !x).Body,
				Expression.Not(boolX)
				);
			SimpleCheck(
				Make.Expression((int x) => ~x).Body,
				Expression.Not(intX)
				);

			SimpleCheck(
				Make.Expression((bool x, bool y) => x && y).Body,
				Expression.AndAlso(boolX, boolY)
				);
			SimpleCheck(
				Make.Expression((bool x, bool y) => x || y).Body,
				Expression.OrElse(boolX, boolY)
				);
		}

		static void OtherOperator()
		{
			SimpleCheck(
				Make.Expression((int x, int y) => x << y).Body,
				Expression.LeftShift(intX, intY)
				);
			SimpleCheck(
				Make.Expression((int x, int y) => x >> y).Body,
				Expression.RightShift(intX, intY)
				);

			var nIntX = Expression.Parameter(typeof(int?), "x");
			SimpleCheck(
				Make.Expression((int? x, int y) => x ?? y).Body,
				Expression.Coalesce(nIntX, intY)
				);

			SimpleCheck(
				Make.Expression((int x, int y) => x > y ? x : y).Body,
				Expression.Condition(
					Expression.GreaterThan(intX, intY),
					intX, intY)
				);
		}

		static void TypeOperator()
		{
			var objX = Expression.Parameter(typeof(object), "x");
			SimpleCheck(
				Make.Expression((object x) => x as int?).Body,
				Expression.TypeAs(objX, typeof(int?))
				);
			SimpleCheck(
				Make.Expression((object x) => x is int).Body,
				Expression.TypeIs(objX, typeof(int))
				);

			SimpleCheck(
				Make.Expression((int x) => (byte)x).Body,
				Expression.Convert(intX, typeof(byte))
				);
			SimpleCheck(
				Make.Expression((int x) => checked((byte)x)).Body,
				Expression.ConvertChecked(intX, typeof(byte))
				);
		}

		static void MemberAccess()
		{
			var point = Expression.Parameter(typeof(Point), "p");
			var intArray = Expression.Parameter(typeof(int[]), "x");

			var xx = Make.Expression((int[] x) => x[0]).Body;

			SimpleCheck(
				Make.Expression((Point p) => p.X).Body,
				Expression.MakeMemberAccess(point, typeof(Point).GetProperty("X"))
				);

			SimpleCheck(
				Make.Expression((int[] x) => x.Length).Body,
				Expression.ArrayLength(intArray)
				);

			SimpleCheck(
				Make.Expression((int[] x) => x[0]).Body,
				Expression.ArrayIndex(intArray, Expression.Constant(0))
				);
		}

		static void New()
		{
			var pointCtor0 = typeof(Point).GetConstructor(new Type[0]);
			var pointCtor2 = typeof(Point).GetConstructor(new[] { typeof(int), typeof(int) });
			var c1 = Expression.Constant(1);
			var c2 = Expression.Constant(2);

			// new
			SimpleCheck(
				Make.Expression(() => new Point(1, 2)).Body,
				Expression.New(pointCtor2, c1, c2)
				);

			// array new
			SimpleCheck(
				Make.Expression(() => new int[] { 1, 2 }).Body,
				Expression.NewArrayInit(typeof(int), c1, c2)
				);

			SimpleCheck(
				Make.Expression(() => new int[2]).Body,
				Expression.NewArrayBounds(typeof(int), c2)
				);

			// new with object initializer
			SimpleCheck(
				Make.Expression(() => new Point { X = 1, Y = 2 }).Body,
				Expression.MemberInit(
					Expression.New(pointCtor0),
					Expression.Bind(typeof(Point).GetProperty("X"), c1),
					Expression.Bind(typeof(Point).GetProperty("Y"), c2))
				);

			// new with recursive object initializer
			var lineCtor = typeof(LineSegment).GetConstructor(new Type[0]);
			SimpleCheck(
				Make.Expression(() =>
				new LineSegment
				{
					Start = { X = 1, Y = 1 },
					End = { X = 2, Y = 2 }
				}).Body,
				Expression.MemberInit(
					Expression.New(lineCtor),
					Expression.MemberBind(typeof(LineSegment).GetProperty("Start"),
						Expression.Bind(typeof(Point).GetProperty("X"), c1),
						Expression.Bind(typeof(Point).GetProperty("Y"), c1)),
					Expression.MemberBind(typeof(LineSegment).GetProperty("End"),
						Expression.Bind(typeof(Point).GetProperty("X"), c2),
						Expression.Bind(typeof(Point).GetProperty("Y"), c2))
					)
				);

			// new with list initializer
			var polylineCtor = typeof(Polyline).GetConstructor(new Type[0]);
			var listAdd = typeof(List<Point>).GetMethod("Add", new[] { typeof(Point) });
			SimpleCheck(
				Make.Expression(() =>
					new Polyline
					{
						Vertices = {
							new Point{ X = 1, Y = 1 },
							new Point{ X = 2, Y = 2 },
						}
					}).Body,
				Expression.MemberInit(
					Expression.New(polylineCtor),
					Expression.ListBind(typeof(Polyline).GetProperty("Vertices"),
						Expression.ElementInit(listAdd,
							Expression.MemberInit(
								Expression.New(pointCtor0),
								Expression.Bind(typeof(Point).GetProperty("X"), c1),
								Expression.Bind(typeof(Point).GetProperty("Y"), c1))),
						Expression.ElementInit(listAdd,
							Expression.MemberInit(
								Expression.New(pointCtor0),
								Expression.Bind(typeof(Point).GetProperty("X"), c2),
								Expression.Bind(typeof(Point).GetProperty("Y"), c2)))
					))
				);
		}

		static void Call()
		{
			var mathAbs = typeof(Math).GetMethod("Abs", new[] { typeof(int) });

			SimpleCheck(
				Make.Expression(() => Math.Abs(1)).Body,
				Expression.Call(mathAbs, Expression.Constant(1))
				);

			new ExpressionToMultiLineTest().Invoke();
		}

		void Invoke()
		{
			var absInfo = this.GetType().GetField("abs", BindingFlags.NonPublic | BindingFlags.Instance);

			SimpleCheck(
				Make.Expression(() => abs(1)).Body,
				Expression.Invoke(
					Expression.MakeMemberAccess(
						Expression.Constant(this), absInfo),
					Expression.Constant(1))
				);
		}

		Func<int, int> abs = Math.Abs;

		#endregion
	}
}
