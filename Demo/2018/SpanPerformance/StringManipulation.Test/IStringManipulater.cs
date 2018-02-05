namespace StringManipulation
{
    /// <summary>
    /// テスト用。
    /// <see cref="Classic.StringExtensions"/>
    /// <see cref="Unsafe.StringExtensions"/>
    /// <see cref="SafeStackalloc.StringExtensions"/>
    /// の静的メソッドを多態化するために使う。
    /// </summary>
    interface IStringManipulater
    {
        string SnakeToCamel(string s);
        string CamelToSnake(string s);
    }

    namespace Classic
    {
        struct Manipulater : IStringManipulater
        {
            public string CamelToSnake(string s) => s.CamelToSnake();
            public string SnakeToCamel(string s) => s.SnakeToCamel();
        }
    }

    namespace Unsafe
    {
        struct Manipulater : IStringManipulater
        {
            public string CamelToSnake(string s) => s.CamelToSnake();
            public string SnakeToCamel(string s) => s.SnakeToCamel();
        }
    }

    namespace SafeStackalloc
    {
        struct Manipulater : IStringManipulater
        {
            public string CamelToSnake(string s) => s.CamelToSnake();
            public string SnakeToCamel(string s) => s.SnakeToCamel();
        }
    }
}
