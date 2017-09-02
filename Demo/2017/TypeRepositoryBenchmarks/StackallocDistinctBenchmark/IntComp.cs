using System.Collections.Generic;
struct IntComp : IEqualityComparer<int>
{
    public bool Equals(int x, int y) => x == y;
    public int GetHashCode(int obj) => obj.GetHashCode();
}
