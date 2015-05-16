namespace Boxing.ToString
{
    using System;

    class Program
    {
        static void Main()
        {
            ObjectWriteLine(5);
            IntWriteLine(5);
        }

        static void ObjectWriteLine(object x)
        {
            // object.ToString が呼ばれる
            // 値型に対してはボックス化が必要
            Console.WriteLine(x.ToString());
        }

        static void IntWriteLine(int x)
        {
            // こういう場合は、int.ToString が直接呼ばれる
            // virtual メソッドだからといって、必ず virtual に呼ばれるわけじゃない
            // コンパイルの時点で型が確定してるなら、非 virtual にメソッドを呼ぶ
            Console.WriteLine(x.ToString());
        }
    }
}
