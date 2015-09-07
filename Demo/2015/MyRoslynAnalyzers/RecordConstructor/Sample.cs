namespace RecordConstructor
{
    class Sample
    {
        public int X { get; }
        public int Y { get; }

        /// <summary>Record Constructor</summary>
        /// <param name="x"><see cref="X"/></param>
        /// <param name="y"><see cref="Y"/></param>
        public Sample(int x = default(int), int y = default(int))
        {
            X = x;
            Y = y;
        }
    }
}
