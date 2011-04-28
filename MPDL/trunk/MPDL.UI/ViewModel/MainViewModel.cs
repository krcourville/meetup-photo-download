using GalaSoft.MvvmLight;
using MPDL.Domain.Service;
using MPDL.Domain.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System;
using System.Windows.Threading;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace MPDL.UI.ViewModel {
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase {
        #region Fields
        private IMPDLService svc;
        #endregion

        #region Init

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
            svc = new MPDLService();
            svc.SetErrorMessageCallback((e) => {
                IsBusy = false;
                MessageBox.Show(e.Message, "Application Error",
                                System.Windows.MessageBoxButton.OK,
                                System.Windows.MessageBoxImage.Error);

            });
            svc.SetBusymessageCallback((msg) => {
                BusyMessage = msg;
            });
            //
            // Load/set config
            if (!svc.ConfigIsSet()) {
                Config = new MPDLConfig();
                ConfigWindowState = WindowState.Open;
            } else {
                Config = svc.GetConfig();
            }
            //
            // Sample data for design
            if (IsInDesignMode) {
                // Code runs in Blend --> create design time data.
                Groups.Add(new MeetupGroup { GroupId = 1, Name = "Group1", UrlName = "Group1Url" });
                Groups.Add(new MeetupGroup { GroupId = 2, Name = "Group2", UrlName = "Group2Url" });
                Groups.Add(new MeetupGroup { GroupId = 3, Name = "Group3", UrlName = "Group3Url" });
            } else {
                // Code runs "for real"
                if (svc.HasGroupsCached()) {
                    svc.GetAllGroups(false)
                        .OrderBy(o => o.Name)
                        .ToList()
                        .ForEach(g => groups.Add(g));
                }
            }
            DefineCommands();
            PropertyChanged += MainViewModel_PropertyChanged;
        }

        #endregion

        #region Events
        void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case SelectedGroupPropertyName:
                    thumbs.Clear();
                    IsBusy = true;
                    svc.GetAlbumsAsync(false, l => {
                        Albums.Clear();
                        l.OrderByDescending(o => o.DateCreated)
                            .ToList()
                            .ForEach(a => Albums.Add(a));
                        IsBusy = false;
                    }, SelectedGroup.GroupId);
                    break;

                case SelectedAlbumPropertyName:
                    DownloadThumbnails(false);
                    break;

                case ConfigWindowStatePropertyName:
                    if (ConfigWindowState == WindowState.Closed) {
                        svc.SetConfig(Config);
                        DownloadGroupsCommand.Execute(null);
                    }

                    break;
            }
        }
        #endregion

        #region Collections
        private ObservableCollection<MeetupGroup> groups = new ObservableCollection<MeetupGroup>();
        public ObservableCollection<MeetupGroup> Groups { get { return groups; } }

        private ObservableCollection<MeetupAlbum> albums = new ObservableCollection<MeetupAlbum>();
        public ObservableCollection<MeetupAlbum> Albums { get { return albums; } }

        private ObservableCollection<PhotoViewModel> thumbs = new ObservableCollection<PhotoViewModel>();
        public ObservableCollection<PhotoViewModel> Thumbs { get { return thumbs; } }

        #endregion

        #region Commands
        public RelayCommand SelectAllPhotosCommand { get; private set; }
        public RelayCommand DeselectAllPhotosCommand { get; private set; }
        public RelayCommand DownloadGroupsCommand { get; private set; }
        public RelayCommand DownloadAlbumsCommand { get; private set; }
        public RelayCommand DownloadThumbsCommand { get; private set; }
        public RelayCommand ToggleConfigWindowCommand { get; private set; }
        public RelayCommand DownloadHighResCommand { get; private set; }
        public RelayCommand OpenHighResFolderCommand { get; private set; }

        private void DefineCommands() {
            //
            OpenHighResFolderCommand = new RelayCommand(() => {
                string downloadFolder = svc.GetHighResDownloadFolder();
                Process.Start(downloadFolder);
            });
            //
            DownloadHighResCommand = new RelayCommand(() => {
                var selected = Thumbs
                    .Where(w => w.IsSelected);

                var toDownload = selected.Select(s => s.MeetupPhoto).ToList();
                IsBusy = true;
                svc.DownloadHighResAsync(toDownload, (m) => {
                    IsBusy = false;
                    foreach (var item in selected)
                        item.IsSelected = false;
                });
            }, () => {
                return Thumbs.Where(w => w.IsSelected).Count() > 0;
            });
            //
            SelectAllPhotosCommand = new RelayCommand(() => {
                foreach (var item in Thumbs) {
                    item.IsSelected = true;
                }
            }, () => {
                return Thumbs.Count > 0;
            });
            DeselectAllPhotosCommand = new RelayCommand(() => {
                foreach (var item in Thumbs) {
                    item.IsSelected = false;
                }
            }, () => Thumbs.Count > 0);

            //
            ToggleConfigWindowCommand = new RelayCommand(() => {
                switch (ConfigWindowState) {
                    case WindowState.Closed:
                        ConfigWindowState = WindowState.Open;
                        break;
                    case WindowState.Open:
                        ConfigWindowState = WindowState.Closed;
                        break;
                    default:
                        break;
                }
            });
            //
            DownloadGroupsCommand = new RelayCommand(() => {
                IsBusy = true;
                svc.GetAllGroupsAsync(true, l => {
                    groups.Clear();
                    l.OrderBy(o => o.Name)
                        .ToList()
                        .ForEach(g => groups.Add(g));
                    IsBusy = false;
                });
            });
            //
            DownloadAlbumsCommand = new RelayCommand(() => {
                IsBusy = true;
                svc.GetAlbumsAsync(true, l => {
                    albums.Clear();
                    l.OrderByDescending(o => o.DateCreated)
                        .ToList()
                        .ForEach(a => albums.Add(a));
                    IsBusy = false;
                }, SelectedGroup.GroupId);
            }
            , () => SelectedGroup != null);
            //
            DownloadThumbsCommand = new RelayCommand(() => DownloadThumbnails(true), () => SelectedAlbum != null);

        }

        private void DownloadThumbnails(bool forceDownload) {
            IsBusy = true;
            thumbs.Clear();
            svc.GetThumbnailsAsync(forceDownload, l => {
                l.ToList()
                    .ForEach(p => {
                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.UriSource = new Uri(p.ThumbUrl);
                        bi.EndInit();
                        thumbs.Add(new PhotoViewModel {
                            ImageData = bi,
                            MeetupPhoto = p
                        });
                    });
                IsBusy = false;
                SelectAllPhotosCommand.RaiseCanExecuteChanged();
                DeselectAllPhotosCommand.RaiseCanExecuteChanged();
            }, SelectedAlbum.AlbumId);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="BusyMessage" /> property's name.
        /// </summary>
        public const string BusyMessagePropertyName = "BusyMessage";

        private string busyMessage = "Processing";

        /// <summary>
        /// Gets the BusyMessage property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string BusyMessage {
            get {
                return busyMessage;
            }

            set {
                if (busyMessage == value) {
                    return;
                }

                var oldValue = busyMessage;
                busyMessage = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(BusyMessagePropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="Config" /> property's name.
        /// </summary>
        public const string ConfigPropertyName = "Config";

        private MPDLConfig config = null;

        /// <summary>
        /// Gets the Config property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MPDLConfig Config {
            get {
                return config;
            }

            set {
                if (config == value) {
                    return;
                }

                var oldValue = config;
                config = value;


                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(ConfigPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="ConfigWindowState" /> property's name.
        /// </summary>
        public const string ConfigWindowStatePropertyName = "ConfigWindowState";

        private WindowState configWindowState = WindowState.Closed;

        /// <summary>
        /// Gets the ConfigWindowState property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public WindowState ConfigWindowState {
            get {
                return configWindowState;
            }

            set {
                if (configWindowState == value) {
                    return;
                }

                var oldValue = configWindowState;
                configWindowState = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(ConfigWindowStatePropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="SelectedAlbum" /> property's name.
        /// </summary>
        public const string SelectedAlbumPropertyName = "SelectedAlbum";

        private MeetupAlbum selectedAlbum = null;

        /// <summary>
        /// Gets the SelectedAlbum property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MeetupAlbum SelectedAlbum {
            get {
                return selectedAlbum;
            }

            set {
                if (selectedAlbum == value) {
                    return;
                }

                var oldValue = selectedAlbum;
                selectedAlbum = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(SelectedAlbumPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="SelectedGroup" /> property's name.
        /// </summary>
        public const string SelectedGroupPropertyName = "SelectedGroup";

        private MeetupGroup selectedGroup = null;

        /// <summary>
        /// Gets the SelectedGroup property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MeetupGroup SelectedGroup {
            get {
                return selectedGroup;
            }

            set {
                if (selectedGroup == value) {
                    return;
                }

                var oldValue = selectedGroup;
                selectedGroup = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(SelectedGroupPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsBusy" /> property's name.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";

        private bool isBusy = false;

        /// <summary>
        /// Gets the IsBusy property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool IsBusy {
            get {
                return isBusy;
            }

            set {
                if (isBusy == value) {
                    return;
                }

                var oldValue = isBusy;
                isBusy = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(IsBusyPropertyName, oldValue, value, true);
            }
        }

        #endregion

        public override void Cleanup() {
            // Clean up if needed
            PropertyChanged -= MainViewModel_PropertyChanged;
            base.Cleanup();
        }
    }
}