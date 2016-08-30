using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionTree
{
	class CustomUnaryPlus
	{
		public static CustomUnaryPlus operator +(CustomUnaryPlus x)
		{
			return new CustomUnaryPlus();
		}
	}

	class Point
	{
		public int X { set; get; }
		public int Y { set; get; }
		public Point() : this(0, 0) { }
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	class LineSegment
	{
		public Point Start { set; get; }
		public Point End { set; get; }
	}

	class Polyline
	{
		public List<Point> Vertices { set; get; }

		public Polyline() { this.Vertices = new List<Point>(); }
	}

	struct Int
	{
		int inner;

		public Int(int n) { this.inner = n; }

		/// <summary>
		/// ベキを求める。
		/// </summary>
		static public Int operator^ (Int a, Int b)
		{
			var pow = 1;
			for (int b_ = b.inner; b_ > 0; --b_)
				pow *= a.inner;
			
			return new Int(pow);
		}
	}
}
