using System.Threading;
using System.Threading.Tasks;

namespace SystemAsync
{
    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task AsyncAction(CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="x1"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<T1>(T1 x1, CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<T1, T2>(T1 x1, T2 x2, CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="x3"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<T1, T2, T3>(T1 x1, T2 x2, T3 x3, CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task<TResult> AsyncFunc<TResult>(CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="x1"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task<TResult> AsyncFunc<T1, TResult>(T1 x1, CancellationToken ct);

    /// <summary>
    /// 非同期メソッド用共通デリゲート。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public delegate Task<TResult> AsyncFunc<T1, T2, TResult>(T1 x1, T2 x2, CancellationToken ct);
}
