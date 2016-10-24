namespace TypeName2
{
    public class Sample
    {
        public enum Color
        {
            Green,
            Yellow,
            Red,
        }

        // enum の Color と同じスコープ内でプロパティの Color を作ろうとしていて
        // この場合はコンパイル エラーになる
        //public Color Color { get; set; }
    }
}
