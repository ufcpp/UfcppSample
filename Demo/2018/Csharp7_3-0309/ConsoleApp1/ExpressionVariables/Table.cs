namespace ConsoleApp1.ExpressionVariables
{
    class Table
    {
        // out var のサンプルを書くのに TryGetValue が一番それっぽくなるので。
        // 内容は適当。
        public static bool TryGetValue(string key, out int value)
        {
            switch (key)
            {
                case "one":
                    value = 1;
                    break;
                case "two":
                    value = 2;
                    break;
                case "three":
                    value = 3;
                    break;
                case "four":
                    value = 4;
                    break;
                case "five":
                    value = 5;
                    break;
                default:
                    value = -1;
                    return false;
            }
            return true;
        }
    }
}
