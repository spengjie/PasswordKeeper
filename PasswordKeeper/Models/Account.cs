using System;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading;

namespace PasswordKeeper
{
    public class Account : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _Name = "";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    IsStringValid("Name", value);
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _URL = "";
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                if (_URL != value)
                {
                    _URL = value;
                    OnPropertyChanged("URL");
                }
            }
        }

        private string _Description = "";
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _Username = "";
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    IsStringValid("Username", value);
                    OnPropertyChanged("Username");
                }
            }
        }

        private string _Password = "";
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    IsStringValid("Password", value);
                    OnPropertyChanged("Password");
                }
            }
        }

        private ObservableCollection<ExtendAttribute> _ExtendAttributes = new ObservableCollection<ExtendAttribute>();
        public ObservableCollection<ExtendAttribute> ExtendAttributes
        {
            get
            {
                return _ExtendAttributes;
            }
        }

        public void Update(Account account)
        {
            Name = account.Name;
            Description = account.Description;
            URL = account.URL;
            Username = account.Username;
            Password = account.Password;
            ExtendAttributes.Clear();
            foreach (ExtendAttribute eachExtendAttribute in account.ExtendAttributes)
            {
                ExtendAttribute extendAttribute = new ExtendAttribute()
                {
                    Name = eachExtendAttribute.Name,
                    Value = eachExtendAttribute.Value
                };
                ExtendAttributes.Add(extendAttribute);
            }
        }

        public Account()
        {
        }

        public Account(string[] items)
        {
            int iLength = items.Length;
            switch (iLength)
            {
                case (1):
                    {
                        _Name = items[0];
                        break;
                    }
                case (2):
                    {
                        _Name = items[0];
                        _URL = items[1];
                        break;
                    }
                case (3):
                    {
                        _Name = items[0];
                        _URL = items[1];
                        _Username = items[2];
                        break;
                    }
                case (4):
                    {
                        _Name = items[0];
                        _URL = items[1];
                        _Username = items[2];
                        _Password = items[3];
                        break;
                    }
                case (5):
                    {
                        _Name = items[0];
                        _Description = items[1];
                        _URL = items[2];
                        _Username = items[3];
                        _Password = items[4];
                        break;
                    }
            }
        }

        public Account(string[] items, ObservableCollection<ExtendAttribute> extendAttrs)
        {
            if (items.Length == 4)
            {
                _Name = items[0];
                _URL = items[1];
                _Username = items[2];
                _Password = items[3];
            }
            else
            {
                _Name = items[0];
                _Description = items[1];
                _URL = items[2];
                _Username = items[3];
                _Password = items[4];
            }
            _ExtendAttributes = extendAttrs;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo Members

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        
        public bool IsStringValid(string property, string value)
        {
            bool isValid = true;

            if (value == string.Empty)
            {
                AddError(property, string.Format("Cannot be empty: {0}", property));
                isValid = false;
            }
            else
            {
                RemoveError(property, string.Format("Cannot be empty: {0}", property));
            }

            return isValid;
        }

        public void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
                errors[propertyName].Add(error);
            OnPropertyChanged("IsValidated");
            Console.WriteLine("AddError");
            foreach (KeyValuePair<string, List<string>> kv in errors)
            {
                Console.WriteLine("Key: {0}", kv.Key);
                foreach (string v in kv.Value)
                {
                    Console.WriteLine("Value: {0}", v);
                }
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);

                if (errors[propertyName].Count == 0)
                    errors.Remove(propertyName);
            }
            OnPropertyChanged("IsValidated");
            Console.WriteLine("RemoveError");
            foreach (KeyValuePair<string, List<string>> kv in errors)
            {
                Console.WriteLine("Key: {0}", kv.Key);
                foreach (string v in kv.Value)
                {
                    Console.WriteLine("Value: {0}", v);
                }
            }
        }
        
        public bool IsValidated
        {
            get
            {
                IsStringValid("Name", Name);
                IsStringValid("Username", Username);
                IsStringValid("Password", Password);
                foreach (ExtendAttribute extendAttribute in ExtendAttributes)
                {
                    if (!extendAttribute.IsValidated)
                    {
                        return false;
                    }
                }
                return errors.Count == 0;
            }
        }

        public string Error
        {
            get { return errors.Count > 0 ? "有验证错误" : "没有验证错误"; }
        }

        public string this[string property]
        {
            get
            {
                if (errors.ContainsKey(property))
                    return string.Join(Environment.NewLine, errors[property]);
                else
                    return null;
            }
        }

        #endregion
    }
}
