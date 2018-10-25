using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace PasswordKeeper
{
    public enum EnumViewState
    {
        Normal = 0,
        EditingExisting = 1,
        EditingNew = 2
    }

    class AccountViewModel : IEditableObject, INotifyPropertyChanged
    {
        Account _AccountInfo;
        public Account AccountInfo
        {
            get
            {
                return _AccountInfo;
            }
        }

        public string Name_URL
        {
            get
            {
                return _AccountInfo.Name + _AccountInfo.URL;
            }
        }

        private BitmapImage _DownloadedIcon = new BitmapImage();
        private static Uri _DefaultIconUri = new Uri(@"pack://application:,,,/PasswordKeeper;component/Resources/DefaultFavicon.ico", UriKind.RelativeOrAbsolute);
        private static BitmapImage _DefaultIcon = new BitmapImage(_DefaultIconUri);
        private BitmapImage _Icon = _DefaultIcon;
        public BitmapImage Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                if (_Icon != value)
                {
                    _Icon = value;
                }
            }
        }

        public void GetUrlIcon(string url)
        {
            Match website = Regex.Match(url, "(?:http://|https://)?([a-zA-Z0-9][-a-zA-Z0-9\\.]*)/?.*");
            if (!website.Success)
            {
                return;
            }
            string WebsiteUrl = website.Groups[1].Value;
            string FaviconUrl;
            if (url.StartsWith("https://"))
            {
                FaviconUrl = "https://" + WebsiteUrl + "/favicon.ico";
            }
            else
            {
                FaviconUrl = "http://" + WebsiteUrl + "/favicon.ico";
            }
            Uri uri = new Uri(FaviconUrl);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmapImage.DownloadCompleted += delegate
            {
                Icon.UriSource = null;
                Icon = _DownloadedIcon;
                _DownloadedIcon = null;
                OnPropertyChanged("Icon");
            };
            bitmapImage.DownloadFailed += delegate
            {
                Icon = _DefaultIcon;
                OnPropertyChanged("Icon");
            };
            bitmapImage.EndInit();
            _DownloadedIcon = bitmapImage;
        }

        private ObservableCollection<string> _AllTags = new ObservableCollection<string>();
        public ObservableCollection<string> AllTags
        {
            get
            {
                return _AllTags;
            }
        }

        private ObservableCollection<string> _MyTags = new ObservableCollection<string>();
        public ObservableCollection<string> MyTags
        {
            get
            {
                return _MyTags;
            }
        }

        private ICollectionView _MyTagsView;
        public ICollectionView MyTagsView
        {
            get
            {
                return _MyTagsView;
            }
        }

        private ObservableCollection<Tag> _Tags = new ObservableCollection<Tag>();
        public ObservableCollection<Tag> Tags
        {
            get
            {
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

        private Account _EditingData;
        public Account EditingData
        {
            get
            {
                return _EditingData;
            }
        }

        private EnumViewState _EditState = EnumViewState.Normal;
        public EnumViewState EditState
        {
            get
            {
                return _EditState;
            }
            set
            {
                if (_EditState != value)
                {
                    _EditState = value;
                    OnPropertyChanged("EditState");
                }
            }
        }

        private bool _TagEditing = false;
        public bool TagEditing
        {
            get
            {
                return _TagEditing;
            }
            set
            {
                if (_TagEditing != value)
                {
                    _TagEditing = value;
                    OnPropertyChanged("TagEditing");
                }
            }
        }

        private RelayCommand _AddTagCommand;
        public ICommand AddTagCommand
        {
            get
            {
                if (_AddTagCommand == null)
                {
                    _AddTagCommand = new RelayCommand(param => DoAddTag());
                }
                return _AddTagCommand;
            }
        }

        public void DoAddTag()
        {
            TagEditing = true;
        }

        private RelayCommand _CancelTagEditCommand;
        public ICommand CancelTagEditCommand
        {
            get
            {
                if (_CancelTagEditCommand == null)
                {
                    _CancelTagEditCommand = new RelayCommand(param => DoCancelTagEdit());
                }
                return _CancelTagEditCommand;
            }
        }

        public void DoCancelTagEdit()
        {
            TagEditing = false;
        }

        private RelayCommand _SaveTagEditCommand;
        public ICommand SaveTagEditCommand
        {
            get
            {
                if (_SaveTagEditCommand == null)
                {
                    _SaveTagEditCommand = new RelayCommand(param => DoSaveTagEdit(param));
                }
                return _SaveTagEditCommand;
            }
        }

        public void DoSaveTagEdit(object param)
        {
            string tagName = param as string;
            TagEditing = false;
            _Tags.Add(new Tag(tagName, true));
            _AllTags.Add(tagName);
        }

        public void BeginEdit()
        {
            _EditingData = new Account();
            if (EditState == EnumViewState.Normal)
            {
                EditState = EnumViewState.EditingExisting;
                _EditingData.Name = _AccountInfo.Name;
                _EditingData.Description = _AccountInfo.Description;
                _EditingData.URL = _AccountInfo.URL;
                _EditingData.Username = _AccountInfo.Username;
                _EditingData.Password = _AccountInfo.Password;
                foreach (ExtendAttribute eachExtendAttribute in _AccountInfo.ExtendAttributes)
                {
                    ExtendAttribute extendAttribute = new ExtendAttribute()
                    {
                        Name = eachExtendAttribute.Name,
                        Value = eachExtendAttribute.Value
                    };
                    _EditingData.ExtendAttributes.Add(extendAttribute);
                }
            }
            foreach (string tagName in _AllTags)
            {
                Tag tag = new Tag(tagName);
                if (_MyTags.Contains(tagName))
                {
                    tag.IsSelected = true;
                }
                _Tags.Add(tag);
            }
        }

        public void CancelEdit()
        {
            _Tags.Clear();
            EditState = EnumViewState.Normal;
            _EditingData = null;
        }

        public void EndEdit()
        {
            _MyTags.Clear();
            foreach (Tag tag in _Tags)
            {
                if (tag.IsSelected)
                {
                    _MyTags.Add(tag.Name);
                }
            }
            _Tags.Clear();
            EditState = EnumViewState.Normal;
            if (_AccountInfo.URL != _EditingData.URL)
            {
                GetUrlIcon(_EditingData.URL);
            }
            _AccountInfo.Update(_EditingData);
            _EditingData = null;
        }

        private RelayCommand _EditCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new RelayCommand(param => BeginEdit());
                }
                return _EditCommand;
            }
        }

        private RelayCommand _AddAttributeCommand;
        public ICommand AddAttributeCommand
        {
            get
            {
                if (_AddAttributeCommand == null)
                {
                    _AddAttributeCommand = new RelayCommand(param => DoAddAttribute());
                }
                return _AddAttributeCommand;
            }
        }

        private void DoAddAttribute()
        {
            ExtendAttribute extendAttribute = new ExtendAttribute();
            _EditingData.ExtendAttributes.Add(extendAttribute);
        }

        private RelayCommand _RemoveAttributeCommand;
        public ICommand RemoveAttributeCommand
        {
            get
            {
                if (_RemoveAttributeCommand == null)
                {
                    _RemoveAttributeCommand = new RelayCommand(param => DoRemoveAttribute(param));
                }
                return _RemoveAttributeCommand;
            }
        }

        private void DoRemoveAttribute(object param)
        {
            ExtendAttribute extendAttribute = param as ExtendAttribute;
            _EditingData.ExtendAttributes.Remove(extendAttribute);
        }

        private RelayCommand _OpenUrlCommand;
        public ICommand OpenUrlCommand
        {
            get
            {
                if (_OpenUrlCommand == null)
                {
                    _OpenUrlCommand = new RelayCommand(param => DoOpenUrlCommand());
                }
                return _OpenUrlCommand;
            }
        }

        private void DoOpenUrlCommand()
        {
            string errMsg = "";
            try
            {
                if (_AccountInfo.URL != "")
                {
                    System.Diagnostics.Process.Start(_AccountInfo.URL);
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                errMsg = uae.Message;
            }
            catch (Exception we)
            {
                errMsg = we.Message;
            }
        }

        public AccountViewModel(ObservableCollection<string> tags)
        {
            _AllTags = tags;
            InitializeView();
            _AccountInfo = new Account();
        }

        public AccountViewModel(Account accountInfo, ObservableCollection<string> tags)
        {
            _AllTags = tags;
            InitializeView();
            _AccountInfo = accountInfo;
            //GetUrlIcon(_AccountInfo.URL);
        }

        private void InitializeView()
        {
            _TagsView = CollectionViewSource.GetDefaultView(_Tags);
            _TagsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _MyTagsView = CollectionViewSource.GetDefaultView(_MyTags);
            _MyTagsView.SortDescriptions.Add(new SortDescription());
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
