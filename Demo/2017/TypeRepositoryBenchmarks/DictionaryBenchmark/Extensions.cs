using System.Collections.Generic;
static class Extensions
{
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> p, out TKey key, out TValue value)
    {
        key = p.Key;
        value = p.Value;
    }
}
