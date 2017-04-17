using System;
using System.Windows.Input;

namespace PlaylistManager
{
    public class ActionCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> execute;

        public ActionCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public ActionCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("The action is null.");
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }

    public class ActionCommand : ActionCommand<object>
    {
        public ActionCommand(Action execute)
            : base(o => execute())
        {
        }

        public ActionCommand(Action<object> execute)
            : base(execute)
        {
        }

        public ActionCommand(Action execute, Func<bool> canExecute)
            : base(o => execute(), o => canExecute())
        {
        }

        public ActionCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }
    }
}
