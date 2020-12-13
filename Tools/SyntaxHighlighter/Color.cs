using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxHighlighter
{
    /// <summary>
    /// RGB。
    /// System.Windows.Color とか使えばよかったかもしれない。
    /// A が要らないから自作。
    /// </summary>
    [Serializable]
    public struct Color
    {
        private uint v_;

        public byte R
        {
            get { return (byte)((v_ >> 0) & 0xff); }
            set { v_ = (v_ & 0x00ffff) | (((uint)value) << 0); }
        }
        public byte G
        {
            get { return (byte)((v_ >> 8) & 0xff); }
            set { v_ = (v_ & 0x00ffff) | (((uint)value) << 8); }
        }
        public byte B
        {
            get { return (byte)((v_ >> 16) & 0xff); }
            set { v_ = (v_ & 0x00ffff) | (((uint)value) << 16); }
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Color))
                return false;
            var other = (Color)obj;
            return v_ == other.v_;
        }

        public override int GetHashCode()
        {
            return v_.GetHashCode();
        }

        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !(left == right);
    }
}
