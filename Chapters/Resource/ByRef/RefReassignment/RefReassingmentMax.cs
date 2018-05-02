namespace ByRef.RefReassignment.RefReassingmentMax
{
    using System;

    class Program
    {
        static ref int RefMaxOld(int[] array)
        {
            if (array.Length == 0) throw new InvalidOperationException();

            // これまでこんな感じでインデックスで持って、
            var maxIndex = 0;

            for (int i = 1; i < array.Length; i++)
            {
                // 毎度毎度、配列のインデックス アクセスするようなコードを書くことがたまに。
                if (array[maxIndex] < array[i])
                {
                    array[maxIndex] = array[i];
                    maxIndex = i;
                }
            }

            return ref array[maxIndex];
        }

        static ref int RefMax(int[] array)
        {
            if (array.Length == 0) throw new InvalidOperationException();

            // それを、こんな風に参照ローカル変数に変えて、
            ref var max = ref array[0];

            for (int i = 1; i < array.Length; i++)
            {
                // ref 再代入で済ませるように。
                ref var x = ref array[i];
                if (max < x) max = ref x;
            }

            return ref max;
        }
    }
}
