namespace System.Runtime.CompilerServices
{
    public class FormattableStringFactory
    {
        public static FormattableString Create(string format, params object[] args)
        {
            return null;
        }
    }

    public class FormattableString : IFormattable
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
