﻿using System;
using System.Windows.Input;
using System.ComponentModel;

namespace ConsoleApplication1
{
    /// <summary>
    /// 特性値データ の ViewModel
    /// </summary>
    public partial class SampleClass : INotifyPropertyChanged
    {
        private double _X;

        /// <summary>
        /// 特性 X
        /// </summary>
        public double X
        {
            get { return _X; }
            set
            {
                if (_X != value)
                {
                    _X = value;
                    RaisePropertyChanged("X");
                }
            }
        }

        private int _Y;

        /// <summary>
        /// 特性 Y
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    RaisePropertyChanged("Y");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            var f = this.PropertyChanged;
            if (f != null)
                f(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
