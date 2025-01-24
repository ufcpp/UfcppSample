using PseudoDictionary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ValueList.Collections;

namespace TestPseudoDictionary;

using static Common;

public class PseudoDictionaryExtensionsValueListTest
{
    [InlineArray(32)]
    private struct Buffer<T>
    {
        private T _value;
    }

    [Fact]
    public void EquivalentToDictionary()
    {
        var buffer = new Buffer<(string key, int value)>();
        var list = new ValueListBuilder<(string key, int value)>(buffer);
        var dic = new Dictionary<string, int>();

        foreach (var word in EnumerateWords())
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dic, word, out _)++;
            list.GetValueRefOrAddDefault(word)++;
        }

        foreach (var (key, value) in dic)
        {
            Assert.Equal(value, list.GetValueRefOrAddDefault(key));
        }
    }

    [Fact]
    public void KeyComparer()
    {
        var buffer = new Buffer<(string key, int value)>();
        var list = new ValueListBuilder<(string key, int value)>(buffer);
        var dic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var word in EnumerateWords())
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dic, word, out _)++;
            list.GetValueRefOrAddDefault(word, EqualsIgnoreCase)++;
        }

        foreach (var (key, value) in dic)
        {
            Assert.Equal(value, list.GetValueRefOrAddDefault(key, EqualsIgnoreCase));
        }
    }

    [Fact]
    public void KeySeletor()
    {
        var buffer = new Buffer<X>();
        var list = new ValueListBuilder<X>(buffer);
        var dic = new Dictionary<string, int>();

        foreach (var word in EnumerateWords())
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dic, word, out _)++;
            list.GetValueRefOrAddDefault(word, GetKey, NewX).Value++;
        }

        foreach (var (key, value) in dic)
        {
            Assert.Equal(value, list.GetValueRefOrAddDefault(key, GetKey, NewX).Value);
        }
    }

    [Fact]
    public void KeySeletorComparer()
    {
        var buffer = new Buffer<X>();
        var list = new ValueListBuilder<X>(buffer);
        var dic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var word in EnumerateWords())
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dic, word, out _)++;
            list.GetValueRefOrAddDefault(word, GetKey, NewX, EqualsIgnoreCase).Value++;
        }

        foreach (var (key, value) in dic)
        {
            Assert.Equal(value, list.GetValueRefOrAddDefault(key, GetKey, NewX, EqualsIgnoreCase).Value);
        }
    }
}
