using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Reflection.BindingFlags;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine(@"Invokes a static method in the netstandard.

Usage: xstatic [type name] [static method name] [args]...");
            return;
        }

        var typeName = args[0];
        var methodName = args[1];
        var methodArgs = args.Skip(2).ToArray();

        var t = FindType(typeName);
        var method = FindMethod(t, methodName, methodArgs);

        var result = Invoke(method, methodArgs);

        Console.WriteLine(result);
    }

    private static object Invoke(MethodInfo method, string[] methodArgs)
    {
        var convertedArgs = new object[methodArgs.Length];
        var parameters = method.GetParameters();

        for (int i = 0; i < methodArgs.Length; i++)
        {
            var a = methodArgs[i];
            var p = parameters[i];
            convertedArgs[i] = Convert(a, p.ParameterType);
        }

        var result = method.Invoke(null, convertedArgs);
        return result;
    }

    private static object Convert(string arg, Type parameterType)
    {
        if (parameterType == typeof(string)) return arg;
        if (parameterType == typeof(int)) return int.Parse(arg);
        if (parameterType == typeof(uint)) return uint.Parse(arg);
        if (parameterType == typeof(double)) return double.Parse(arg);
        if (parameterType == typeof(float)) return float.Parse(arg);
        if (parameterType == typeof(long)) return long.Parse(arg);
        if (parameterType == typeof(ulong)) return ulong.Parse(arg);
        if (parameterType == typeof(short)) return short.Parse(arg);
        if (parameterType == typeof(ushort)) return ushort.Parse(arg);
        if (parameterType == typeof(byte)) return byte.Parse(arg);
        if (parameterType == typeof(sbyte)) return sbyte.Parse(arg);
        if (parameterType.IsEnum) return Enum.Parse(parameterType, arg);
        return arg;
    }

    private static MethodInfo FindMethod(Type t, string methodName, string[] methodArgs)
    {
        foreach (var m in t.GetMethods(Static | Public))
        {
            if (m.Name != methodName) continue;

            var parameters = m.GetParameters();

            if (parameters.Length != methodArgs.Length) continue;

            if (!parameters.All(p => IsConvertibleFromString(p.ParameterType))) continue;

            return m;
        }

        return null;
    }

    private static bool IsConvertibleFromString(Type t)
    {
        if (t.IsPrimitive) return true;
        if (t.IsEnum) return true;
        if (t == typeof(string)) return true;
        return false;
    }

    private static Type FindType(string typeName)
    {
        var sharedPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
        var netstdPath = Path.Combine(sharedPath, "netstandard.dll");
        var asm = Assembly.LoadFile(netstdPath);
        var t = asm.GetType(typeName);
        return t;
    }
}
