// file-local なインターフェイスも OK だし、
// それを public な型で実装するのも OK。

public class CX : IX // OK
{
    // file 修飾子は top-level の型にしか付けれないみたいで、
    // file void M() { } とかは書けない。

    // でも、 file-local インターフェイス で明示的実装しておけば実質 file-local メソッドに。
    void IX.M() { }
}

file interface IX
{
    void M(); // これを明示的実装すれば file 内限定メソッド作れる。
}
