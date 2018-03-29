namespace ConsoleApp1.Ref
{
    struct Node
    {
        public int Value;
        public int NextIndex;
        public Node(int value, int nextIndex) => (Value, NextIndex) = (value, nextIndex);
    }
}
