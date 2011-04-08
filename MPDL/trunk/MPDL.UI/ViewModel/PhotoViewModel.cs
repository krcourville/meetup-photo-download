using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;
using MPDL.Domain.Model;

namespace MPDL.UI.ViewModel {
    public class PhotoViewModel : ViewModelBase {
        /// <summary>
        /// The <see cref="MeetupPhoto" /> property's name.
        /// </summary>
        public const string MeetupPhotoPropertyName = "MeetupPhoto";

        private MeetupPhoto meetupPhoto = null;

        /// <summary>
        /// Gets the MeetupPhoto property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MeetupPhoto MeetupPhoto {
            get {
                return meetupPhoto;
            }

            set {
                if (meetupPhoto == value) {
                    return;
                }

                var oldValue = meetupPhoto;
                meetupPhoto = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(MeetupPhotoPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool isSelected = false;

        /// <summary>
        /// Gets the IsSelected property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool IsSelected {
            get {
                return isSelected;
            }

            set {
                if (isSelected == value) {
                    return;
                }

                var oldValue = isSelected;
                isSelected = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(IsSelectedPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="ImageData" /> property's name.
        /// </summary>
        public const string ImageDataPropertyName = "ImageData";

        private BitmapImage imageData = null;

        /// <summary>
        /// Gets the ImageData property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public BitmapImage ImageData {
            get {
                return imageData;
            }

            set {
                if (imageData == value) {
                    return;
                }

                var oldValue = imageData;
                imageData = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(ImageDataPropertyName, oldValue, value, true);
            }
        }
    }
}
