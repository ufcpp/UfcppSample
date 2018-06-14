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
            buffer.A[0] = 1; // 元々 OK

            _buffer.A[0] = 2; // C# 7.3 から OK

            RefFixedBuffer(ref buffer);

            System.Console.WriteLine(buffer.A[0]);  // 元々 OK
            System.Console.WriteLine(_buffer.A[0]); // C# 7.3 から OK
        }

        unsafe static void RefFixedBuffer(ref Buffer buffer)
        {
            buffer.A[1] = 3; // C# 7.3 から OK
        }
    }
}
