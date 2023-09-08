using System;
using System.Windows.Input;

namespace SqlDbEditor.Command
{
    /// <summary>
    /// A custom implementation of the ICommand interface for handling index change events.
    /// </summary>
    public class ChangeIndexCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecuteFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeIndexCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The action to execute when the command is invoked.</param>
        /// <param name="canExecuteFunc">An optional function that determines if the command can be executed.</param>
        public ChangeIndexCommand(Action executeAction, Func<bool> canExecuteFunc = null)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            _canExecuteFunc = canExecuteFunc;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc == null || _canExecuteFunc();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _executeAction();
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event to indicate that the ability of the command to execute has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
