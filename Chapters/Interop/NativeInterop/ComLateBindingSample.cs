using System;

namespace NativeInterop
{
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
