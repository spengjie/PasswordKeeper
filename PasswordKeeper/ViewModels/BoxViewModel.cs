using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using System.IO;
using System.Windows.Input;
using System.Xml;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PasswordKeeper
{
    class BoxViewModel : INotifyPropertyChanged
    {
        private Box _BoxInfo = new Box();
        public Box BoxInfo
        {
            get
            {
                return _BoxInfo;
            }
        }

        private string _Name = "";
        public string Name
        {
            get
            {
                _Name = Path.GetFileNameWithoutExtension(_BoxFilePath);
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _BoxFilePath = "";
        public string BoxFilePath
        {
            get
            {
                return _BoxFilePath;
            }
            set
            {
                _BoxFilePath = value;
                OnPropertyChanged("BoxFilePath");
            }
        }

        private string _LoginError = "";
        public string LoginError
        {
            get
            {
                return _LoginError;
            }
            set
            {
                _LoginError = value;
                OnPropertyChanged("LoginError");
            }
        }

        private bool _IsLogined = false;
        public bool IsLogined
        {
            get
            {
                if (!BoxInfo.IsAuthenEnabled)
                {
                    _IsLogined = true;
                }
                return _IsLogined;
            }
            set
            {
                _IsLogined = value;
                OnPropertyChanged("IsLogined");
            }
        }
        
        private RelayCommand _LoginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_LoginCommand == null)
                {
                    _LoginCommand = new RelayCommand(param => DoLogin(param.ToString()));
                }
                return _LoginCommand;
            }
        }

        private void DoLogin(string authenLoginPasswords)
        {
            if (BoxInfo.IsAuthenEnabled)
            {
                if (BoxInfo.LoginPassword == authenLoginPasswords)
                {
                    IsLogined = true;
                }
                else
                {
                    LoginError = "密码错误";
                }
            }
            else
            {
                IsLogined = true;
            }
        }

        private bool IsLoginEqual(string[] authenLoginPasswords)
        {
            return BoxInfo.LoginPassword == authenLoginPasswords[0] + authenLoginPasswords[1] + authenLoginPasswords[2] + authenLoginPasswords[4];
        }

        private ObservableCollection<AccountViewModel> _AccountViewModels = new ObservableCollection<AccountViewModel>();
        public ObservableCollection<AccountViewModel> AccountViewModels
        {
            get
            {
                return _AccountViewModels;
            }
        }

        private ICollectionView _AccountsView;
        public ICollectionView AccountsView
        {
            get
            {
                return _AccountsView;
            }
        }
        
        private ObservableCollection<string> _Tags = new ObservableCollection<string>();
        public ObservableCollection<string> Tags
        {
            get
            {
                foreach (AccountViewModel accountViewModel in _AccountViewModels)
                {
                    foreach (string tag in accountViewModel.MyTags)
                    {
                        if (!_Tags.Contains(tag))
                        {
                            _Tags.Add(tag);
                        }
                    }
                }
                return _Tags;
            }
        }

        private ICollectionView _TagsView;
        public ICollectionView TagsView
        {
            get
            {
                return _TagsView;
            }
        }

        public string _FilterString = "";
        public string FilterString
        {
            get
            {
                return _FilterString;
            }
            set
            {
                _FilterString = value;
                OnPropertyChanged("FilterString");
                _AccountsView.Refresh();
            }
        }

        private RelayCommand _FilterCommand;
        public ICommand FilterCommand
        {
            get
            {
                if (_FilterCommand == null)
                {
                    _FilterCommand = new RelayCommand(param => DoFilter(param.ToString()));
                }
                return _FilterCommand;
            }
        }

        public void DoFilter(string filterString)
        {
            string filterStringLower = filterString.ToLower();
            if (filterStringLower.StartsWith("tag:"))
            {
                string tagName = filterStringLower.Remove(0, 4).Trim();
                _AccountsView.Filter = delegate (object obj)
                {
                    AccountViewModel accountViewModel = obj as AccountViewModel;
                    return accountViewModel.MyTags.Contains(tagName);
                };
                FilterString = string.Format("Tag: {0}", tagName);
            }
            else
            {
                _AccountsView.Filter = delegate (object obj)
                {
                    AccountViewModel accountViewModel = obj as AccountViewModel;
                    string sName = accountViewModel.AccountInfo.Name;
                    string sUserName = accountViewModel.AccountInfo.Username;
                    if (string.IsNullOrEmpty(sName) && string.IsNullOrEmpty(sUserName)) return false;
                    int indexOfName = sName.ToLower().IndexOf(filterStringLower, 0);
                    int indexOfUsername = sUserName.IndexOf(filterStringLower, 0);
                    return (indexOfName > -1 || indexOfUsername > -1);
                };
                FilterString = filterString;
            }
        }

        private RelayCommand _ClearFilterCommand;
        public ICommand ClearFilterCommand
        {
            get
            {
                if (_ClearFilterCommand == null)
                {
                    _ClearFilterCommand = new RelayCommand(param => DoClearFilter(), param => CanDoClearFilter());
                }
                return _ClearFilterCommand;
            }
        }

        public void DoClearFilter()
        {
            _AccountsView.Filter = null;
            FilterString = "";
        }

        public bool CanDoClearFilter()
        {
            if (FilterString == "")
            {
                return false;
            }
            return true;
        }

        private RelayCommand _TagFilterCommand;
        public ICommand TagFilterCommand
        {
            get
            {
                if (_TagFilterCommand == null)
                {
                    _TagFilterCommand = new RelayCommand(param => DoTagFilter(param.ToString()));
                }
                return _TagFilterCommand;
            }
        }

        public void DoTagFilter(string tagName)
        {
            DoFilter(string.Format("Tag: {0}", tagName));
        }

        private RelayCommand _AddAccountCommand;
        public ICommand AddAccountCommand
        {
            get
            {
                if (_AddAccountCommand == null)
                {
                    _AddAccountCommand = new RelayCommand(param => DoAddAccount());
                }
                return _AddAccountCommand;
            }
        }

        public void DoAddAccount()
        {
            AccountViewModel accountViewModel = new AccountViewModel(Tags)
            {
                EditState = EnumViewState.EditingNew
            };
            accountViewModel.BeginEdit();
            AccountViewModels.Add(accountViewModel);
        }

        private RelayCommand _RemoveAccountCommand;
        public ICommand RemoveAccountCommand
        {
            get
            {
                if (_RemoveAccountCommand == null)
                {
                    _RemoveAccountCommand = new RelayCommand(param => DoRemoveAccount(param));
                }
                return _RemoveAccountCommand;
            }
        }

        public void DoRemoveAccount(object param)
        {
            AccountViewModel accountViewModel = param as AccountViewModel;
            AccountViewModels.Remove(accountViewModel);
            SaveBoxFile(_BoxFilePath);
        }

        private RelayCommand _CancelEditAccountCommand;
        public ICommand CancelEditAccountCommand
        {
            get
            {
                if (_CancelEditAccountCommand == null)
                {
                    _CancelEditAccountCommand = new RelayCommand(param => DoCancelEditAccount(param));
                }
                return _CancelEditAccountCommand;
            }
        }

        public void DoCancelEditAccount(object param)
        {
            AccountViewModel accountViewModel = param as AccountViewModel;
            if (accountViewModel.EditState == EnumViewState.EditingNew)
            {
                AccountViewModels.Remove(accountViewModel);
            }
            else if (accountViewModel.EditState == EnumViewState.EditingExisting)
            {
                accountViewModel.CancelEdit();
            }
        }

        private RelayCommand _SaveEditAccountCommand;
        public ICommand SaveEditAccountCommand
        {
            get
            {
                if (_SaveEditAccountCommand == null)
                {
                    _SaveEditAccountCommand = new RelayCommand(param => DoSaveEditAccount(param), param => CanDoSaveEditAccount(param));
                }
                return _SaveEditAccountCommand;
            }
        }

        public void DoSaveEditAccount(object param)
        {
            AccountViewModel accountViewModel = param as AccountViewModel;
            accountViewModel.EndEdit();
            SaveBoxFile(_BoxFilePath);
            _AccountsView.Refresh();
        }

        public bool CanDoSaveEditAccount(object param)
        {
            AccountViewModel accountViewModel = param as AccountViewModel;
            if (accountViewModel.EditState == EnumViewState.Normal)
            {
                return true;
            }
            return accountViewModel.EditingData.IsValidated;
        }

        private RelayCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(param => DoSave(param));
                }
                return _SaveCommand;
            }
        }

        public void DoSave(object param)
        {
            AccountViewModel accountViewModel = param as AccountViewModel;
            accountViewModel.EndEdit();
            SaveBoxFile(_BoxFilePath);
            _AccountsView.Refresh();
        }

        private RelayCommand _SaveLoginInfoCommand;
        public ICommand SaveLoginInfoCommand
        {
            get
            {
                if (_SaveLoginInfoCommand == null)
                {
                    _SaveLoginInfoCommand = new RelayCommand(param => DoSaveLoginInfo());
                }
                return _SaveLoginInfoCommand;
            }
        }

        public void DoSaveLoginInfo()
        {
            if (BoxInfo.IsLoginInfoModified)
            {
                SaveBoxFile(_BoxFilePath);
                BoxInfo.IsLoginInfoModified = false;
            }
        }

        private RelayCommand _CopyTextCommand;
        public ICommand CopyTextCommand
        {
            get
            {
                if (_CopyTextCommand == null)
                {
                    _CopyTextCommand = new RelayCommand(param => DoCopyText(param));
                }
                return _CopyTextCommand;
            }
        }

        public void DoCopyText(object param)
        {
            TextBlock textBlock = param as TextBlock;
            Clipboard.SetText(textBlock.Text);
        }

        public void CreateBoxFile(string boxFilePath)
        {
            SaveBoxFile(boxFilePath);
        }

        public bool OpenBoxFile(string boxFilePath)
        {
            if (!File.Exists(boxFilePath))
            {
                System.Windows.MessageBox.Show("File is not exist.");
                return false;
            }
            try
            {
                string XmlContent = Security.DecryptData(boxFilePath);
                XmlDocument XmlDoc = new XmlDocument();

                //载入XML
                XmlDoc.LoadXml(XmlContent);
                //获取Box节点
                XmlNode BoxNode = XmlDoc.SelectSingleNode("/Box");
                //获取Login信息
                _BoxInfo.IsAuthenEnabled = bool.Parse(BoxNode.SelectSingleNode("IsAuthenEnabled").InnerText);
                _BoxInfo.LoginPrompt = BoxNode.SelectSingleNode("LoginPrompt").InnerText;
                _BoxInfo.LoginPassword = BoxNode.SelectSingleNode("LoginPassword").InnerText;
                _BoxInfo.Version = new Version(BoxNode.SelectSingleNode("Version").InnerText);
                //获取Accounts信息
                XmlNode AccountsNode = BoxNode.SelectSingleNode("Accounts");
                XmlNodeList AccountNodes = AccountsNode.ChildNodes;
                foreach (XmlNode AccountNode in AccountNodes)
                {
                    Account account = new Account()
                    {
                        Name = AccountNode.SelectSingleNode("Name").InnerText,
                        Description = AccountNode.SelectSingleNode("Description").InnerText,
                        URL = AccountNode.SelectSingleNode("URL").InnerText,
                        Username = AccountNode.SelectSingleNode("Username").InnerText,
                        Password = AccountNode.SelectSingleNode("Password").InnerText
                    };

                    XmlNode ExtendAttributesNode = AccountNode.SelectSingleNode("ExtendAttributes");
                    XmlNodeList ExtendAttributeNodes = ExtendAttributesNode.ChildNodes;
                    foreach (XmlNode ExtendAttributeNode in ExtendAttributeNodes)
                    {
                        ExtendAttribute extendAttribute = new ExtendAttribute()
                        {
                            Name = ExtendAttributeNode.SelectSingleNode("Name").InnerText,
                            Value = ExtendAttributeNode.SelectSingleNode("Value").InnerText
                        };
                        account.ExtendAttributes.Add(extendAttribute);
                    }
                    AccountViewModel accountViewModel = new AccountViewModel(account, Tags);
                    XmlNode TagsNode = AccountNode.SelectSingleNode("Tags");
                    XmlNodeList TagNodes = TagsNode.ChildNodes;
                    foreach (XmlNode TagNode in TagNodes)
                    {
                        accountViewModel.MyTags.Add(TagNode.InnerText);
                    }
                    _AccountViewModels.Add(accountViewModel);
                }
                _BoxInfo.IsLoginInfoModified = false;
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("Invalid box file: {0}", ex.Message));
                return false;
            }
        }

        public void SaveBoxFile(string boxFilePath)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = XmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlDoc.AppendChild(xmlDecl);
            //设置Box节点
            XmlElement BoxElement = XmlDoc.CreateElement("Box");
            XmlDoc.AppendChild(BoxElement);
            XmlElement VersionElement = XmlDoc.CreateElement("Version");
            BoxElement.AppendChild(VersionElement);
            VersionElement.InnerText = _BoxInfo.Version.ToString();
            //设置Login信息
            XmlElement IsAuthenEnabledElement = XmlDoc.CreateElement("IsAuthenEnabled");
            BoxElement.AppendChild(IsAuthenEnabledElement);
            IsAuthenEnabledElement.InnerText = _BoxInfo.IsAuthenEnabled.ToString();
            XmlElement LoginPasswordElement = XmlDoc.CreateElement("LoginPassword");
            BoxElement.AppendChild(LoginPasswordElement);
            LoginPasswordElement.InnerText = _BoxInfo.LoginPassword.ToString();
            XmlElement LoginPromptElement = XmlDoc.CreateElement("LoginPrompt");
            BoxElement.AppendChild(LoginPromptElement);
            LoginPromptElement.InnerText = _BoxInfo.LoginPrompt.ToString();
            //设置Accounts信息
            XmlElement AccountsElement = XmlDoc.CreateElement("Accounts");
            BoxElement.AppendChild(AccountsElement);
            foreach (AccountViewModel accountViewModel in _AccountViewModels)
            {
                if (accountViewModel.EditState == EnumViewState.EditingNew)
                {
                    continue;
                }
                XmlElement AccountElement = XmlDoc.CreateElement("Account");
                AccountsElement.AppendChild(AccountElement);

                XmlElement NameElement = XmlDoc.CreateElement("Name");
                AccountElement.AppendChild(NameElement);
                XmlElement DescriptionElement = XmlDoc.CreateElement("Description");
                AccountElement.AppendChild(DescriptionElement);
                NameElement.InnerText = accountViewModel.AccountInfo.Name;
                XmlElement URLElement = XmlDoc.CreateElement("URL");
                AccountElement.AppendChild(URLElement);
                URLElement.InnerText = accountViewModel.AccountInfo.URL;
                XmlElement UsernameElement = XmlDoc.CreateElement("Username");
                AccountElement.AppendChild(UsernameElement);
                UsernameElement.InnerText = accountViewModel.AccountInfo.Username;
                XmlElement PasswordElement = XmlDoc.CreateElement("Password");
                AccountElement.AppendChild(PasswordElement);
                PasswordElement.InnerText = accountViewModel.AccountInfo.Password;

                int i = 0;
                while (i < accountViewModel.AccountInfo.ExtendAttributes.Count)
                {
                    if (accountViewModel.AccountInfo.ExtendAttributes[i].Name == "" && accountViewModel.AccountInfo.ExtendAttributes[i].Value == "")
                    {
                        accountViewModel.AccountInfo.ExtendAttributes.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                XmlElement ExtendAttributesElement = XmlDoc.CreateElement("ExtendAttributes");
                AccountElement.AppendChild(ExtendAttributesElement);
                foreach (ExtendAttribute extendAttribute in accountViewModel.AccountInfo.ExtendAttributes)
                {
                    XmlElement ExtendAttributeElement = XmlDoc.CreateElement("ExtendAttribute");
                    ExtendAttributesElement.AppendChild(ExtendAttributeElement);

                    XmlElement ExNameElement = XmlDoc.CreateElement("Name");
                    ExtendAttributeElement.AppendChild(ExNameElement);
                    ExNameElement.InnerText = extendAttribute.Name;
                    XmlElement ExValueElement = XmlDoc.CreateElement("Value");
                    ExtendAttributeElement.AppendChild(ExValueElement);
                    ExValueElement.InnerText = extendAttribute.Value;
                }

                XmlElement TagsElement = XmlDoc.CreateElement("Tags");
                AccountElement.AppendChild(TagsElement);
                foreach (string tag in accountViewModel.MyTags)
                {
                    XmlElement TagElement = XmlDoc.CreateElement("Tag");
                    TagElement.InnerText = tag;
                    TagsElement.AppendChild(TagElement);
                }
            }
            Security.EncryptData(XmlDoc.OuterXml, boxFilePath);
        }

        public BoxViewModel(string boxFilePath)
        {
            InitializeView();
            BoxFilePath = boxFilePath;
        }

        private void InitializeView()
        {
            _AccountsView = CollectionViewSource.GetDefaultView(_AccountViewModels);
            _AccountsView.SortDescriptions.Add(new SortDescription("AccountInfo.Name", ListSortDirection.Ascending));
            _AccountsView.GroupDescriptions.Add(new PropertyGroupDescription("AccountInfo.Name"));
            _TagsView = CollectionViewSource.GetDefaultView(_Tags);
            _TagsView.SortDescriptions.Add(new SortDescription());
        }

        public void DestoryIcons()
        {
            foreach (AccountViewModel accountViewModel in AccountViewModels)
            {
                accountViewModel.Icon = null;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
