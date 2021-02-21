#nullable enable

using System;

// この2つに関しては default == null なので変なことにはならない。
Console.WriteLine(M<string?>()); // null
Console.WriteLine(M<string>()); // null
Console.WriteLine(M<int?>()); // null

// 問題が非 null 値型で、この場合 default != null なのでちょっと変。
Console.WriteLine(M<int>()); // 0

// ジェネリックな T? は nullable じゃなくて defaultable。
// default を渡しても警告にならない。 
static T? M<T>() => default;
