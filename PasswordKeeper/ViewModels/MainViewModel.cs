using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Media.Animation;

namespace PasswordKeeper
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BoxViewModel> _BoxViewModels = new ObservableCollection<BoxViewModel>();
        public ObservableCollection<BoxViewModel> BoxViewModels
        {
            get
            {
                return _BoxViewModels;
            }
        }

        private BoxViewModel _SelectedBoxViewModel;
        public BoxViewModel SelectedBoxViewModel
        {
            set
            {
                _SelectedBoxViewModel = value;
                OnPropertyChanged("SelectedBoxViewModel");
            }
            get
            {
                return _SelectedBoxViewModel;
            }
        }

        private RelayCommand _CreateBoxCommand;
        public ICommand CreateBoxCommand
        {
            get
            {
                if (_CreateBoxCommand == null)
                {
                    _CreateBoxCommand = new RelayCommand(para => DoCreateBoxFile());
                }
                return _CreateBoxCommand;
            }
        }

        private void DoCreateBoxFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                DefaultExt = ".box", // Default file extension
                Filter = "Box File|*.box" // Filter files by extension 
            };

            // Show save file dialog box
            Nullable<bool> dialogResult = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (dialogResult == true)
            {
                //Open document 
                string filename = saveFileDialog.FileName;
                BoxViewModel boxViewModel = new BoxViewModel(filename);
                boxViewModel.CreateBoxFile(filename);
                _BoxViewModels.Add(boxViewModel);
                SelectedBoxViewModel = boxViewModel;
            }
            else
            {
                return;
            }
        }

        private RelayCommand _OpenBoxsCommand;
        public ICommand OpenBoxsCommand
        {
            get
            {
                if (_OpenBoxsCommand == null)
                {
                    _OpenBoxsCommand = new RelayCommand(para => DoOpenBoxsFile());
                }
                return _OpenBoxsCommand;
            }
        }

        private void DoOpenBoxsFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = ".box", // Default file extension
                Filter = "Box File|*.box" // Filter files by extension 
            };

            // Show open file dialog box
            Nullable<bool> dialogResult = openFileDialog.ShowDialog();

            // Process open file dialog box results 
            if (dialogResult == true)
            {
                // Open document 
                string[] filenames = openFileDialog.FileNames;
                foreach (string filename in filenames)
                {
                    BoxViewModel boxViewModel;
                    boxViewModel = GetBoxInBoxes(filename);
                    if (boxViewModel != null)
                    {
                        SelectedBoxViewModel = boxViewModel;
                        continue;
                    }
                    boxViewModel = new BoxViewModel(filename);
                    if (!boxViewModel.OpenBoxFile(filename))
                    {
                        continue;
                    }
                    _BoxViewModels.Add(boxViewModel);
                    SelectedBoxViewModel = boxViewModel;
                }
            }
            else
            {
                return;
            }
        }

        private RelayCommand _OpenBoxCommand;
        public ICommand OpenBoxCommand
        {
            get
            {
                if (_OpenBoxCommand == null)
                {
                    _OpenBoxCommand = new RelayCommand(para => DoOpenBoxFile());
                }
                return _OpenBoxCommand;
            }
        }

        private void DoOpenBoxFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".box", // Default file extension
                Filter = "Box File|*.box" // Filter files by extension 
            };

            // Show open file dialog box
            Nullable<bool> dialogResult = openFileDialog.ShowDialog();

            // Process open file dialog box results 
            if (dialogResult == true)
            {
                // Open document 
                string[] filenames = openFileDialog.FileNames;
                foreach (string filename in filenames)
                {
                    BoxViewModel boxViewModel;
                    boxViewModel = GetBoxInBoxes(filename);
                    if (boxViewModel != null)
                    {
                        SelectedBoxViewModel = boxViewModel;
                        continue;
                    }
                    boxViewModel = new BoxViewModel(filename);
                    if (!boxViewModel.OpenBoxFile(filename))
                    {
                        continue;
                    }
                    _BoxViewModels.Add(boxViewModel);
                    SelectedBoxViewModel = boxViewModel;
                }
            }
            else
            {
                return;
            }
        }

        private BoxViewModel GetBoxInBoxes(string boxFilePath)
        {
            foreach (BoxViewModel boxViewModel in BoxViewModels)
            {
                if (boxViewModel.BoxFilePath == boxFilePath)
                {
                    return boxViewModel;
                }
            }
            return null;
        }

        private RelayCommand _CloseBoxCommand;
        public ICommand CloseBoxCommand
        {
            get
            {
                if (_CloseBoxCommand == null)
                {
                    _CloseBoxCommand = new RelayCommand(para => DoCloseBoxFile(para));
                }
                return _CloseBoxCommand;
            }
        }

        private void DoCloseBoxFile(object para)
        {
            BoxViewModel boxViewModel = para as BoxViewModel;
            boxViewModel.DestoryIcons();
            BoxViewModels.Remove(boxViewModel);
        }


        private RelayCommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand(para => DoClose(para));
                }
                return _CloseCommand;
            }
        }

        private void DoClose(object para)
        {
            Window window = para as Window;
            //Storyboard stdEnd = (Storyboard)window.Resources["CloseWindow"];
            //stdEnd.Completed += (c, d) =>
            //{
            window.Close();
            //};
            //stdEnd.Begin();
        }

        private RelayCommand _ChangeSkinCommand;
        public ICommand ChangeSkinCommand
        {
            get
            {
                if (_ChangeSkinCommand == null)
                {
                    _ChangeSkinCommand = new RelayCommand(para => DoChangeSkin(para),
                    p => this.CanDoChangeSkinCommand(p));
                }
                return _ChangeSkinCommand;
            }
        }

        int stylenum = 1;
        private void DoChangeSkin(object para)
        {
            if (stylenum == 3)
            {
                stylenum = 1;
            }
            else
            {
                stylenum += 1;
            }
            Uri uri = new Uri(string.Format("Themes/Default{0}.xaml", stylenum), UriKind.Relative);
            Application.Current.Resources.MergedDictionaries[1].Source = uri;
        }

        private bool CanDoChangeSkinCommand(object para)
        {
            return para != null;
        }

        public MainViewModel(object para)
        {
            var itemsView = (IEditableCollectionView)CollectionViewSource.GetDefaultView(_BoxViewModels);
            itemsView.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtBeginning;

            Uri uri = new Uri(string.Format("Themes/Default{0}.xaml", stylenum), UriKind.Relative);
            ResourceDictionary res = new ResourceDictionary()
            {
                Source = uri
            };
            Application.Current.Resources.MergedDictionaries.Add(res);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
