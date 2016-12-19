using System;

struct DefiniteAssignment
{
    // DateTimeOffset には中身があるはずなのに…
    DateTimeOffset _x;

    public DefiniteAssignment(int n) { } // PCL ではエラーにならない
}
