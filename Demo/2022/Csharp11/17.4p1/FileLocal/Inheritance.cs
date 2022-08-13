namespace FileLocal;

// file-local なクラスで public なクラスを継承。
file class FileDerived : Base
{
}

public class Base
{
    // これはダメ。
    // file-local な型を public な引数・戻り値には書けない。
    //public static FileDerived Create() => new X();

    // これは書けるので、大体これでやる。
    public static Base Create() => new FileDerived();
}

#if ERROR
// 逆はダメ。
file class FileBase { }
class Derived : FileBase { } // CS9053 エラー。
#endif
