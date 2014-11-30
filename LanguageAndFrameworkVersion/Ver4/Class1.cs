namespace VersionSample
{
    class Class1
    {
        public void X()
        {
            Csharp6.SyntaxSugarSample.X();
            Csharp6.SyntaxSugarSample.SameAsX();
            Csharp6.SyntaxSugarSample.Y();
            //Csharp6.AsyncSample.XAsync().Wait();
            //Csharp6.AsyncSample.AproxSameXAsync().Wait();
            Csharp6.ExtensionListInitializerSample.X();
            Csharp6.ExtensionListInitializerSample.SameAsX();
            var entry = new Csharp6.Entry(1, "one");
            Csharp6.NullConditionalSample.X(entry);
            Csharp6.NullConditionalSample.SameAsX(entry);
            Csharp6.NullConditionalSample.Y(entry);
            Csharp6.NullConditionalSample.SameAsY(entry);
            Csharp6.StringInterpolationSample.X();
            Csharp6.StringInterpolationSample.SameAsX();

            Csharp5.ForeachBreakingChangeSample.X();
            //Csharp5.CallerInfoSample.X();
            //Csharp5.CallerInfoSample.ApproxSameAsX();
            //Csharp5.AsyncSample.XAsync().Wait();
            //Csharp5.AsyncTaskSample.XAsync().Wait();

            Csharp4.VarianceSample.X();
            Csharp4.VarianceBclSample.X();
            Csharp4.DynamicSample.X();
            Csharp4.DefaultParameterSample.X();
            Csharp4.DefaultParameterSample.SameAsX();

            Csharp3.SyntaxSugarSample.X();
            Csharp3.SyntaxSugarSample.SameAsX();
            var snake_case_string = "snake_case_string";
            Csharp3.ExtensionMethodSample.SnakeToPascal(snake_case_string);
            Csharp3.ExtensionMethodSample.SameAsSnakeToPascal(snake_case_string);
            Csharp3.LinqToObjectSample.X();
            Csharp3.PartialMethodSample.X();
        }
    }
}
