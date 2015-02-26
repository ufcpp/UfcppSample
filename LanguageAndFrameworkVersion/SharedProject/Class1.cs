namespace VersionSample
{
    class Class1
    {
        public void X()
        {
            // C# コンパイラーのレベルで実現している機能(.NET のバージョンは問わない)
            Csharp6.SyntaxSugarSample.X();
            Csharp6.SyntaxSugarSample.SameAsX();
            var entry = new Csharp6.Entry(1, "one");
            Csharp6.NullConditionalSample.X(entry);
            Csharp6.NullConditionalSample.SameAsX(entry);
            Csharp6.NullConditionalSample.Y(entry);
            Csharp6.NullConditionalSample.SameAsY(entry);
            Csharp6.StringInterpolationSample.X();
            Csharp6.StringInterpolationSample.SameAsX();
            Csharp3.SyntaxSugarSample.X();
            Csharp3.SyntaxSugarSample.SameAsX();
            Csharp3.PartialMethodSample.X();

            // .NET のレベルでは 1.0, 2.0 時代から対応していたけども、C# 的には後から対応したもの
            Csharp6.SyntaxSugarSample.Y();
            Csharp4.VarianceSample.X();
            Csharp4.DefaultParameterSample.X();
            Csharp4.DefaultParameterSample.SameAsX();

            // どのバージョンでもコンパイルは通るけども、挙動がちょっと違うもの
            Csharp5.ForeachBreakingChangeSample.X();

#if Ver3 || (Ver2 && Plus)
            // .NET 3.5 以降のみ。ただし、.NET 2.0 でも簡単なクラスの自作で対応可能。

            Csharp6.ExtensionListInitializerSample.X();
            Csharp6.ExtensionListInitializerSample.SameAsX();

            var snake_case_string = "snake_case_string";
            Csharp3.ExtensionMethodSample.SnakeToPascal(snake_case_string);
            Csharp3.ExtensionMethodSample.SameAsSnakeToPascal(snake_case_string);
            Csharp3.LinqToObjectSample.X();
#endif

#if Ver4_5 || (Ver2 && Plus)
            // .NET 4.5 以降のみ。ただし、.NET 2.0 でも簡単なクラスの自作で対応可能。
            // .NET 4 以降であれば NuGet パッケージの参照で対応可能。

            Csharp5.CallerInfoSample.X();
            Csharp5.CallerInfoSample.ApproxSameAsX();
#endif

#if Ver4_5 || (Ver4 && Plus)
            // .NET 4.5 以降のみ。ただし、.NET 4 以降であれば NuGet パッケージの参照で対応可能。

            Csharp6.AsyncSample.XAsync().Wait();
            Csharp6.AsyncSample.AproxSameXAsync().Wait();
            Csharp5.AsyncSample.XAsync().Wait();
            Csharp5.AsyncTaskSample.XAsync().Wait();
#endif

#if Ver4_6 || (Ver2 && Plus)
            // .NET 4.6 以降のみ。ただし、.NET 2.0 でも簡単なクラスの自作で対応可能。

            Csharp6.StringInterpolationSample.Y();
            Csharp6.StringInterpolationSample.SameAsY();
#endif

#if Ver4
            // .NET 4 以降のみ。

            Csharp4.VarianceBclSample.X();
            Csharp4.DynamicSample.X();
#endif
        }
    }
}
