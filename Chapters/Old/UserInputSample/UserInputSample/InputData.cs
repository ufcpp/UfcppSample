using System.Windows.Input;

namespace UserInputSample
{
    public class InputData : NotificationObject
    {
        void Run()
        {
            //TODO: ↓ここに演習問題の回答コードを書いてください

            this.Result = this.A + this.B;
        }

        public double A
        {
            get { return _a; }
            set { _a = value; RaisePropertyChanged("A"); }
        }
        private double _a;

        public double B
        {
            get { return _b; }
            set { _b = value; RaisePropertyChanged("B"); }
        }
        private double _b;

        public double C
        {
            get { return _c; }
            set { _c = value; RaisePropertyChanged("C"); }
        }
        private double _c;

        public double D
        {
            get { return _d; }
            set { _d = value; RaisePropertyChanged("D"); }
        }
        private double _d;

        public double E
        {
            get { return _e; }
            set { _e = value; RaisePropertyChanged("E"); }
        }
        private double _e;

        public double Result
        {
            get { return _r; }
            set { _r = value; RaisePropertyChanged("Result"); }
        }
        private double _r;

        public ICommand RunCommand
        {
            get
            {
                if (_runCommand == null)
                {
                    _runCommand = new DelegateCommand(this.Run);
                }

                return _runCommand;
            }
        }

        public ICommand _runCommand;
    }
}
