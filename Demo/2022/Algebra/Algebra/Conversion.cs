namespace Algebra;

public interface IImplicitConversion<TSelf, TBase> where TSelf : IImplicitConversion<TSelf, TBase>
{
    static abstract implicit operator TSelf(TBase x);
}

public interface IExplicitConversion<TSelf, TBase> where TSelf : IExplicitConversion<TSelf, TBase>
{
    static abstract explicit operator TSelf(TBase x);
}
