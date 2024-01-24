using System;
using System.Windows.Input;

namespace Launcher.Share.Commands
{
    /// <summary>
    /// Implementation of ICommand interface that runs synchronous functions.
    /// </summary>
    public class Command : ICommand
    {
        private Action Action { get; set; }

        private bool _canExecute;

        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            Action = action;
            _canExecute = true;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter = null)
        {
            try
            {
                Action();
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Implementation of ICommand interface that runs synchronous functions with a parameter of a specified type.
    /// </summary>
    public class Command<TParameter> : ICommand where TParameter : class
    {
        private Action<TParameter> Action { get; set; }

        private bool _canExecute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<TParameter> action)
        {
            Action = action;
            _canExecute = true;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter = null)
        {
            try
            {
                if (parameter != null && parameter is TParameter)
                    Action(parameter as TParameter);
            }

            catch (Exception)
            {
                throw;
            }

            finally
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
