#define DYNAMIC

using System;
using System.Reflection;

#region type Shape interface { func Area() double; func Perimeter() double; }

/// <summary>
/// Shape。
/// C# 用の interface。
/// </summary>
interface IShape
{
    /// <summary>
    /// 面積を求める。
    /// </summary>
    /// <returns>面積。</returns>
    double Area();

    /// <summary>
    /// 周長を求める。
    /// </summary>
    /// <returns>周長。</returns>
    double Perimeter();
}

/// <summary>
/// 動的ディスパッチ用のアダプター。
/// 仮想メソッドテーブル ＋ インスタンスを抱えてる感じ。
/// </summary>
/// <typeparam name="T">IShape に Duck Typing したい型。</typeparam>
class ShapeDispatcher<T> : IShape
{
    public T Instance;
    public Func<T, double> AreaImpl;
    public Func<T, double> PerimeterImpl;

    #region C# の仕様とつなぎこむためのの部分

    public double Area()
    {
        return AreaImpl(Instance);
    }

    public double Perimeter()
    {
        return PerimeterImpl(Instance);
    }

    #endregion
}

static partial class Methods
{
#if DYNAMIC
    static readonly Type methods = typeof(Methods);

    /// <summary>
    /// T 型を IShape に Duck Typing するためのキャスト関数。
    /// リフレクションを使った動的生成版。
    /// </summary>
    /// <remarks>
    /// 毎度リフレクションしてると重たいけど、キャッシュ機構も組み込めばそれなりにパフォーマンス出ると思う。
    /// </remarks>
    /// <typeparam name="T">IShape にキャストしたい型。</typeparam>
    /// <param name="x">そのインスタンス。</param>
    /// <returns>IShape 化したもの。</returns>
    public static IShape AsShape<T>(this T x)
    {
        var t = typeof(T);
        Type TypeOfArea = typeof(Func<T, double>);
        Type TypeOfPerimeter = typeof(Func<T, double>);

        var areaInfo = methods.GetMethod("Area", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { t }, null);
        var area = Delegate.CreateDelegate(TypeOfArea, areaInfo);

        var perimeterInfo = methods.GetMethod("Perimeter", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { t }, null);
        var perimeter = Delegate.CreateDelegate(TypeOfPerimeter, perimeterInfo);

        return new ShapeDispatcher<T>
        {
            Instance = x,
            AreaImpl = (Func<T, double>)area,
            PerimeterImpl = (Func<T, double>)perimeter,
        };
    }
#endif
}

#endregion
#region type Rectangle struct { var Width double; var Height double; }
// func (x Rectangle) Area () double { return x.Width * x.Height; }
// func (x Rectangle) Perimeter () double { return 2 * (x.Width + x.Height); }

/// <summary>
/// 長方形。
/// </summary>
/// <remarks>
/// クラス/構造体本体にはフィールドしか定義しない。
/// 当然、IShape も継承しない。
/// </remarks>
struct Rectangle
{
    public double Width;
    public double Height;
}

/// <summary>
/// メソッドは全部、拡張メソッド（static メソッド）として定義する。
/// </summary>
static partial class Methods
{
    /// <summary>
    /// Rectangle 向けの Area 実装。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>面積。</returns>
    public static double Area(this Rectangle x)
    {
        return x.Width * x.Height;
    }

    /// <summary>
    /// Rectangle 向けの Perimeter 実装。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>周長。</returns>
    public static double Perimeter(this Rectangle x)
    {
        return 2 * (x.Width + x.Height);
    }

#if !DYNAMIC
    /// <summary>
    /// Rectangle 型を IShape に Duck Typing するためのキャスト関数。
    /// リフレクション使わない（コンパイル時解決が可能）ならこうやる。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>IShape 化したもの。</returns>
    public static IShape AsShape(this Rectangle x)
    {
        return new ShapeDispatcher<Rectangle>
        {
            Instance = x,
            AreaImpl = Area,
            PerimeterImpl = Perimeter,
        };
    }
#endif
}

#endregion
#region type Circle struct { var Radius double; }
// func (x Circle) Area () double { return PI * x.Radius * x.Radius; }
// func (x Circle) Perimeter () double { return 2 * Math.PI * x.Radius); }

/// <summary>
/// 円。
/// </summary>
/// <remarks>
/// Rectangle 同様、フィールドしか定義しない。
/// </remarks>
struct Circle
{
    public double Radius;
}

static partial class Methods
{
    /// <summary>
    /// Circle 向けの Area 実装。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>面積。</returns>
    public static double Area(this Circle x)
    {
        return Math.PI * x.Radius * x.Radius;
    }

    /// <summary>
    /// Circle 向けの Perimeter 実装。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>周長。</returns>
    public static double Perimeter(this Circle x)
    {
        return 2 * Math.PI * x.Radius;
    }

#if !DYNAMIC
    /// <summary>
    /// Circle 型を IShape に Duck Typing するためのキャスト関数。
    /// Shape 同様、コンパイル時解決用。
    /// </summary>
    /// <param name="x">インスタンス。</param>
    /// <returns>IShape 化したもの。</returns>
    public static IShape AsShape(this Circle x)
    {
        return new ShapeDispatcher<Circle>
        {
            Instance = x,
            AreaImpl = Area,
            PerimeterImpl = Perimeter,
        };
    }
#endif
}

#endregion

/// <summary>
/// 利用例。
/// </summary>
class DuckTypingInterface
{
    /// <summary>
    /// 上で書いたコードの利用例。
    /// </summary>
    public static void Test()
    {
        var rect = new Rectangle { Width = 2, Height = 3 };
        var circle = new Circle { Radius = 1.41421356 };

        Console.WriteLine("長方形");
        Output(rect.AsShape());
        Console.WriteLine("円");
        Output(circle.AsShape());
    }

    /// <summary>
    /// よくある interface のサンプルコード。
    /// 図形の面積と周長を Console 出力。
    /// </summary>
    /// <param name="s"></param>
    static void Output(IShape s)
    {
        Console.WriteLine("面積: {0}", s.Area());
        Console.WriteLine("周長: {0}", s.Perimeter());
    }
}
