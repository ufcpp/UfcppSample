using System;

namespace ConsoleApplication1
{
  /// <summary>
  /// 相空間（位置＋運動量）上のベクトル。
  /// </summary>
  /// <remarks>
  /// 曲面上の運動のシミュレーション用なので、2次元。
  /// 4次のルンゲクッタ法を使った数値計算関数付き。
  /// </remarks>
  public class PhaseVector
  {
    public double q1;
    public double q2;
    public double p1;
    public double p2;

    public PhaseVector(double q1, double q2, double p1, double p2)
    {
      this.q1 = q1;
      this.q2 = q2;
      this.p1 = p1;
      this.p2 = p2;
    }

    public static PhaseVector operator +(PhaseVector x, PhaseVector y)
    {
      return new PhaseVector(
               x.q1 + y.q1, x.q2 + y.q2,
               x.p1 + y.p1, x.p2 + y.p2);
    }

    public static PhaseVector operator /(PhaseVector x, double a)
    {
      return new PhaseVector(
               x.q1 / a, x.q2 / a,
               x.p1 / a, x.p2 / a);
    }

    public static PhaseVector operator *(double a, PhaseVector x)
    {
      return new PhaseVector(
               a * x.q1, a * x.q2,
               a * x.p1, a * x.p2);
    }

    public delegate void Callback(double t, PhaseVector x);
    public delegate PhaseVector PhaseFunc(PhaseVector x);

    /// <summary>
    /// 微分方程式
    /// (d/dt)q = f(q)
    /// の解を数値計算で求める。
    /// </summary>
    /// <param name="t0">時刻の初期値</param>
    /// <param name="t1">時刻の最終値</param>
    /// <param name="dt">時刻の刻み幅</param>
    /// <param name="display_interval">結果出力の間隔</param>
    /// <param name="initial">q の初期値</param>
    /// <param name="f">f</param>
    /// <param name="cb">結果出力用のコールバック関数</param>
    /// <remarks>
    /// 4次のルンゲクッタ法で計算。
    /// </remarks>
    public static void Simulate(
      double t0, double t1, double dt, int display_interval,
      PhaseVector initial,
      PhaseFunc f, Callback cb)
    {
      PhaseVector q = initial;

      int n = 1;
      for (double t = t0; t < t1; t += dt, ++n)
      {
        PhaseVector k1 = dt * f(q);
        PhaseVector k2 = dt * f(q + k1 / 2);
        PhaseVector k3 = dt * f(q + k2 / 2);
        PhaseVector k4 = dt * f(q + k3);
        q = q + (k1 + 2 * (k2 + k3) + k4) / 6;

        if (n > display_interval)
        {
          cb(t, q);
          n = 1;
        }
      }
    }
  }

  /// <summary>
  /// サンプルプログラム。
  /// 球面上の運動をシミュレーション。
  /// </summary>
  class Program
  {
    static void Main(string[] args)
    {
      const double dt = 0.01;
      const double t_end = 10;
      const int DISPLAY_INTERVAL = 5;

      const double q1 = 0;
      const double q2 = Math.PI / 2;
      const double p1 = 0.1;
      const double p2 = 0;

      PhaseVector.Simulate(
        0, t_end, dt, DISPLAY_INTERVAL,
        new PhaseVector(q1, q2, p1, p2),
        F, Show
        );
    }

    static PhaseVector F(PhaseVector q)
    {
      const double M = 0.1;
      const double G = 10;

      double s2 = Math.Sin(q.q2);
      double c2 = Math.Cos(q.q2);

      return new PhaseVector(
        q.p1 / (M * s2),
        q.p2 / M,
        0,
        (q.p1 * q.p1 * c2)
          / (M * s2 * s2 * s2)
         - M * G * s2
       );
    }

    static void Show(double t, PhaseVector q)
    {
      double y = Math.Sin(q.q2);
      double x = Math.Cos(q.q1) * y;
      y = Math.Sin(q.q1) * y;
      double z = -Math.Cos(q.q2);

      Console.Write("{0},{1},{2},{3}\n",
        t, x, y, z);
    }
  }
}
