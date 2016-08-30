using System;
using System.Windows.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TypeDefinition.Models
{
    /// <summary>
    /// 特性データ の ViewModel
    /// </summary>
    public partial class CharacteristicsViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private double _X;

        /// <summary>
        /// 指標 X
        /// </summary>
        [Range(0.0, 1.0)]
        public double X
        {
            get { return _X; }
            set
            {
                _X = value;
                RaisePropertyChanged("X");
                RaiseErrorsChanged("X");
                RaisePropertyChanged("Norm");
                RaiseErrorsChanged("Norm");
                _Submit.RaiseCanExecuteChanged();
            }
        }

        private double _Y;

        /// <summary>
        /// 指標 Y
        /// </summary>
        [Range(0.0, 1.0)]
        public double Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                RaisePropertyChanged("Y");
                RaiseErrorsChanged("Y");
                RaisePropertyChanged("Norm");
                RaiseErrorsChanged("Norm");
                _Submit.RaiseCanExecuteChanged();
            }
        }

        private double _Z;

        /// <summary>
        /// 指標 Z
        /// </summary>
        [Range(0.0, 1.0)]
        public double Z
        {
            get { return _Z; }
            set
            {
                _Z = value;
                RaisePropertyChanged("Z");
                RaiseErrorsChanged("Z");
                RaisePropertyChanged("Norm");
                RaiseErrorsChanged("Norm");
                _Submit.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// 一次ノルム
        /// </summary>
        public double Norm
        {
            get { return X + Y + Z; }
        }

        DelegateCommand _Submit;

        /// <summary>
        /// コマンドバインディング用 Submit コマンド
        /// </summary>
        public ICommand SubmitCommand
        {
            get
            {
                if (_Submit == null)
                    _Submit = new DelegateCommand { ExecuteHandler = OnSubmit, CanExecuteHandler = OnSubmitCanExcute };
                return _Submit;
            }
        }

        /// <summary>
        /// 変更の適用操作
        /// </summary>
        public Action<object> Submit { get; set; }

        void OnSubmit(object paramter)
        {
            var d = Submit;
            if (d != null)
                d(paramter);
        }

        bool OnSubmitCanExcute(object paramter)
        {
            return HasErrors;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            var d = this.PropertyChanged;
            if (d != null)
                d(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyDataErrorInfo Members

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        void RaiseErrorsChanged(string propertyName)
        {
            var d = this.ErrorsChanged;
            if (d != null)
                d(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            switch(propertyName)
            {
                case "X": if (!(0 <= X && X <= 1)) yield return "requires: 0 <= X && X <= 1"; break;
                case "Y": if (!(0 <= Y && Y <= 1)) yield return "requires: 0 <= Y && Y <= 1"; break;
                case "Z": if (!(0 <= Z && Z <= 1)) yield return "requires: 0 <= Z && Z <= 1"; break;
                case "Norm": if (!(Norm >= 1)) yield return "requires: Norm >= 1"; break;
            }
        }

        public bool HasErrors
        {
            get
            {
                return !(
                    0 <= X && X <= 1
                    && 0 <= Y && Y <= 1
                    && 0 <= Z && Z <= 1
                    && Norm >= 1
                    );
            }
        }

        #endregion
    }
}
