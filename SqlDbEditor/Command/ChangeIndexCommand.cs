using System;
using System.Windows.Input;

namespace SqlDbEditor.Command
{
    public class ChangeIndexCommand : ICommand
    {
        private readonly Action executeAction;
        private readonly Func<bool> canExecuteFunc;

        public event EventHandler CanExecuteChanged;

        public ChangeIndexCommand(Action executeAction, Func<bool> canExecuteFunc = null)
        {
            this.executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            this.canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteFunc != null)
            {
                return canExecuteFunc();
            }

            return true;
        }

        public void Execute(object parameter)
        {
            executeAction();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
