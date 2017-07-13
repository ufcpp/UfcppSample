using MSXML2;
using System;

namespace NativeInterop
{
    /// <summary>
    /// COM interop の例として、MSXML2 を参照してみる。
    /// </summary>
    class ComImportSample
    {
        public static void Main()
        {
            var doc = new DOMDocument60();

            if (doc.load("Sample.xml"))
            {
                var s = doc.documentElement;

                foreach (IXMLDOMElement item in s.getElementsByTagName("Item"))
                {
                    var name = item.getAttribute("Name");
                    var value = item.getAttribute("Value");

                    Console.WriteLine($"{name} = {value}");
                }
            }
        }
    }
}
