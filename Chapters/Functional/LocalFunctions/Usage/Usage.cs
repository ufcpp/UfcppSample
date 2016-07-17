namespace LocalFunctions.Usage
{
    class Usage1
    {
        static void M()
        {
            // 何らかの前準備とか
            MInternal();
        }

        static void MInternal()
        {
            // 実際の処理はこちらで
        }
    }

    class Usage2
    {
        static void M()
        {
            // 何らかの前準備とか

            void m()
            {
                // 実際の処理はこちらで
            }

            m();
        }
    }
}
