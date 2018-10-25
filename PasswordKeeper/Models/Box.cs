
using System.ComponentModel;

namespace PasswordKeeper
{
    public class Box : INotifyPropertyChanged
    {
        private Version _Version = new Version("0.0.0.0");
        public Version Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }

        private bool _IsAuthenEnabled = false;
        public bool IsAuthenEnabled
        {
            get
            {
                return _IsAuthenEnabled;
            }
            set
            {
                if (_IsAuthenEnabled != value)
                {
                    _IsAuthenEnabled = value;
                    OnPropertyChanged("IsAuthenEnabled");
                    _IsLoginInfoModified = true;
                }
            }
        }

        private string _LoginPrompt = "";
        public string LoginPrompt
        {
            get
            {
                return _LoginPrompt;
            }
            set
            {
                if (_LoginPrompt != value)
                {
                    _LoginPrompt = value;
                    OnPropertyChanged("LoginPrompt");
                    _IsLoginInfoModified = true;
                }
            }
        }

        private string _LoginPassword = "";
        public string LoginPassword
        {
            get
            {
                return _LoginPassword;
            }
            set
            {
                if (_LoginPassword != value)
                {
                    _LoginPassword = value;
                    OnPropertyChanged("LoginPassword");
                    _IsLoginInfoModified = true;
                }
            }
        }
        
        private bool _IsLoginInfoModified = false;
        public bool IsLoginInfoModified
        {
            get
            {
                return _IsLoginInfoModified;
            }
            set
            {
                if (_IsLoginInfoModified != value)
                {
                    _IsLoginInfoModified = value;
                }
            }
        }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
