using System;
using System.Collections.Generic;

/// <summary>
/// Changed in VS 2012
///
/// In https://msdn.microsoft.com/en-us/library/hh678682(v=vs.110).aspx
/// You can use the iteration variable of a foreach statement in a lambda expression that’s contained in the body of the loop.
/// </summary>
class IterationVariableOfForeach
{
    static void Main()
    {
        var methods = new List<Action>();
        foreach (var word in new string[] { "hello", "world" })
        {
            methods.Add(() => Console.Write(word + " "));
        }

        methods[0]();
        methods[1]();
    }

    // Output in Visual Studio 2012: 
    // hello world

    // Output in Visual Studio 2010: 
    // world world
}
