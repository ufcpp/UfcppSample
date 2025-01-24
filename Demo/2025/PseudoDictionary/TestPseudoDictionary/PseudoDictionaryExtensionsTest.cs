using PseudoDictionary;
using System.Runtime.InteropServices;

namespace TestPseudoDictionary;

using static Common;

public class PseudoDictionaryExtensionsTest
{
    [Fact]
    public void EquivalentToDictionary()
    {
        var list = new List<(string key, int value)>();
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
        var list = new List<(string key, int value)>();
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
        var list = new List<X>();
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
        var list = new List<X>();
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
