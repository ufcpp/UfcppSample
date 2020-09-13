using System.Windows.Input;

namespace UserInputSample
{
    public class InputData : NotificationObject
    {
        void Run()
        {
            //TODO: ↓ここに演習問題の回答コードを書いてください

            Result = A + B;
        }

        public double A
        {
            get => _a;
            set { _a = value; RaisePropertyChanged(nameof(A)); }
        }
        private double _a;

        public double B
        {
            get => _b;
            set { _b = value; RaisePropertyChanged(nameof(B)); }
        }
        private double _b;

        public double C
        {
            get => _c;
            set { _c = value; RaisePropertyChanged(nameof(C)); }
        }
        private double _c;

        public double D
        {
            get => _d;
            set { _d = value; RaisePropertyChanged(nameof(D)); }
        }
        private double _d;

        public double E
        {
            get => _e;
            set { _e = value; RaisePropertyChanged(nameof(E)); }
        }
        private double _e;

        public double Result
        {
            get { return _r; }
            set { _r = value; RaisePropertyChanged("Result"); }
        }
        private double _r;

        public ICommand RunCommand => _runCommand ??= new DelegateCommand(Run);
        public ICommand? _runCommand;
    }
}
