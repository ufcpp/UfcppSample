using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ContextFreeTasks
{
    internal static class UnsafeHelper
    {
        // ここの GetType なくしたいけども…
        private static bool IsTaskAwaiter<TAwaiter>(TAwaiter awaiter) => awaiter is TaskAwaiter || IsTaskAwaiterT(awaiter.GetType());

        private static bool IsTaskAwaiterT(Type t)
        {
            if (!t.GetTypeInfo().IsGenericType) return false;
            return t.GetGenericTypeDefinition() == typeof(TaskAwaiter<>);
        }

        /// <summary>
        /// TaskAwaiter の中身が Task 1個だけという前提で unsafe に中の Task を取り出す
        /// </summary>
        /// <typeparam name="TAwaiter"></typeparam>
        /// <param name="awaiter"></param>
        /// <returns></returns>
        public static Task ExtractTask<TAwaiter>(TAwaiter awaiter)
        {
            if (IsTaskAwaiter(awaiter))
            {
                unsafe
                {
                    // 構造体のメンバーを、構造体介さず、リフレクションも介さず、Unsafe/ポインターを使って直接参照。
                    var p = Unsafe.AsPointer(ref awaiter);
                    var t = Unsafe.AsRef<Task>(p);
                    return t;
                }
            }
            else return null;
        }
    }
}
