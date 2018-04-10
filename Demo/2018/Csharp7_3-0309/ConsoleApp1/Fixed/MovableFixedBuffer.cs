namespace ConsoleApp1.Fixed.MovableFixedBuffer
{
    unsafe struct Buffer
    {
        public fixed byte A[8];

#if Uncompilable
        // これは C# 7.3 でもダメ
        public byte First => A[0];
#endif
    }

    class Program
    {
        static Buffer _buffer;

        unsafe static void Main()
        {
            var buffer = new Buffer();
            buffer.A[0] = 1; // これは元々 OK

            _buffer.A[0] = 2; // これは C# 7.3 から OK

            RefFixedBuffer(ref buffer);

            System.Console.WriteLine(buffer.A[0]);
            System.Console.WriteLine(buffer.A[1]);
            System.Console.WriteLine(_buffer.A[0]); // これも C# 7.3 から OK
        }

        // unsafe は C# 7.3 でも必須
        unsafe static void RefFixedBuffer(ref Buffer buffer)
        {
            buffer.A[1] = 3; // これも C# 7.3 から OK
        }
    }
}
