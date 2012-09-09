using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using MPDL.Domain.Model;

namespace MPDL.UI.ViewModel
{
    public class AlbumViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="MeetupAlbum" /> property's name.
        /// </summary>
        public const string MeetupAlbumPropertyName = "MeetupAlbum";

        private MeetupAlbum meetupAlbum = null;

        /// <summary>
        /// Gets the MeetupPhoto property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public MeetupAlbum MeetupAlbum
        {
            get
            {
                return meetupAlbum;
            }

            set
            {
                if (meetupAlbum == value)
                {
                    return;
                }

                var oldValue = meetupAlbum;
                meetupAlbum = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(MeetupAlbumPropertyName, oldValue, value, true);
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
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                if (isSelected == value)
                {
                    return;
                }

                var oldValue = isSelected;
                isSelected = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(IsSelectedPropertyName, oldValue, value, true);
            }
        }
    }
}
