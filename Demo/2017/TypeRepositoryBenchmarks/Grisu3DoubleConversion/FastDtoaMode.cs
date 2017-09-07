namespace Grisu3DoubleConversion
{
    public enum FastDtoaMode
    {
        // Computes the shortest representation of the given input. The returned
        // result will be the most accurate number of this length. Longer
        // representations might be more accurate.
        FAST_DTOA_SHORTEST,
        // Same as FAST_DTOA_SHORTEST but for single-precision floats.
        FAST_DTOA_SHORTEST_SINGLE,
        // Computes a representation where the precision (number of digits) is
        // given as input. The precision is independent of the decimal point.
        FAST_DTOA_PRECISION
    };
}