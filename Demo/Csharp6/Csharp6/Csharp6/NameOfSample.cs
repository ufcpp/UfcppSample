using System;

namespace Csharp6.Csharp6.NameOf
{
    class MyClass
    {
        public int MyProperty => myField;
        private int myField = 10;

        public void MyMethod()
        {
            var myLocal = 10;
            Console.WriteLine(nameof(MyClass));
            Console.WriteLine(nameof(MyProperty) + " = " + MyProperty);
            Console.WriteLine(nameof(myField) + " = " + myField);
            Console.WriteLine(nameof(MyMethod));
            Console.WriteLine(nameof(myLocal) + " = " + myLocal);
        }
    }
}
