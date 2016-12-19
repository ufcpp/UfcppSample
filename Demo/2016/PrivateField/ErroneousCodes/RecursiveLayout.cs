struct Container<T>
{
    public T Item;
}

struct RecursiveLayout
{
    // 無限再帰するので、この構造体はレイアウトが確定できない
    Container<RecursiveLayout> _x; // PCL ではエラーにならない
}
