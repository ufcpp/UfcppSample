using MSXML2;
using System;

namespace NativeInterop
{
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
