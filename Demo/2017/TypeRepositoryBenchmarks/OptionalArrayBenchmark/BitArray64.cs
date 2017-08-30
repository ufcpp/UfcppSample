struct BitArray64
{
    long _bits;

    public bool this[int index]
    {
        get => (_bits & (1L << index)) != 0L;
        set
        {
            if (value) _bits |= (1L << index);
            else _bits &= ~(1L << index);
        }
    }
}
