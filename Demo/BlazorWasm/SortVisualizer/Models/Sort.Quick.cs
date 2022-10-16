namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> QuickSort(int[] a) => QuickSort(a, 0, a.Length - 1);

    static IEnumerable<Operation> QuickSort(int[] a, int first, int last)
    {
        if (first == last) yield break;

        // 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）
        var pivotI = Median(a, first, (first + last) / 2, last);
        var pivot = a[pivotI];

        // 左右分割
        int l = first;
        int r = last;

        while (l <= r)
        {
            while (l < last && a[l] < pivot)
            {
                yield return new(Kind.Compare, l, pivotI);
                l++;
            }
            while (r > first && a[r] >= pivot)
            {
                yield return new(Kind.Compare, r, pivotI);
                r--;
            }
            if (l > r) break;
            Swap(a, l, r);
            yield return new(Kind.Swap, l, r);
            l++; r--;
        }

        // 再帰呼び出し
        foreach (var op in QuickSort(a, first, l - 1)) yield return op;
        foreach (var op in QuickSort(a, l, last)) yield return op;


        static int Median(int[] a, int i, int j, int k)
        {
            if (a[i] > a[j])
            {
                if (a[i] > a[k]) return i;
                else return k;
            }
            else
            {
                if (a[j] > a[k]) return j;
                else return k;
            }
        }
    }
}
