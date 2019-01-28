namespace BoolOtherThan01
{
    using System;

    class TypeSwitch
    {
        static void Main()
        {
            Branch(0);
            Branch(1);
            Branch(2);
        }

        static unsafe void Branch(byte x)
        {
            var b = *((bool*)&x);

            Console.WriteLine($"value = {x}");
            Console.Write("    traditional switch: ");
            switch (b)
            {
                case false:
                    Console.WriteLine("false");
                    break;
                case true:
                    Console.WriteLine("true");
                    break;
                default:
                    // 0でも1でもないbool値の時にここに来る
                    Console.WriteLine("other");
                    break;
            }

            Console.Write("    type switch: ");
            switch (b)
            {
                case false when true:
                    Console.WriteLine("false");
                    break;
                case true:
                    Console.WriteLine("true");
                    break;
                default:
                    // 絶対ここは通らない
                    Console.WriteLine("other");
                    break;
            }
        }
    }
}
