using System;

using System.Windows.Input;

namespace PasswordKeeper
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    // To use this class within your viewmodel class:
    //private RelayCommand _myCommand;
    //public ICommand MyCommand
    //{
    //    get
    //    {
    //        if (_myCommand == null)
    //        {
    //            _myCommand = new RelayCommand(p => this.DoMyCommand(p),
    //                p => this.CanDoMyCommand(p));
    //        }
    //        return _myCommand;
    //    }
    //}
}
