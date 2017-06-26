using Lib;

namespace LibDependentOnA
{
    public static class Class1Factory
    {
        public static Class1 Create(int id) => new Class1 { Id = id };
    }
}
