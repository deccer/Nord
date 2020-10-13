using System;
using System.Windows.Input;

namespace AtlasStudio.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _action;
        private readonly Predicate<T> _canExecute;

        public DelegateCommand(Action<T> action, Predicate<T> canExecute = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
    }
}
