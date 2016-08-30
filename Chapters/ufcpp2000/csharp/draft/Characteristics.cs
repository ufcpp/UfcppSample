using System.Diagnostics.Contracts;

namespace TypeDefinition.Models
{
    /// <summary>
    /// 特性データ
    /// </summary>
    public partial class Characteristics
    {
        public Characteristics(double X, double Y, double Z)
        {
            Contract.Ensures(this.Validate());

            this._X = X;
            this._Y = Y;
            this._Z = Z;
        }

        /// <summary>
        /// 特性 X
        /// </summary>
        public double X { get { return _X; } }
        private readonly double _X;

        /// <summary>
        /// 特性 Y
        /// </summary>
        public double Y { get { return _Y; } }
        private readonly double _Y;

        /// <summary>
        /// 特性 Z
        /// </summary>
        public double Z { get { return _Z; } }
        private readonly double _Z;

        /// <summary>
        /// 一次ノルム
        /// </summary>
        public double Norm { get { return X + Y + Z; } }

        public bool Validate()
        {
            return
                0 <= X && X <= 1
                && 0 <= Y && Y <= 1
                && 0 <= Z && Z <= 1
                && Norm >= 1;
        }
    }
}
