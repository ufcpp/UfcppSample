using System;
using System.Collections.Generic;
using System.Text;

namespace BitOperations
{
    /// <summary>
    /// Type Class 的な手段でいろんな型の n bit 目読み書き・シフト操作をするためのインターフェイス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface SBitOperator<T>
    {
        int Size { get; }
        bool GetBit(ref T x, int index);
        void SetBit(ref T x, int index, bool value);
        T RightShift(T x);
    }
}
