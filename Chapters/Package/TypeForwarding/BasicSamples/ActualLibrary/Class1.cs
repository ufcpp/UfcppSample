public class Class1
{
    public string Name => GetType().Assembly.GetName().Name + " / " + nameof(Class1);
}
