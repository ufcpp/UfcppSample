using System.Text.Utf8;

namespace SampleApp.Lib
{
    interface IMemberAccessor
    {
        TypedPointer GetPointer(int index);
    }

    interface INamedMemberAccessor
    {
        int GetIndex(Utf8String name);
    }
}
