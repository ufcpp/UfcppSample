using System;

namespace Csharp6.Csharp5.NameOf
{
    class MyClass
    {
        public int MyProperty => myField;
        private int myField = 10;

        public void MyMethod()
        {
            var myLocal = 10;
            Console.WriteLine("MyClass");
            Console.WriteLine("MyProperty = " + MyProperty);
            Console.WriteLine("myField = " + myField);
            Console.WriteLine("MyMethod");
            Console.WriteLine("myLocal = " + myLocal);
        }
    }
}
