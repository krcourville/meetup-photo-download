using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPDL.Domain {
    public static class Constants {
        public const string GroupQueryTemplate = @"https://api.meetup.com/groups.xml/?key={0}&member_id={1}";
        public const string AlbumQueryTemplate = @"https://api.meetup.com/2/photo_albums?key={0}&sign=true&group_id={1}&format=xml";
        public const string PhotoQueryTemplate = @"https://api.meetup.com/2/photos?key={0}&sign=true&photo_album_id={1}&format=xml";
        public const string GroupsFileName = "groups.xml";
        public const string ConfigFileName = "config.xml";
        public const string CacheFolder = "cache";
    }
}
