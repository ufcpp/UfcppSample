using System;
using System.Windows.Input;

namespace UserInputSample
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _executer;

        public DelegateCommand(Action executer) { _executer = executer; }

        public void Execute(object? parameter) => _executer();

        public bool CanExecute(object? parameter) => true;

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null!);
        }
    }
}
