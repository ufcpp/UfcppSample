using System.Globalization;

namespace System
{
    public abstract class FormattableString : IFormattable
    {
        public abstract string Format { get; }
        public abstract int ArgumentCount { get; }
        public abstract object[] GetArguments();
        public abstract object GetArgument(int index);
        public abstract string ToString(IFormatProvider formatProvider);
        string IFormattable.ToString(string ignored, IFormatProvider formatProvider) => ToString(formatProvider);
        public static string Invariant(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException(nameof(formattable));
            }
            return formattable.ToString(CultureInfo.InvariantCulture);
        }
        public override string ToString() => ToString(CultureInfo.CurrentCulture);
        protected FormattableString() { }
    }
}

namespace System.Runtime.CompilerServices
{
    public static class FormattableStringFactory
    {
        private sealed class ConcreteFormattableString : FormattableString
        {
            private readonly string _format;
            private readonly object[] _arguments;
            public override string Format => _format;
            public override int ArgumentCount => _arguments.Length;
            internal ConcreteFormattableString(string format, object[] arguments)
            {
                _format = format;
                _arguments = arguments;
            }
            public override object[] GetArguments() => _arguments;
            public override object GetArgument(int index) => _arguments[index];
            public override string ToString(IFormatProvider formatProvider) => string.Format(formatProvider, _format, _arguments);
        }
        public static FormattableString Create(string format, params object[] arguments)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));
            return new ConcreteFormattableString(format, arguments);
        }
    }
}
