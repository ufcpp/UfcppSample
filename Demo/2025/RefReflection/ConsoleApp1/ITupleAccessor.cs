namespace ConsoleApp1;

public interface ITupleAccessor
{
    int Length { get; }
    TypedRef this[int index] { get; }
}
