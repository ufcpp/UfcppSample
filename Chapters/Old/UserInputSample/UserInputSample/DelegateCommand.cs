using System;
using System.Windows.Input;

namespace UserInputSample
{
    public class DelegateCommand : ICommand
    {
        Action executer;

        public DelegateCommand(Action executer) { this.executer = executer; }

        public void Execute(object parameter)
        {
            executer();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var d = CanExecuteChanged;
            if (d != null)
                d(this, null);
        }
    }
}
