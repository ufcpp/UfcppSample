using System;

namespace NativeInterop
{
    /// <summary>
    /// <see cref="Type.GetTypeFromProgID(string)"/> と dynamic を使えば、COM を遅延バインディングで使える。
    /// やっていることは <see cref="ComImportSample"/> と同じ。
    /// </summary>
    class ComLateBindingSample
    {
        public static void Main()
        {
            var t = Type.GetTypeFromProgID("MSXML2.DOMDocument");
            dynamic doc = Activator.CreateInstance(t);

            if (doc.load("Sample.xml"))
            {
                var s = doc.documentElement;

                foreach (var item in s.getElementsByTagName("Item"))
                {
                    var name = item.getAttribute("Name");
                    var value = item.getAttribute("Value");

                    Console.WriteLine($"{name} = {value}");
                }
            }
        }
    }
}
