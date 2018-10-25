using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace PasswordKeeper
{
    public class ExtendAttribute : INotifyPropertyChanged, IDataErrorInfo
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

        private string _Value = "";
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    IsStringValid("Value", value);
                    OnPropertyChanged("Value");
                }
            }
        }

        public ExtendAttribute()
        {
        }

        public ExtendAttribute(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IDataErrorInfo Members

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public bool IsStringValid(string property, string value)
        {
            bool isValid = true;

            if (value == string.Empty)
            {
                AddError(property, "Cannot be empty");
                isValid = false;
            }
            else
            {
                RemoveError(property, "Cannot be empty");
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
        }

        public bool IsValidated
        {
            get
            {
                IsStringValid("Name", Name);
                IsStringValid("Value", Value);
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
