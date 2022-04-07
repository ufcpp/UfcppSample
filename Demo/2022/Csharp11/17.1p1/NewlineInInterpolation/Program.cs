var a = 1;
var b = 2;
var s = $"a: {
    a // ここの改行、前は入れれなかった。特に理由はないので修正。
    }, b: {
    b
    }";

Console.WriteLine(s);
