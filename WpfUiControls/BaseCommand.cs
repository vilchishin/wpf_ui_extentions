using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUiControls.ViewModels
{
    public class BaseCommand : ICommand
    {
        #region Private fields

        private Action execute;
        private Func<bool> canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of BaseCommand class
        /// </summary>
        /// <param name="execute">Action that executes by the command</param>
        /// <param name="canExecute">The execution condition</param>
        public BaseCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand interface members

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute();
        }

        #endregion
    }

    public class BaseCommand<T> : ICommand
    {
        #region Private fields

        private Action<T> execute;
        private Func<T, bool> canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of BaseCommand class
        /// </summary>
        /// <param name="execute">Action that executes by the command</param>
        /// <param name="canExecute">The execution condition</param>
        public BaseCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand interface members

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        #endregion
    }

}
